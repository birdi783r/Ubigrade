using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ubigrade.Application.Models;
using Ubigrade.Library.Models;
using Ubigrade.Library.Processors;

namespace Ubigrade.Application.Controllers
{
    //[Authorize(Policy = "TeacherPolicy, StudentPolicy")]
    public class SchuelerController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string ConnectionString;
        public SchuelerController(IConfiguration configuration)
        {
            _config = configuration;
            ConnectionString = _config.GetConnectionString("Ubigrade2");
        }
        // GET: Schueler
        public async Task<IActionResult> Index()
        {
            var s = User;
            try
            {
                var data = await SchuelerProcessor.LoadSchuelerAsync(ConnectionString);

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

                return View(ViewListeSchueler);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        // GET: Schueler/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            try
            {

                SchuelerDLModel resultschueler = await SchuelerProcessor.GetByIdSchuelerAsync(ConnectionString, id);

                List<FaecherModel> ViewSchuelerFaecher = new List<FaecherModel>();
                foreach (var item in resultschueler.Faecher)
                {
                    ViewSchuelerFaecher.Add(
                        new FaecherModel() { SkennZahl = item.Skennzahl, Fachbezeichnung = item.Fachbezeichnung });
                }

                SchuelerModel schueler =
                    new SchuelerModel
                    {
                        Checkpersonnumber = resultschueler.Checkpersonnumber,
                        NName = resultschueler.NName,
                        VName = resultschueler.VName,
                        Geschlecht = resultschueler.Geschlecht,
                        EmailAdresse = resultschueler.EmailAdresse,
                        Schuljahr = resultschueler.Schuljahr,
                        Faecher = ViewSchuelerFaecher
                    };

                return View(schueler);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: Schueler/Create
        public ActionResult Create()
        {
            var model = new SchuelerModel();
            return View(model);
        }

        // POST: Schueler/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create(SchuelerModel neuerschueler)
        {
            try
            {
                if (ModelState.IsValid)
                    await SchuelerProcessor.CreateSchuelerAsync(neuerschueler.Checkpersonnumber.ToString(), neuerschueler.NName, neuerschueler.VName, neuerschueler.Geschlecht, neuerschueler.EmailAdresse, neuerschueler.Schuljahr, ConnectionString);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        // GET: Schueler/Edit/5
        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                SchuelerDLModel resultschueler = await SchuelerProcessor.GetByIdSchuelerAsync(ConnectionString, id);

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

                return View(schueler);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        // POST: Schueler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(string id, SchuelerDLModel changedschueler)
        {
            try
            {
                var f = await SchuelerProcessor.SaveSchuelerAsync(ConnectionString, id, changedschueler);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        // GET: Schueler/Delete/5
        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                SchuelerDLModel resultschueler = await SchuelerProcessor.GetByIdSchuelerAsync(ConnectionString, id);

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

                return View(schueler);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST: Schueler/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete_Post(string id)
        {
            try
            {
                await SchuelerProcessor.DeleteSchuelerAsync(ConnectionString, id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}