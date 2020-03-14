using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ubigrade.Library.Google
{
    public class GoogleFlowCreator
    {
        public const string GoogleClientId = "519381508568-vqa2fm3s18r49oj94s6g6gjmnbbcsb8n.apps.googleusercontent.com";
        public const string GoogleClientSecret = "91rCz39YBzAB5jmedbWEnHs5";

        //public const string Token = "{\"access_token\":\"ya29.Il_BB84Mlh0G8D5fSZyI3si_TuF1U2OPSomjfncwllzdLdyo-83EJBBCtNSvCIu6UVvEGX6_6AxC-fIlMzyCXIlr4r86ynHbWb1_-bbsScKllfrXrMdoTuZ34kUFmRaoWA\",\"token_type\":null,\"expires_in\":3599,\"refresh_token\":\"1//03JQhCdANdESsCgYIARAAGAMSNwF-L9Ir7CKVFLvVMlzXMdmeETAyIObEux8PDdjSZb6oKQNEFC_KEgQ4Cgbj51FTH_2oqNcC_Wg\",\"scope\":null,\"id_token\":null,\"Issued\":\"2020-03-06T21:51:01+01:00\",\"IssuedUtc\":\"2020-03-06T21:51:01+01:00\"}";

        public static UserCredential CreateCredential(TokenResponse token)
        {

            var initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = GoogleClientId,
                    ClientSecret = GoogleClientSecret
                }
            };
            var flow = new GoogleAuthorizationCodeFlow(initializer);
            //TokenResponse token = (TokenResponse)JsonConvert.DeserializeObject(Token);
            return new UserCredential(flow, "", token);
        }
    }
}
