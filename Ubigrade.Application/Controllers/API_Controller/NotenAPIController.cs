using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ubigrade.Application.Models;
using Ubigrade.Library.Models;
using Ubigrade.Library.Processors;

namespace Ubigrade.Application.Controllers.API_Controller
{
    [Route("api/noten")]
    [ApiController]
    public class NotenAPIController : ControllerBase
    {
        // GET: api/NotenAPI    
        private readonly IConfiguration _config;
        private readonly string ConnectionString;
        public NotenAPIController(IConfiguration configuration)
        {
            _config = configuration;
            ConnectionString = _config.GetConnectionString("UbiServer");
        }
        [HttpGet]
        public async Task<List<NotenModel>> GetNoten()
        {
            try
            {
                var data = await NotenProcessor.LoadNotenAsync(ConnectionString);

                List<NotenModel> ViewListeNoten = new List<NotenModel>();

                foreach (var item in data)
                {
                    ViewListeNoten.Add(
                        new NotenModel
                        {
                            NId = item.NId,
                            Bezeichnung = item.Bezeichnung,
                            Mindestanforderung = item.Mindestanforderung
                        }
                        );
                }
                return ViewListeNoten;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: api/Noten/5
        [HttpGet("{id}"/*, Name = "Get"*/)]
        public async Task<NotenModel> GetNoteAsync(int id)
        {
            try
            {
                NotenDLModel resultnote = await NotenProcessor.GetByIdNoteAsync(id, ConnectionString);

                NotenModel note =
                    new NotenModel
                    {
                        NId = resultnote.NId,
                        Bezeichnung = resultnote.Bezeichnung,
                        Mindestanforderung = resultnote.Mindestanforderung
                    };

                return note;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST: api/Noten
        [HttpPost]
        public async Task<bool> PostNote(NotenModel note)
        {
            try
            {
                var b = await NotenProcessor.CreateNoteAsync(
                    note.NId,
                    note.Bezeichnung,
                    note.Mindestanforderung,
                    ConnectionString);
                return b;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // PUT: api/Noten/5
        [HttpPut("{id}")]
        public async Task<bool> PutNoteAsync(int id, NotenDLModel note)
        {
            try
            {
                var d = await NotenProcessor.SaveNoteAsync(id, note,ConnectionString);
                return d;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // DELETE: api/Noten/5
        [HttpDelete("{id}")]
        public async Task DeleteNote(int id)
        {
            try
            {
                NotenProcessor.DeleteNoteAsync(id, ConnectionString);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
