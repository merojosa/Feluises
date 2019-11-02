using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FeLuisesScrumDEV.Models;

namespace FeLuisesScrumDEV.Controllers
{

    public class ProjectsController : Controller
    {
        private FeLuisesEntities db = new FeLuisesEntities();

        // GET: Projects
        public ActionResult Index()
        {
            if (Convert.ToInt32(Session["userRole"]) == 0) //si es jefe
            {
                var projectChief = db.Project.Include(p => p.Client);
            }
            else if (Convert.ToInt32(Session["userRole"]) == 1 || Convert.ToInt32(Session["userRole"]) == 2) // si es dev o leader
            {
                //var project = db.Project.Include(p => p.Client).Where(db.WorksIn.Any(w => w.idEmployeeFKPK == p.));
            }
            
            
            return View(project.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Project.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.idClientFK = new SelectList(db.Client, "idClientPK", "clientName");
            var EmployeesController = new EmployeesController();
            var availableEmployes = EmployeesController.AvailableEmployees();
            ViewBag.idEmployeePK = new SelectList(availableEmployes, "idEmployeePK", "employeeName");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProjectPK,projectName,objective,estimatedCost,realCost,startingDate,finishingDate,budget,estimatedDuration,idClientFK")] Project project, [Bind(Include = "idEmployeePK")] WorksIn employee)
        {
            var EmployeesController = new EmployeesController();
            var availableEmployes = EmployeesController.AvailableEmployees();
            if ( db.Project.Any(x => x.projectName == project.projectName))
            {
                ModelState.AddModelError("projectName", "Ya existe un proyecto registrado con ese nombre");
                ViewBag.idClientFK = new SelectList(db.Client, "idClientPK", "clientName");
                ViewBag.idEmployeePK = new SelectList(availableEmployes, "idEmployeePK", "employeeName");
                return View(project);
            }
            if ( ModelState.IsValid )
            {
                db.Project.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idClientFK = new SelectList(db.Client, "idClientPK", "clientName", project.idClientFK);
            ViewBag.idEmployeePK = new SelectList(availableEmployes, "idEmployeePK", "employeeName", employee);
            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Project.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.idClientFK = new SelectList(db.Client, "idClientPK", "clientName", project.idClientFK);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProjectPK,projectName,objective,estimatedCost,realCost,startingDate,finishingDate,budget,estimatedDuration,idClientFK")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idClientFK = new SelectList(db.Client, "idClientPK", "clientName", project.idClientFK);
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Project.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Project.Find(id);
            db.Project.Remove(project);
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