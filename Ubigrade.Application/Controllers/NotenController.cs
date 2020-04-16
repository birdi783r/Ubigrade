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

namespace Ubigrade.Application.Controllers
{
    public class NotenController : Controller
    {
        private readonly IConfiguration _conf;
        private readonly string ConnectionString;
        public NotenController(IConfiguration configuration)
        {
            _conf = configuration;
            ConnectionString = _conf.GetConnectionString("Ubigrade2");
        }
        // GET: Noten
        public async Task<ActionResult> Index()
        {
            try
            {
                var data = await NotenProcessor.LoadNotenAsync("");
                List<NotenModel> ViewListeNoten = new List<NotenModel>();
                foreach (var item in data)
                {
                    ViewListeNoten.Add
                        (new NotenModel{
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
        public async Task<IActionResult> Create(NotenModel neuenote)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await NotenProcessor.CreateNoteAsync(neuenote.NId, neuenote.Bezeichnung, neuenote.Mindestanforderung, ConnectionString);
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
        public async Task<IActionResult> Edit(int id)
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
        public async Task<IActionResult> Edit(int id, NotenDLModel changednote)
        {
            try
            {
                await NotenProcessor.SaveNoteAsync(id, changednote, ConnectionString);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: Noten/Delete/5
        public async Task<IActionResult> Delete(int id)
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
        public async Task<IActionResult> Delete_Post(int id)
        {
            try
            {
                await NotenProcessor.DeleteNoteAsync(id,ConnectionString);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}