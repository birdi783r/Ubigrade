using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2.Web;
using Google.Apis.Requests;
using Google.Apis.People;
using Google.Apis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Google.Apis.Services;
using Google.Apis.People.v1;
using System.Threading.Tasks;
using Google.Apis.People.v1.Data;

namespace Ubigrade.Library.Google
{
    public class GoogleFlowCreator
    {
        private readonly static string GoogleClientId = "519381508568-vqa2fm3s18r49oj94s6g6gjmnbbcsb8n.apps.googleusercontent.com";
        private readonly static string GoogleClientSecret = "91rCz39YBzAB5jmedbWEnHs5";

        //public const string Token = "{\"access_token\":\"ya29.Il_BB84Mlh0G8D5fSZyI3si_TuF1U2OPSomjfncwllzdLdyo-83EJBBCtNSvCIu6UVvEGX6_6AxC-fIlMzyCXIlr4r86ynHbWb1_-bbsScKllfrXrMdoTuZ34kUFmRaoWA\",\"token_type\":null,\"expires_in\":3599,\"refresh_token\":\"1//03JQhCdANdESsCgYIARAAGAMSNwF-L9Ir7CKVFLvVMlzXMdmeETAyIObEux8PDdjSZb6oKQNEFC_KEgQ4Cgbj51FTH_2oqNcC_Wg\",\"scope\":null,\"id_token\":null,\"Issued\":\"2020-03-06T21:51:01+01:00\",\"IssuedUtc\":\"2020-03-06T21:51:01+01:00\"}";

        public static UserCredential CreateCredential(string refreshtoken)
        {
            var Token = new TokenResponse()
            {
                RefreshToken = refreshtoken,
                TokenType="Bearer",
                ExpiresInSeconds=3599,
                IdToken=null,
                Scope=""
            };
            var initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = GoogleClientId,
                    ClientSecret = GoogleClientSecret
                }
            };
            
            var flow = new GoogleAuthorizationCodeFlow(initializer);
            UserCredential credential =  new UserCredential(flow, "", Token);
        
            return credential;
        }

        public async static Task<Person> GetUserId(UserCredential credential)
        {
            //directory api key AIzaSyBq6xSct8OhL2-5zUWMH73wRIN6iFGOg2k
            //people und cloud resource manager api key AIzaSyBtsaZHK0PswRKIcErSX7eH7ojT_rHeKpE
         
            var b = await credential.RefreshTokenAsync(CancellationToken.None);
            var r = b;
            var service = new PeopleService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "UbiGrade"
            });

            PeopleResource.GetRequest request = service.People.Get("people/me");
            request.BearerToken = credential.Token.AccessToken;
            request.RequestMaskIncludeField = "people.names";

            // htl orgunit id = 1056718226129

            var x = await request.ExecuteAsync();
            return x;
        }
    }
}
