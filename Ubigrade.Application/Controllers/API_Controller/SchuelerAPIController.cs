using System;
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
    [Route("api/schueler")]
    [ApiController]
    public class SchuelersController : ControllerBase 
    {
        public string conString = ConfigurationManager.ConnectionStrings["UbiServer"].ConnectionString;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchuelerModel>>> GetSchuelers()
        {
            try
            {
                //var x = conString;
                var data = await SchuelerProcessor.LoadSchuelerAsync();
                List<SchuelerModel> ViewListeSchueler = new List<SchuelerModel>();

                foreach (var item in data)
                {
                    ViewListeSchueler.Add(
                        new SchuelerModel()
                        {
                            Checkpersonnumber = item.Checkpersonnumber,
                            NName = item.NName,
                            VName = item.VName,
                            EmailAdresse = item.EmailAdresse,
                            Geschlecht = item.Geschlecht,
                            Schuljahr = item.Schuljahr
                        });
                }

                return ViewListeSchueler;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: api/Schueler/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SchuelerModel>> GetSchueler(int id)
        {
            try
            {
                await Task.Delay(10);

                SchuelerDLModel resultschueler = SchuelerProcessor.GetByIdSchueler(id);

                SchuelerModel schueler =
                    new SchuelerModel
                    {
                        Checkpersonnumber = resultschueler.Checkpersonnumber,
                        NName = resultschueler.NName,
                        VName = resultschueler.VName,
                        Geschlecht = resultschueler.Geschlecht,
                        EmailAdresse = resultschueler.EmailAdresse,
                        Schuljahr = resultschueler.Schuljahr
                    };
                return schueler;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // PUT: api/Schueler/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<string> PutSchueler(int id, SchuelerDLModel schueler)
        {
            try
            {
                await Task.Delay(10);

                SchuelerProcessor.SaveSchueler(id, schueler);

                return "schueler_updated";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST: api/Schueler
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<string>> PostSchueler(SchuelerModel schueler)
        {
            try
            {
                await Task.Delay(10);

                SchuelerProcessor.CreateSchueler(
                schueler.Checkpersonnumber,
                schueler.NName,
                schueler.VName,
                schueler.Geschlecht,
                schueler.EmailAdresse,
                schueler.Schuljahr);

                return "schueler_created";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // DELETE: api/Schueler/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteSchueler(int id)
        {
            try
            {
                await Task.Delay(10);

                SchuelerProcessor.DeleteSchueler(id);

                return "schueler_deleted";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
