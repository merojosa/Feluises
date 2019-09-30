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
    public class DeveloperKnowledgesController : Controller
    {
        private FeLuisesEntities db = new FeLuisesEntities();

        // GET: DeveloperKnowledges
        public ActionResult Index()
        {
            var developerKnowledge = db.DeveloperKnowledge.Include(d => d.Employee);
            return View(developerKnowledge.ToList());
        }

        // GET: DeveloperKnowledges/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeveloperKnowledge developerKnowledge = db.DeveloperKnowledge.Find(id);
            if (developerKnowledge == null)
            {
                return HttpNotFound();
            }
            return View(developerKnowledge);
        }

        // GET: DeveloperKnowledges/Create
        public ActionResult Create()
        {
            ViewBag.idEmployeeFKPK = new SelectList(db.Employee, "idEmployeePK", "employeeName");
            return View();
        }

        // POST: DeveloperKnowledges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idEmployeeFKPK,devKnowledgePK")] DeveloperKnowledge developerKnowledge)
        {
            if (ModelState.IsValid)
            {
                db.DeveloperKnowledge.Add(developerKnowledge);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idEmployeeFKPK = new SelectList(db.Employee, "idEmployeePK", "employeeName", developerKnowledge.idEmployeeFKPK);
            return View(developerKnowledge);
        }

        // GET: DeveloperKnowledges/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeveloperKnowledge developerKnowledge = db.DeveloperKnowledge.Find(id);
            if (developerKnowledge == null)
            {
                return HttpNotFound();
            }
            ViewBag.idEmployeeFKPK = new SelectList(db.Employee, "idEmployeePK", "employeeName", developerKnowledge.idEmployeeFKPK);
            return View(developerKnowledge);
        }

        // POST: DeveloperKnowledges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEmployeeFKPK,devKnowledgePK")] DeveloperKnowledge developerKnowledge)
        {
            if (ModelState.IsValid)
            {
                db.Entry(developerKnowledge).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idEmployeeFKPK = new SelectList(db.Employee, "idEmployeePK", "employeeName", developerKnowledge.idEmployeeFKPK);
            return View(developerKnowledge);
        }

        // GET: DeveloperKnowledges/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeveloperKnowledge developerKnowledge = db.DeveloperKnowledge.Find(id);
            if (developerKnowledge == null)
            {
                return HttpNotFound();
            }
            return View(developerKnowledge);
        }

        // POST: DeveloperKnowledges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DeveloperKnowledge developerKnowledge = db.DeveloperKnowledge.Find(id);
            db.DeveloperKnowledge.Remove(developerKnowledge);
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
