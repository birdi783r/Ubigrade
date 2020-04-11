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
    public class NotenController : Controller
    {
        // GET: Noten
        public async Task<ActionResult> Index()
        {
            try
            {
                var data = await NotenProcessor.LoadNotenAsync("");

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
                return View(ViewListeNoten);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: Noten/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Noten/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Noten/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NotenModel neuenote)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NotenProcessor.CreateNote(neuenote.NId, neuenote.Bezeichnung, neuenote.Mindestanforderung);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: Noten/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                NotenDLModel resultnote = NotenProcessor.GetByIdNote(id);

                NotenModel note =
                    new NotenModel
                    {
                        NId = resultnote.NId,
                        Bezeichnung = resultnote.Bezeichnung,
                        Mindestanforderung = resultnote.Mindestanforderung
                    };

                return View(note);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST: Noten/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, NotenDLModel changednote)
        {
            try
            {
                NotenProcessor.SaveNote(id, changednote);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: Noten/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                NotenDLModel resultnote = NotenProcessor.GetByIdNote(id);

                NotenModel note =
                    new NotenModel
                    {
                        NId = resultnote.NId,
                        Bezeichnung = resultnote.Bezeichnung,
                        Mindestanforderung = resultnote.Mindestanforderung
                    };

                return View(note);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST: Noten/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Post(int id)
        {
            try
            {
                NotenProcessor.DeleteNote(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}