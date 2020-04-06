using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Ubigrade.Application.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string LoginProvider { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        //komm reine wenn ich auf die blaue google taste drücke
        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        //da komm ich rein nachdem ich auf meine email bei google drück um mich einzuloggen.
        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new {ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor : true);
            //problem hier bei nip io adresse. ^
            if (result.Succeeded)
            {
                // Store the access token and resign in so the token is included in
                // in the cookie
                ////var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                ////var existingClaims = await _userManager.GetClaimsAsync(user);

                ////var props = new AuthenticationProperties();
                ////props.StoreTokens(info.AuthenticationTokens);
                ////props.IsPersistent = true;

                ////List<Claim> claimsToRemove = new List<Claim>();
                ////foreach (var existingClaim in existingClaims)
                ////{
                ////    if (existingClaim.Type.StartsWith("Google"))
                ////        claimsToRemove.Add(existingClaim);
                ////}
                ////foreach (var existingClaim in info.Principal.Claims)
                ////{
                ////    claimsToRemove.Add(existingClaim);
                ////}

                ////if (claimsToRemove.Count > 0)
                ////    await _userManager.RemoveClaimsAsync(user, claimsToRemove);

                ////// TODO: write a helper class to manage the claims; don't forget to remove existing ones!
                ////List<Claim> claimsToAdd = new List<Claim>();

                ////foreach (var claim in info.Principal.Claims)
                ////{
                ////    claimsToAdd.Add(claim);
                ////}
                ////claimsToAdd.Add(new Claim("GoogleUserId", info.ProviderKey));
                ////foreach (var token in info.AuthenticationTokens)
                ////{
                ////    claimsToAdd.Add(new Claim("Google" + token.Name, token.Value));
                ////}
                ////await _userManager.AddClaimsAsync(user, claimsToAdd);
                // ?? notneeded ?? await _signInManager.SignInAsync(user, props);

                ////ClaimsIdentity googleIdentity = new ClaimsIdentity(claimsToAdd, "Google");
                ////_signInManager.Context.User.AddIdentity(googleIdentity);
                ////await _signInManager.Context.SignInAsync(_signInManager.Context.User, props);

                // ONLY TO STORE TOKENS IN EXTERNAL DB
                // Store the authentication tokens in the external database ...
                // await _signInManager.UpdateExternalAuthenticationTokensAsync(info);

                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                LoginProvider = info.LoginProvider;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        // If they exist, add claims to the user for:
                        //    Given (first) name
                        //    Locale
                        //    Picture
                        if (info.Principal.HasClaim(c => c.Type == ClaimTypes.GivenName))
                        {
                            await _userManager.AddClaimAsync(user,
                                info.Principal.FindFirst(ClaimTypes.GivenName));
                            await _userManager.AddClaimAsync(user,
                                info.Principal.FindFirst(ClaimTypes.Email));
                        }

                        // Include the access token in the properties
                        var props = new AuthenticationProperties();
                        props.StoreTokens(info.AuthenticationTokens);
                        props.IsPersistent = true;
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            LoginProvider = info.LoginProvider;
            ReturnUrl = returnUrl;
            return Page();
        }
        ////internal class TestClaimsFactory : IUserClaimsPrincipalFactory<IdentityUser>
        ////{
        ////    private SignInManager<IdentityUser> _signInManager;
        ////    private IUserClaimsPrincipalFactory<IdentityUser> _previousFactory;

        ////    public TestClaimsFactory(SignInManager<IdentityUser> signInManager, IUserClaimsPrincipalFactory<IdentityUser> previousFactory)
        ////    {
        ////        _signInManager = signInManager;
        ////        _previousFactory = previousFactory;
        ////    }

        ////    public async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
        ////    {
        ////        ClaimsPrincipal cp = await _previousFactory.CreateAsync(user);

        ////        var info = await _signInManager.GetExternalLoginInfoAsync();
        ////        if (info != null)
        ////        {
        ////            List<Claim> claimsToAdd = new List<Claim>();

        ////            foreach (var claim in info.Principal.Claims)
        ////            {
        ////                claimsToAdd.Add(claim);
        ////            }
        ////            claimsToAdd.Add(new Claim("GoogleUserId", info.ProviderKey));
        ////            foreach (var token in info.AuthenticationTokens)
        ////            {
        ////                claimsToAdd.Add(new Claim("Google" + token.Name, token.Value));
        ////            }

        ////            ClaimsIdentity googleIdentity = new ClaimsIdentity(claimsToAdd, "Google");
        ////            cp.AddIdentity(googleIdentity);
        ////        }

        ////        return cp;
        ////    }
        ////}
    }
}
