using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ubigrade.Application.Controllers
{
    public class FaecherController : Controller
    {
        // GET: Faecher
        public ActionResult Index()
        {
            return View();
        }

        // GET: Faecher/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Faecher/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Faecher/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Faecher/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Faecher/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Faecher/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Faecher/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}