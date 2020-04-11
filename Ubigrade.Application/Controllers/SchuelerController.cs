using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ubigrade.Application.Models;
using Ubigrade.Library.Models;
using Ubigrade.Library.Processors;

namespace Ubigrade.Application.Controllers
{
    public class SchuelerController : Controller
    {
        // GET: Schueler
        public async Task<ActionResult> Index()
        {
            try
            {
                var data = await SchuelerProcessor.LoadSchuelerAsync("");

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
        public ActionResult Details(int id)
        {
            return View();
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
        public ActionResult Create(SchuelerModel neuerschueler)
        {
            try
            {
                if (ModelState.IsValid)
                    SchuelerProcessor.CreateSchueler(neuerschueler.Checkpersonnumber, neuerschueler.NName, neuerschueler.VName, neuerschueler.Geschlecht, neuerschueler.EmailAdresse, neuerschueler.Schuljahr);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        // GET: Schueler/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
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
        public ActionResult Edit(int id, SchuelerDLModel changedschueler)
        {
            try
            {
                SchuelerProcessor.SaveSchueler(id, changedschueler);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        // GET: Schueler/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
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

                return View(schueler);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST: Schueler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Post(int id)
        {
            try
            {
                SchuelerProcessor.DeleteSchueler(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}