using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CWB.App.Services.EmailServices
{
    public class KafkaEmailProducer
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        public KafkaEmailProducer(string bootstrapServers, string topic)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
            _topic = topic;
        }

        public async Task SendEmailRequestAsync(string recipientEmail, string subject, string body)
        {
            var payload = new EmailPayload
            {
                RecipientEmail = recipientEmail,
                Subject = subject,
                Body = body
            };

            var json = JsonSerializer.Serialize(payload);

            await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = json });

            // Optional flush
            _producer.Flush();
        }
    }
}

public class EmailPayload
{
    public string RecipientEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}