using Google.Apis.Auth.OAuth2.Responses;
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
using Ubigrade.Library.Google;

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
            private readonly IUserStore<IdentityUser> store;
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
                //ClaimsPrincipal cp = await innerFactory.CreateAsync(user);
                var cp =  ClaimsPrincipal.Current.Identities.First();
                var info = await this.signInManager.GetExternalLoginInfoAsync();
                if (info != null)
                {
                    List<Claim> claimsToAdd = new List<Claim>();

                    foreach (var claim in info.Principal.Claims)
                    {
                        claimsToAdd.Add(claim);
                    }
                    claimsToAdd.Add(new Claim("Google_issued_utc", info.AuthenticationProperties.IssuedUtc.ToString()));

                    foreach (var token in info.AuthenticationTokens)
                    {
                        claimsToAdd.Add(new Claim("Google_" + token.Name, token.Value));
                    }

                    ClaimsIdentity googleIdentity = new ClaimsIdentity(claimsToAdd, "Google");
                    var principal = new ClaimsPrincipal();
                    principal.AddIdentity(googleIdentity);
                    var credential = GoogleProviderHelper.CreateUserCredential(principal);
                    var profile = await GetGoogleData.GetClassroomUserProfile(credential);
                    var test = profile.VerifiedTeacher;
                    if (profile.VerifiedTeacher.HasValue)
                        googleIdentity.AddClaim(new Claim(ClaimTypes.Role, "Teacher"));
                    if (!profile.VerifiedTeacher.HasValue)
                        googleIdentity.AddClaim(new Claim(ClaimTypes.Role, "Student"));

                    //var i = cp.Identity as ClaimsIdentity;
                    //i.AddClaims(claimsToAdd);
                    ClaimsPrincipal.Current.AddIdentity(googleIdentity);
                }
                return ClaimsPrincipal.Current;
            }
            #region test
            //public async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
            //{
            //    ClaimsPrincipal cp = await innerFactory.CreateAsync(user);
            //    var info = await this.signInManager.GetExternalLoginInfoAsync();
            //    if (info != null)
            //    {
            //        List<Claim> claimsToAdd = new List<Claim>();

            //        foreach (var claim in info.Principal.Claims)
            //        {
            //            claimsToAdd.Add(claim);
            //        }
            //        claimsToAdd.Add(new Claim("Google_userid", info.ProviderKey));
            //        claimsToAdd.Add(new Claim("Google_issued_utc", info.AuthenticationProperties.IssuedUtc.ToString()));

            //        foreach (var token in info.AuthenticationTokens)
            //        {
            //            claimsToAdd.Add(new Claim("Google_" + token.Name, token.Value));
            //        }
            //        ClaimsIdentity googleIdentity = new ClaimsIdentity(claimsToAdd, "Google");
            //        var principal = new ClaimsPrincipal();
            //        principal.AddIdentity(googleIdentity);
            //        var credential = GoogleProviderHelper.CreateUserCredential(principal);
            //        var profile = await GetGoogleData.GetClassroomUserProfile(credential);
            //        var test = profile.VerifiedTeacher;
            //        if (profile.VerifiedTeacher.Value || profile.VerifiedTeacher == null)
            //            googleIdentity.AddClaim(new Claim(ClaimTypes.Role, "Teacher"));

            //        var i = cp.Identity as ClaimsIdentity;

            //        cp.AddIdentity(googleIdentity);

            //    }

            //    return cp;
            //}
            #endregion
        }
    }
}
