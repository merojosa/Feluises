using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FeLuisesScrumDEV.Models;
using System.ComponentModel.DataAnnotations;

namespace FeLuisesScrumDEV.Controllers
{
    public class RequerimentsController : Controller
    {
        private FeLuisesEntities db = new FeLuisesEntities();

        // GET: Requeriments
        public ActionResult Index()
        {
            var requeriment = db.Requeriment.Include(r => r.Employee).Include(r => r.Module);
            return View(requeriment.ToList());
        }

        // GET: Requeriments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requeriment requeriment = db.Requeriment.Find(id);
            if (requeriment == null)
            {
                return HttpNotFound();
            }
            return View(requeriment);
        }

        // GET: Requeriments/Create
        public ActionResult Create()
        {
            ViewBag.idEmployeeFK = new SelectList(db.Employee, "idEmployeePK", "employeeName");
            ViewBag.idProjectFKPK = new SelectList(db.Module, "idProjectFKPK", "name");
            return View();
        }

        // POST: Requeriments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProjectFKPK,idModuleFKPK,idRequerimentPK,idEmployeeFK,estimatedDuration,realDuration,status,startingDate,endDate,complexity")] Requeriment requeriment)
        {
            if (ModelState.IsValid)
            {
                db.Requeriment.Add(requeriment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idEmployeeFK = new SelectList(db.Employee, "idEmployeePK", "employeeName", requeriment.idEmployeeFK);
            ViewBag.idProjectFKPK = new SelectList(db.Module, "idProjectFKPK", "name", requeriment.idProjectFKPK);
            return View(requeriment);
        }


        // GET: Requeriments/Edit/5
        public ActionResult Edit(int? ProjectId, int? ModuleId, int? RequerimentId)
    {
        if (ProjectId == null || ModuleId == null || RequerimentId == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Requeriment requeriment = db.Requeriment.Find(ProjectId, ModuleId, RequerimentId);
        if (requeriment == null)
        {
            return HttpNotFound();
        }
        ViewBag.idEmployeeFK = new SelectList(db.Employee, "idEmployeePK", "employeeName", requeriment.idEmployeeFK);
        ViewBag.idProjectFKPK = new SelectList(db.Module, "idProjectFKPK", "name", requeriment.idProjectFKPK);
        return View(requeriment);
    }

    // POST: Requeriments/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProjectFKPK,idModuleFKPK,idRequerimentPK,idEmployeeFK,estimatedDuration,realDuration,status,startingDate,endDate,complexity")] Requeriment requeriment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requeriment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idEmployeeFK = new SelectList(db.Employee, "idEmployeePK", "employeeName", requeriment.idEmployeeFK);
            ViewBag.idProjectFKPK = new SelectList(db.Module, "idProjectFKPK", "name", requeriment.idProjectFKPK);
            return View(requeriment);
        }

        // GET: Requeriments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requeriment requeriment = db.Requeriment.Find(id);
            if (requeriment == null)
            {
                return HttpNotFound();
            }
            return View(requeriment);
        }

        // POST: Requeriments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Requeriment requeriment = db.Requeriment.Find(id);
            db.Requeriment.Remove(requeriment);
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

        public class RequerimentValidation
        {
            public static ValidationResult validateName(String id)
            {
                {
                    FeLuisesEntities db = new FeLuisesEntities();
                    if (db.Client.Any(x => x.idClientPK == id) || db.Employee.Any(x => x.idEmployeePK == id))
                        return new ValidationResult("A person id must be unique");
                    return ValidationResult.Success;
                }
            }
        }
    }
}
