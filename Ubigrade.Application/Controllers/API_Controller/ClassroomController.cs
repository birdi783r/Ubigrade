﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Classroom.v1.Data;
using Google.Apis.People.v1.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ubigrade.Library.Google;
using static Ubigrade.Library.Google.GetGoogleData;

namespace Ubigrade.Application.Controllers.API_Controller
{
    [Route("api/classroomcourse")]
    [ApiController]
    public class ClassroomController : ControllerBase
    {
        // GET: api/Classroom
        //public readonly static string GoogleClientId = "519381508568-vqa2fm3s18r49oj94s6g6gjmnbbcsb8n.apps.googleusercontent.com";
        //public readonly static string GoogleClientSecret = "91rCz39YBzAB5jmedbWEnHs5";
        //private const string Token = "{\"access_token\":\"ya29.Il_BB84Mlh0G8D5fSZyI3si_TuF1U2OPSomjfncwllzdLdyo-83EJBBCtNSvCIu6UVvEGX6_6AxC-fIlMzyCXIlr4r86ynHbWb1_-bbsScKllfrXrMdoTuZ34kUFmRaoWA\",\"token_type\":null,\"expires_in\":3599,\"refresh_token\":\"1//03JQhCdANdESsCgYIARAAGAMSNwF-L9Ir7CKVFLvVMlzXMdmeETAyIObEux8PDdjSZb6oKQNEFC_KEgQ4Cgbj51FTH_2oqNcC_Wg\",\"scope\":null,\"id_token\":null,\"Issued\":\"2020-03-06T21:51:01+01:00\",\"IssuedUtc\":\"2020-03-06T21:51:01+01:00\"}";

        public class Data
        {
            public TokenResponse Token { get; set; }
            public string UserID { get; set; }
        }

        //ubigrade19-20 accesstoken ya29.a0Adw1xeW9JU5qn-Qe27xun1ejJ6ebLluqoFBMCAfHhaASLRT3S2GdqAEg_kAhQ9bSp3rwvbn4a4eEHCQT4L0giqFoRSftfQWcsPSdD7CnxQsDulM3YasZRzxsBr7LkvdmFOaGuPj6xkiENhnCVBXbq-mjst4e3KSwKH8
        //GET: api/Course
        [Route("courses")]
        [HttpGet]
        //public async Task<List<Course>> Get(string refreshtoken)
        public async Task<Person> Get(string refreshtoken)
        {
            //var credential = GoogleFlowCreator.CreateCredential(refreshtoken);
            //var x = await GetAllStudentActiveCoursesByAccesstoken(credential);
            //return x;

            var credential = GoogleFlowCreator.CreateCredential(refreshtoken);
            var x = await GoogleFlowCreator.GetUserId(credential);
            return x;
        }

        ////GET: api/Classroom/5
        //[Route("userid")]
        //[HttpGet]
        //public string Get(TokenResponse token)
        //{
        //    var credential = GoogleFlowCreator.CreateCredential(token);
        //    return credential.UserId;
        //}

        // POST: api/Classroom

        [HttpPost]
        [Route("user")]
        public string Post(string refreshtoken)
        {
            var credential = GoogleFlowCreator.CreateCredential(refreshtoken);
            var u = credential.UserId;
            if (String.IsNullOrWhiteSpace(u))
                return "failed";
            return "succes \n" + "u: " + u;
        }


        // PUT: api/Classroom/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
