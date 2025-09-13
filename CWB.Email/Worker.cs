using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private IConsumer<Ignore, string> _consumer;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Build consumer config from env / configuration
        var bootstrap = _configuration["KAFKA_BOOTSTRAP"] ?? "kafka:9092";
        var groupId = _configuration["KAFKA_GROUP"] ?? "email-consumer-group";
        var topic = _configuration["KAFKA_TOPIC"] ?? "email-events";

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = bootstrap,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };

        _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig)
            .SetErrorHandler((_, e) => _logger.LogError("Kafka error: {Reason}", e.Reason))
            .Build();

        _consumer.Subscribe(topic);
        _logger.LogInformation("Kafka Consumer started. Bootstrap={Bootstrap} GroupId={GroupId} Topic={Topic}", bootstrap, groupId, topic);

        // run consume loop in background
        Task.Run(() => ConsumeLoop(stoppingToken), stoppingToken);

        return Task.CompletedTask;
    }

    private void ConsumeLoop(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var cr = _consumer.Consume(stoppingToken);
                if (cr?.Message?.Value == null) continue;

                _logger.LogInformation("Consumed message at {TP}: {Value}", cr.TopicPartitionOffset, cr.Message.Value);

                EmailPayload email;
                try
                {
                    email = JsonSerializer.Deserialize<EmailPayload>(cr.Message.Value);
                    if (email == null)
                    {
                        _logger.LogWarning("Deserialized payload is null. Raw: {Raw}", cr.Message.Value);
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to deserialize payload: {Raw}", cr.Message.Value);
                    continue;
                }

                // validate
                if (string.IsNullOrWhiteSpace(email.RecipientEmail))
                {
                    _logger.LogWarning("Payload missing RecipientEmail: {Payload}", cr.Message.Value);
                    continue;
                }

                // attempt send (with retry)
                SendEmailWithRetryAsync(email, stoppingToken).GetAwaiter().GetResult();
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (ConsumeException cex)
            {
                _logger.LogError(cex, "Consume exception");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in consume loop");
            }
        }

        try { _consumer.Close(); } catch { }
    }

    private async Task SendEmailWithRetryAsync(EmailPayload payload, CancellationToken cancellationToken)
    {
        int maxAttempts = 3;
        int attempt = 0;
        Exception lastEx = null;

        while (++attempt <= maxAttempts && !cancellationToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Attempt {Attempt} to send email to {Recipient}", attempt, payload.RecipientEmail);
                await SendEmailAsync(payload, cancellationToken);
                _logger.LogInformation("Email successfully sent to {Recipient}", payload.RecipientEmail);
                return;
            }
            catch (Exception ex)
            {
                lastEx = ex;
                _logger.LogError(ex, "Send attempt {Attempt} failed for {Recipient}", attempt, payload.RecipientEmail);
                if (attempt < maxAttempts)
                {
                    await Task.Delay(2000 * attempt, cancellationToken); // exponential-ish backoff
                }
            }
        }

        _logger.LogError(lastEx, "Failed to send email to {Recipient} after {Attempts} attempts", payload.RecipientEmail, maxAttempts);
    }

    private async Task SendEmailAsync(EmailPayload payload, CancellationToken cancellationToken)
    {
        // read smtp config from IConfiguration
        var host = _configuration["SmtpSettings:Host"];
        var portStr = _configuration["SmtpSettings:Port"];
        var user = _configuration["SmtpSettings:Username"];
        var pass = _configuration["SmtpSettings:Password"];
        var from = _configuration["SmtpSettings:From"] ?? user;
        var useStartTls = bool.TryParse(_configuration["SmtpSettings:UseStartTls"], out var ust) ? ust : true;
        var allowInvalidSsl = bool.TryParse(_configuration["SmtpSettings:AllowInvalidSsl"], out var a) ? a : false;

        if (string.IsNullOrWhiteSpace(host))
        {
            throw new InvalidOperationException("SMTP host is not configured (SmtpSettings:Host).");
        }

        if (!int.TryParse(portStr, out var port))
        {
            port = useStartTls ? 587 : 465;
            _logger.LogInformation("Smtp port not configured or invalid. Using default {Port}", port);
        }

        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(from));
        message.To.Add(MailboxAddress.Parse(payload.RecipientEmail));
        message.Subject = payload.Subject ?? "(no subject)";
        message.Body = new TextPart("plain") { Text = payload.Body ?? string.Empty };

        using var client = new SmtpClient();

        // optional: allow invalid ssl for debugging only
        if (allowInvalidSsl)
        {
            _logger.LogWarning("AllowInvalidSsl=true. SSL certificate errors will be ignored (debug only).");
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
        }

        SecureSocketOptions socketOption = useStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.SslOnConnect;

        _logger.LogInformation("Connecting to SMTP server {Host}:{Port} with {SocketOption}", host, port, socketOption);
        await client.ConnectAsync(host, port, socketOption, cancellationToken);

        if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass))
        {
            _logger.LogInformation("Authenticating SMTP user {User}", user);
            await client.AuthenticateAsync(user, pass, cancellationToken);
        }
        else
        {
            _logger.LogInformation("No SMTP credentials provided; attempting unauthenticated send (may be rejected).");
        }

        _logger.LogInformation("Sending email to {Recipient}", payload.RecipientEmail);
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}

public class EmailPayload
{
    public string RecipientEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}
