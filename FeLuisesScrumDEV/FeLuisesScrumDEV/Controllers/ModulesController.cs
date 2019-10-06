using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FeLuisesScrumDEV.Models;
using FeLuisesScrumDEV.viewModel;

namespace FeLuisesScrumDEV.Controllers
{
    public class ModulesController : Controller
    {
        private FeLuisesEntities db = new FeLuisesEntities();

        // GET: Modules
        public ActionResult Index()
        {
            var arrangedList = new List<IndexViewModel>();
            var module = db.Module.Include(m => m.Project);
            IndexViewModel dummy = new IndexViewModel();
            int lastPKchecked = -1;
            foreach (var item in module)
            {
                if (item.idProjectFKPK == lastPKchecked)
                {
                    dummy.AssociatedModules.Add(item);
                }
                else
                {
                    if (lastPKchecked != -1)
                        arrangedList.Add(dummy);
                    dummy = new IndexViewModel();
                    dummy.Project = item.Project;
                    dummy.AssociatedModules.Add(item);
                    lastPKchecked = item.idProjectFKPK;
                }
            }
            if (lastPKchecked != -1)
                arrangedList.Add(dummy); ;
            return View(arrangedList);
        }

        // GET: Modules/Details/5
        public ActionResult Details(int? idProjectFKPK, int? idModulePK)
        {
            if (idProjectFKPK == null || idModulePK == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Module.Find(idProjectFKPK, idModulePK);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // GET: Modules/Create
        public ActionResult Create()
        {
            ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName");
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProjectFKPK,idModulePK,name")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Module.Add(module);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName", module.idProjectFKPK);
            return View(module);
        }

        // GET: Modules/Edit/5
        public ActionResult Edit(int? idProjectFKPK, int? idModulePK)
        {
            if (idProjectFKPK == null || idModulePK == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (idModulePK == -1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            Module module = db.Module.Find(idProjectFKPK, idModulePK);
            if (module == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName", module.idProjectFKPK);
            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProjectFKPK,idModulePK,name")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName", module.idProjectFKPK);
            return View(module);
        }

        // GET: Modules/Delete/5
        public ActionResult Delete(int? idProjectFKPK, int? idModulePK)
        {
            if (idProjectFKPK == null || idModulePK == null || idModulePK == -1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Module.Find(idProjectFKPK, idModulePK);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? idProjectFKPK, int? idModulePK)
        {
            if (idModulePK == -1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            Module module = db.Module.Find(idProjectFKPK,idModulePK);
            db.Module.Remove(module);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
