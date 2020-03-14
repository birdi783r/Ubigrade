using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;

namespace Ubigrade.Library.Google
{
    public class GoogleProviderHelper
    {
        public const string ProviderName = "Google";

        public const string GoogleClientId = "519381508568-vqa2fm3s18r49oj94s6g6gjmnbbcsb8n.apps.googleusercontent.com";
        public const string GoogleClientSecret = "91rCz39YBzAB5jmedbWEnHs5";

        //directory
        public const string GoogleClientId2 = "519381508568-p1khgbbnkc28ovkvnfph4sideq2nnfui.apps.googleusercontent.com";
        public const string GoogleClientSecret2 = "91rCz39YBzAB5jmedbWEnHs5";

        //ClientId = "519381508568-vqa2fm3s18r49oj94s6g6gjmnbbcsb8n.apps.googleusercontent.com",
        //ClientSecret = "91rCz39YBzAB5jmedbWEnHs5"

        // TODO: review the scopes needed - in particular for the drive API
        public static readonly string[] ClassroomIntegrationScopes = new[] {
                "https://www.googleapis.com/auth/drive",
                "https://www.googleapis.com/auth/classroom.coursework.students.readonly",
                "https://www.googleapis.com/auth/classroom.coursework.me.readonly",
                "https://www.googleapis.com/auth/classroom.course-work.readonly",
                "https://www.googleapis.com/auth/classroom.coursework.students",
                "https://www.googleapis.com/auth/classroom.coursework.me",
                "https://www.googleapis.com/auth/classroom.courses",
                "https://www.googleapis.com/auth/classroom.rosters.readonly",
                "https://www.googleapis.com/auth/classroom.rosters",
"https://www.googleapis.com/auth/classroom.rosters.readonly",
"https://www.googleapis.com/auth/classroom.profile.emails",
"https://www.googleapis.com/auth/classroom.profile.photos",
                //DirectoryService.Scope.AdminDirectoryUser,
                //DirectoryService.Scope.AdminDirectoryOrgunit,
                //DirectoryService.Scope.AdminDirectoryOrgunitReadonly,
                //DirectoryService.Scope.AdminDirectoryUserReadonly,
                "https://www.googleapis.com/auth/user.organization.read",
                "https://www.googleapis.com/auth/userinfo.profile",
                "https://www.googleapis.com/auth/cloud-platform",
"https://www.googleapis.com/auth/cloud-platform.read-only",
"https://www.googleapis.com/auth/cloudplatformorganizations",
"https://www.googleapis.com/auth/cloudplatformorganizations.readonly"
            };
        //
        public const string UserIdFieldName = ProviderName + "_userid";
        public const string AccessTokenFieldName = ProviderName + "_access_token";
        public const string TokenIssuedFieldName = ProviderName + "_issued_utc";
        public const string TokenExpiresAtFieldName = ProviderName + "_expires_at";
        public const string RefreshTokenFieldName = ProviderName + "_refresh_token";
        public const string EmailAddressFieldName = ProviderName + "Email";
        public const string NameFieldName = ProviderName + "Name";

        public static UserCredential CreateUserCredential(ClaimsPrincipal principal)
        {
            var initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = GoogleClientId,
                    ClientSecret = GoogleClientSecret,
                },
                Scopes = ClassroomIntegrationScopes
            };

            ClaimsIdentity identity = principal.Identities.Where(i => i.AuthenticationType == ProviderName).Single();

            var flow = new GoogleAuthorizationCodeFlow(initializer);
            var userId = identity.FindFirst(UserIdFieldName).Value;

            DateTime issuedAt = DateTime.Parse(identity.FindFirst(TokenIssuedFieldName).Value);
            DateTime expiresAt = DateTime.Parse(identity.FindFirst(TokenExpiresAtFieldName).Value);
            long seconds = (long)(expiresAt - issuedAt).TotalSeconds;
            Claim refreshTokenClaim = identity.FindFirst(RefreshTokenFieldName);
            var token = new TokenResponse()
            {
                AccessToken = identity.FindFirst(AccessTokenFieldName).Value,
                RefreshToken = refreshTokenClaim?.Value,
                IssuedUtc = issuedAt,
                ExpiresInSeconds = seconds,
            };
            var d = JsonConvert.SerializeObject(token);
            return new UserCredential(flow, userId, token);
        }

    }
}
