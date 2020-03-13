using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ubigrade.Application
{
    public class GoogleAwareSignInManager : SignInManager<IdentityUser>
    {
        public GoogleAwareSignInManager(UserManager<IdentityUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<IdentityUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<IdentityUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<IdentityUser> confirmation)
            : base(userManager, contextAccessor, new GoogleAwareClaimsFactory(claimsFactory), optionsAccessor, logger, schemes, confirmation)
        {
            GoogleAwareClaimsFactory gacf = base.ClaimsFactory as GoogleAwareClaimsFactory;
            gacf.SetSignInManager(this);
        }

        private class GoogleAwareClaimsFactory : IUserClaimsPrincipalFactory<IdentityUser>
        {
            private readonly IUserClaimsPrincipalFactory<IdentityUser> innerFactory;
            private GoogleAwareSignInManager signInManager;

            public GoogleAwareClaimsFactory(IUserClaimsPrincipalFactory<IdentityUser> innerFactory)
            {
                this.innerFactory = innerFactory;
            }

            public void SetSignInManager(GoogleAwareSignInManager signInManager)
            {
                this.signInManager = signInManager;
            }

            public async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
            {
                ClaimsPrincipal cp = await innerFactory.CreateAsync(user);

                var info = await this.signInManager.GetExternalLoginInfoAsync();
                if (info != null)
                {
                    List<Claim> claimsToAdd = new List<Claim>();

                    foreach (var claim in info.Principal.Claims)
                    {
                        claimsToAdd.Add(claim);
                    }
                    claimsToAdd.Add(new Claim("Google_userid", info.ProviderKey));
                    claimsToAdd.Add(new Claim("Google_issued_utc", info.AuthenticationProperties.IssuedUtc.ToString()));
                    foreach (var token in info.AuthenticationTokens)
                    {
                        claimsToAdd.Add(new Claim("Google_" + token.Name, token.Value));
                    }

                    ClaimsIdentity googleIdentity = new ClaimsIdentity(claimsToAdd, "Google");
                    cp.AddIdentity(googleIdentity);
                }

                return cp;
            }
        }
    }
}
