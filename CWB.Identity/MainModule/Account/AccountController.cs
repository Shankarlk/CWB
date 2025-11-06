// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using CWB.CommonUtils.Common;
using CWB.CommonUtils.KafkaConfigs;
using CWB.CommonUtils.MessageBrokers;
using CWB.Constants.MessageBrokers;
using CWB.Constants.UserIdentity;
using CWB.Identity.Domain;
using CWB.Identity.IdentityUtils;
using CWB.Identity.Services;
using CWB.Identity.ViewModels;
using CWB.Logging;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CWB.Identity
{
    /// <summary>
    /// This sample controller implements a typical login/logout/provision workflow for local and external accounts.
    /// The login service encapsulates the interactions with the user data store. This data store is in-memory only and cannot be used for production!
    /// The interaction service provides a way for the UI to communicate with identityserver for validation and context retrieval
    /// </summary>
    [SecurityHeaders]
    //[AllowAnonymous]
    public class AccountController : Controller
    {

        private readonly UserManager<CwbUser> _userManager;
        private readonly SignInManager<CwbUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly ApiUrls _apiUrls;
        private readonly UserService _userService;

        private readonly IMessageBroker _messageBroker;
        private readonly KafkaConfig _kafkaConfig;
        private readonly ILoggerManager _logger;

        public AccountController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            UserManager<CwbUser> userManager,
            SignInManager<CwbUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<ApiUrls> apiUrlsOptions,
            UserService userService,
            IMessageBroker messageBroker,
            IOptions<KafkaConfig> kafkaConfigOptions, ILoggerManager logger
            )
        {

            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _apiUrls = apiUrlsOptions.Value;
            _userService = userService;
            _messageBroker = messageBroker;
            _kafkaConfig = kafkaConfigOptions.Value;
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // build a model so we know what to show on the login page
            var vm = await BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return RedirectToAction("Challenge", "External", new { scheme = vm.ExternalLoginScheme, returnUrl });
            }

            return View(vm);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            if (button != "login")
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (context.IsNativeClient())
                    {
                        // The client is native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.LoadingPage("Redirect", model.ReturnUrl);
                    }

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, true);
                // validate username/password against in-memory store
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.FirstName + " " + user.LastName, clientId: context?.Client.ClientId));

                    if (user.ChangedPassword == 1) // <-- your user table property
                    {
                        // redirect to ChangePassword screen
                        return RedirectToAction("ChangePassword", "Account", new { returnUrl = model.ReturnUrl });
                    }
                    // only set explicit expiration here if user chooses "remember me". 
                    // otherwise we rely upon expiration configured in cookie middleware.
                    AuthenticationProperties props = null;
                    if (AccountOptions.AllowRememberLogin && model.RememberLogin)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                        };
                    };

                    if (context != null)
                    {
                        if (context.IsNativeClient())
                        {
                            // The client is native, so this change in how to
                            // return the response is for better UX for the end user.
                            return this.LoadingPage("Redirect", model.ReturnUrl);
                        }

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(model.ReturnUrl);
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("invalid return URL");
                    }
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }


        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(vm);
            }

            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await _signInManager.SignOutAsync();

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if (vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [Authorize(Roles = Roles.SUPERADMIN)]
        [HttpGet]
        public async Task<IActionResult> AddAdmin(string returnUrl)
        {
            // build a model so we know what to show on the login page
            RegisterViewModel model = new RegisterViewModel();
            model.ReturnUrl = returnUrl;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddAdmin(RegisterViewModel model, string button)
        {
            if (button == "cancel")
                return RedirectToAction("AdminList");

            if (!ModelState.IsValid)
                return View("Register", model);

            var existingUser = await _userManager.FindByNameAsync(model.Username);
            var finduser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Username already exists.");
                return View("Register", model);
            }
            if (finduser != null)
            {
                ModelState.AddModelError("", "EmailId already exists.");
                return View("Register", model);
            }

            var user = new CwbUser
            {
                UserName = model.Username,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                FirstName = model.FirstName,
                LastName = model.LastName,
                TenantId = 1 // Or derive from model.ClientCode if multi-tenant
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View("Register", model);
            }

            await _userManager.AddToRoleAsync(user, Roles.ADMIN);

            await _userManager.AddClaimsAsync(user, new[]
            {
                new Claim(JwtClaimTypes.Name, $"{model.FirstName} {model.LastName}"),
                new Claim(JwtClaimTypes.GivenName, model.FirstName),
                new Claim(JwtClaimTypes.FamilyName, model.LastName),
                new Claim(JwtClaimTypes.Role, Roles.ADMIN),
                new Claim("TenantId", model.ClientCode) // Optional: Use ClientCode or map it
            });

            TempData["SuccessMessage"] = "Admin user created successfully!";
            return RedirectToAction("AdminList");
        }

        [Authorize(Roles = Roles.SUPERADMIN)]
        [HttpGet]
        public async Task<IActionResult> AdminList()
        {
            var users = await _userManager.Users
                .Where(u => u.TenantId == 1)
                .ToListAsync();

            var admins = new List<CwbUser>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Admin"))  // Match your defined `Roles.ADMIN`
                {
                    admins.Add(user);
                }
            }

            return View(admins); // Send to AdminList.cshtml
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new CwbUser
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PhoneNumber = model.PhoneNumber
            };

            try
            {
                var finduser = await _userManager.FindByEmailAsync(model.Email);
                if(finduser != null)
                {
                    finduser.UserName = model.Username;
                    finduser.FirstName = model.FirstName;
                    finduser.LastName = model.LastName;
                    finduser.PhoneNumber = model.PhoneNumber;

                    var updateResult = await _userManager.UpdateAsync(finduser);
                    if (updateResult.Succeeded)
                    {
                        var roleResult = await _userManager.AddToRoleAsync(finduser, Roles.ADMIN);
                        var claims = new[]
                        {
            new Claim(JwtClaimTypes.Name, $"{finduser.FirstName} {finduser.LastName}"),
            new Claim(JwtClaimTypes.GivenName, finduser.FirstName),
            new Claim(JwtClaimTypes.FamilyName, finduser.LastName),
            new Claim(JwtClaimTypes.Role, Roles.ADMIN),
            new Claim("TenantId", model.TenantId)
        };
                        var claimsResult = await _userManager.AddClaimsAsync(finduser, claims);
                    }
                }
                else
                {
                    user.ChangedPassword = 1;
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        var roleResult = await _userManager.AddToRoleAsync(user, Roles.ADMIN);
                        if (!roleResult.Succeeded)
                        {
                            return Ok(model);
                        }

                        // Add claims for the user
                        var claims = new[]
                        {
            new Claim(JwtClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(JwtClaimTypes.GivenName, user.FirstName),
            new Claim(JwtClaimTypes.FamilyName, user.LastName),
            new Claim(JwtClaimTypes.Role, Roles.ADMIN),
            new Claim("TenantId", model.TenantId)
        };
                        var claimsResult = await _userManager.AddClaimsAsync(user, claims);
                        if (!claimsResult.Succeeded)
                        {
                            return Ok(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            //if (result.Succeeded)
            //{
            //    // Ensure the required roles exist
            //    if (!await _roleManager.RoleExistsAsync("User"))
            //    {
            //        var roleResult = await _roleManager.CreateAsync(new IdentityRole("User"));
            //        if (!roleResult.Succeeded)
            //        {
            //            foreach (var error in roleResult.Errors)
            //            {
            //                ModelState.AddModelError(string.Empty, error.Description);
            //            }
            //            return Ok(model);
            //        }
            //    }

            //    //        // Assign the "User" role to the newly registered user
            //    var roleAssignResult = await _userManager.AddToRoleAsync(user, "User");
            //    if (!roleAssignResult.Succeeded)
            //    {
            //        foreach (var error in roleAssignResult.Errors)
            //        {
            //            ModelState.AddModelError(string.Empty, error.Description);
            //        }
            //        return View(model);
            //    }

            //    // Optionally add claims for the user
            //    var claims = new[]
            //    {
            //        new Claim(JwtClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            //        new Claim(JwtClaimTypes.GivenName, user.FirstName),
            //        new Claim(JwtClaimTypes.FamilyName, user.LastName),
            //        new Claim(JwtClaimTypes.Role, "User")
            //    };
            //    var claimsResult = await _userManager.AddClaimsAsync(user, claims);
            //    if (!claimsResult.Succeeded)
            //    {
            //        foreach (var error in claimsResult.Errors)
            //        {
            //            ModelState.AddModelError(string.Empty, error.Description);
            //        }
            //        return Ok(model);
            //    }

            //    // Redirect to a confirmation page
            //    return Ok("RegisterConfirmation");
            //}

            // Handle errors during user creation
            //foreach (var error in result.Errors)
            //{
            //    ModelState.AddModelError(string.Empty, error.Description);
            //}

            return Ok(model);
        }
            //[HttpGet]
            //public async Task<IActionResult> Register(string returnUrl)
            //{
            //    // build a model so we know what to show on the reg page
            //    var vm = await BuildRegisterViewModelAsync(returnUrl);

            //    return View(vm);
            //}

            [HttpGet]
        [Authorize]
        public IActionResult ChangePassword(string returnUrl)
        {
            var vm = BuildChangePasswordViewModel(returnUrl);

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Generate reset token
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                // Reset password using token
                user.ChangedPassword = 1;
                var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

                if (result.Succeeded)
                {
                    return Ok("Password has been reset successfully.");
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = (HttpContext.User.Identity as ClaimsIdentity);

            var email = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var appUser = await _userManager.FindByEmailAsync(email);
            appUser.ChangedPassword = 0;
            var result = await _userManager.ChangePasswordAsync(appUser, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View(model);
            }

            return Redirect(model.ReturnUrl);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model, string button)
        {
            if (button == "cancel")
                return Redirect(_apiUrls.App);
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userService.GetUserbyUsernameEmail(model.EmailUserName);
            if (user == null)
            {
                ModelState.AddModelError(nameof(model.EmailUserName), $"User not found with Username or Email: {model.EmailUserName}");
                return View(model);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(Reset), "Account", new { token, email = user.Email }, Request.Scheme);
            //add to message broker on tenant request
            var messageInfo = new MessageInfo
            {
                MessageObject = new ForgotPasswordMessageVM
                {
                    DisplayName = user.FirstName + " " + user.LastName,
                    Email = user.Email,
                    Link = callback
                },
                MessageType = MessageTypes.ForgotUsername
            };

            await _messageBroker.SendAsync(_kafkaConfig, messageInfo);
            return RedirectToAction(nameof(ForgotPasswordSuccess));
        }

        public IActionResult ForgotPasswordSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotUsername()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotUsername(ForgotUsernameViewModel model, string button)
        {
            if (button == "cancel")
                return Redirect(_apiUrls.App);
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userService.GetUserbyPhoneEmail(model.PhoneEmail);
            if (user == null)
            {
                ModelState.AddModelError(nameof(model.PhoneEmail), $"User not found with Phone number or Email: {model.PhoneEmail}");
                return View(model);
            }
            //add to message broker on tenant request
            var messageInfo = new MessageInfo
            {
                MessageObject = new ForgotUsernameMessageVM
                {
                    DisplayName = user.FirstName + " " + user.LastName,
                    Email = user.Email,
                    Username = user.UserName
                },
                MessageType = MessageTypes.ForgotUsername
            };

            await _messageBroker.SendAsync(_kafkaConfig, messageInfo);
            return RedirectToAction(nameof(ForgotUserSuccess));

        }

        public IActionResult ForgotUserSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Reset(string token, string email)
        {
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Reset(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                ModelState.AddModelError("Error", $"Invalid Request please try again!");
            var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View(model);
            }
            return RedirectToAction(nameof(ResetConfirmation));
        }

        [HttpGet]
        public IActionResult ResetConfirmation()
        {
            ViewBag.LoginLink = _apiUrls.App + "/Home/Login";
            return View();
        }

        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }

                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }

        private async Task<RegisterViewModel> BuildRegisterViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                var vm = new RegisterViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }

                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new RegisterViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private ChangePasswordViewModel BuildChangePasswordViewModel(string returnUrl)
        {

            return new ChangePasswordViewModel
            {
                ReturnUrl = string.IsNullOrEmpty(returnUrl) ? _apiUrls.App : returnUrl
            };
        }
    }
}
