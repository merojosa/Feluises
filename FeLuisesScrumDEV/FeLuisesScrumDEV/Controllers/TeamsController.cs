using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FeLuisesScrumDEV.Controllers
{
    public class TeamsController : Controller
    {
        // GET: Teams
        public ActionResult Index()
        {
            return View();
        }

        // GET: Teams/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Teams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Teams/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Teams/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Teams/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Teams/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
