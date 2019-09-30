using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FeLuisesScrumDEV.Models;

namespace FeLuisesScrumDEV.Controllers
{
    public class WorksInsController : Controller
    {
        private FeLuisesEntities db = new FeLuisesEntities();

        // GET: WorksIns
        public ActionResult Index()
        {
            var worksIn = db.WorksIn.Include(w => w.Employee).Include(w => w.Project);
            return View(worksIn.ToList());
        }

        // GET: WorksIns/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorksIn worksIn = db.WorksIn.Find(id);
            if (worksIn == null)
            {
                return HttpNotFound();
            }
            return View(worksIn);
        }

        // GET: WorksIns/Create
        public ActionResult Create()
        {
            ViewBag.idEmployeeFKPK = new SelectList(db.Employee, "idEmployeePK", "employeeName");
            ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName");
            return View();
        }

        // POST: WorksIns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idEmployeeFKPK,idProjectFKPK,role")] WorksIn worksIn)
        {
            if (ModelState.IsValid)
            {
                db.WorksIn.Add(worksIn);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idEmployeeFKPK = new SelectList(db.Employee, "idEmployeePK", "employeeName", worksIn.idEmployeeFKPK);
            ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName", worksIn.idProjectFKPK);
            return View(worksIn);
        }

        // GET: WorksIns/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorksIn worksIn = db.WorksIn.Find(id);
            if (worksIn == null)
            {
                return HttpNotFound();
            }
            ViewBag.idEmployeeFKPK = new SelectList(db.Employee, "idEmployeePK", "employeeName", worksIn.idEmployeeFKPK);
            ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName", worksIn.idProjectFKPK);
            return View(worksIn);
        }

        // POST: WorksIns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEmployeeFKPK,idProjectFKPK,role")] WorksIn worksIn)
        {
            if (ModelState.IsValid)
            {
                db.Entry(worksIn).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idEmployeeFKPK = new SelectList(db.Employee, "idEmployeePK", "employeeName", worksIn.idEmployeeFKPK);
            ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName", worksIn.idProjectFKPK);
            return View(worksIn);
        }

        // GET: WorksIns/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorksIn worksIn = db.WorksIn.Find(id);
            if (worksIn == null)
            {
                return HttpNotFound();
            }
            return View(worksIn);
        }

        // POST: WorksIns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            WorksIn worksIn = db.WorksIn.Find(id);
            db.WorksIn.Remove(worksIn);
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
