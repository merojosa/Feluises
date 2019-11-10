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
        // EFE: Lista todos los conocimientos
        public ActionResult Index()
        {
            var developerKnowledge = db.DeveloperKnowledge.Include(d => d.Employee);
            return View(developerKnowledge.ToList());
        }

        // GET: DeveloperKnowledges/Details/5
        // no se usa
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
        //EFE: Crea un nuevo conocimiento
        public ActionResult Create()
        {
            ViewBag.idEmployeeFKPK = new SelectList(db.Employee, "idEmployeePK", "employeeName");
            return View();
        }

        // POST: DeveloperKnowledges/Create
        //EFE: Valida todos los campos
        //REQ: campos obligatorios
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idEmployeeFKPK,devKnowledgePK")] DeveloperKnowledge developerKnowledge)
        {
            if (db.DeveloperKnowledge.Any(x => x.devKnowledgePK == developerKnowledge.devKnowledgePK) && db.DeveloperKnowledge.Any(x => x.idEmployeeFKPK == developerKnowledge.idEmployeeFKPK))
            {
                ModelState.AddModelError("devKnowledgePK", "Este empleado ya posee este conocimiento");
                ViewBag.idEmployeeFKPK = new SelectList(db.Employee, "idEmployeePK", "employeeName", developerKnowledge.idEmployeeFKPK);
                return View(developerKnowledge);
            }
            if (ModelState.IsValid)
            {
                db.DeveloperKnowledge.Add(developerKnowledge);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
  
            return View(developerKnowledge);
        }

        // GET: DeveloperKnowledges/Edit/5
        //EFE: Edita el conocimiento requerido
        public ActionResult Edit(string id, string ability)
        {
            if (id == null || ability == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeveloperKnowledge developerKnowledge = db.DeveloperKnowledge.Find(id, ability);
            if (developerKnowledge == null)
            {
                return HttpNotFound();
            }
            ViewBag.idEmployeeFKPK = new SelectList(db.Employee, "idEmployeePK", "employeeName", developerKnowledge.idEmployeeFKPK);
            return View(developerKnowledge);
        }

        // POST: DeveloperKnowledges/Edit/5
        //EFE: VAlida los campos
        //REQ: Campos obligatorios
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEmployeeFKPK,devKnowledgePK")] DeveloperKnowledge developerKnowledge)
        {
            if ( !db.DeveloperKnowledge.Any(x => x.idEmployeeFKPK==developerKnowledge.idEmployeeFKPK && x.devKnowledgePK==developerKnowledge.devKnowledgePK) && ModelState.IsValid )
            {
                db.Entry(developerKnowledge).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.idEmployeeFKPK = new SelectList(db.Employee, "idEmployeePK", "employeeName", developerKnowledge.idEmployeeFKPK);
                ModelState.AddModelError("devKnowledgePK", "Este empleado ya posee este conocimiento");
                return View(developerKnowledge);
            }
        }

        // GET: DeveloperKnowledges/Delete/5
        //EFE: Elimina el conocimiento seleccionado.
        public ActionResult Delete(string id, string ability)
        {
            if (id == null || ability == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeveloperKnowledge developerKnowledge = db.DeveloperKnowledge.Find(id, ability);
            if (developerKnowledge == null)
            {
                return HttpNotFound();
            }
            return View(developerKnowledge);
        }

        // POST: DeveloperKnowledges/Delete/5
        //EFE: Valida que se hayan guardado los campos
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string ability)
        {
            DeveloperKnowledge developerKnowledge = db.DeveloperKnowledge.Find(id, ability);
            db.DeveloperKnowledge.Remove(developerKnowledge);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = developerKnowledge.idEmployeeFKPK });
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
