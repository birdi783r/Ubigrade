﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ubigrade.Application.Models;
using Ubigrade.Library.Models;
using Ubigrade.Library.Processors;

namespace Ubigrade.Application.Controllers.API_Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotenController : ControllerBase
    {
        public string conString = ConfigurationManager.ConnectionStrings["UbiServer"].ConnectionString;
        // GET: api/Noten
        [HttpGet]
        [Route("list")]
        public async Task<List<NotenModel>> GetGrades()
        {
            var x = conString;
            try
            {
                var data = await NotenProcessor.LoadNotenAsync(conString);

                List<NotenModel> ViewListeNoten = new List<NotenModel>();

                foreach (var item in data)
                {
                    ViewListeNoten.Add(
                        new NotenModel
                        {
                            NId = item.NId,
                            Bezeichnung = item.Bezeichnung,
                            Mindestanforderung = item.Mindestanforderung
                        });
                }
                return ViewListeNoten;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: api/Noten/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Noten
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Noten/5
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

//// GET: api/Noten/5
//[HttpGet("{id}"/*, Name = "Get"*/)]
//public NotenModel GetNote(int id)
//{
//    try
//    {
//        NotenDLModel resultnote = NotenProcessor.GetByIdNote(id);

//        NotenModel note =
//            new NotenModel
//            {
//                NId = resultnote.NId,
//                Bezeichnung = resultnote.Bezeichnung,
//                Mindestanforderung = resultnote.Mindestanforderung
//            };

//        return note;
//    }
//    catch (Exception ex)
//    {
//        throw new Exception(ex.Message);
//    }
//}
//
//// POST: api/Noten
//[HttpPost]
//public void PostNote(NotenModel note)
//{
//    try
//    {
//        NotenProcessor.CreateNote(
//            note.NId,
//            note.Bezeichnung,
//            note.Mindestanforderung
//            );
//    }
//    catch (Exception ex)
//    {
//        throw new Exception(ex.Message);
//    }
//}
//
//// PUT: api/Noten/5
//[HttpPut("{id}")]
//public void PutNote(int id, NotenDLModel note)
//{
//    try
//    {
//        NotenProcessor.SaveNote(id, note);
//    }
//    catch (Exception ex)
//    {
//        throw new Exception(ex.Message);
//    }
//}

//// DELETE: api/Noten/5
//[HttpDelete("{id}")]
//public void DeleteNote(int id)
//{
//    try
//    {
//        NotenProcessor.DeleteNote(id);
//    }
//    catch (Exception ex)
//    {
//        throw new Exception(ex.Message);
//    }
//}