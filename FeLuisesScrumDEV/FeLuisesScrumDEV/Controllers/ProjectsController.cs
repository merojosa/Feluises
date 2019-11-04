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
                return View(projectChief.ToList());
            }
            else if (Convert.ToInt32(Session["userRole"]) == 1 || Convert.ToInt32(Session["userRole"]) == 2) // si es dev o leader
            {

                var user = Session["userID"].ToString();

                var participacion = db.WorksIn.Where(w => w.idEmployeeFKPK == user);
                var proyectos = db.Project.Where(p => participacion.Any(w => w.idProjectFKPK == p.idProjectPK));
                return View(proyectos.ToList());
            }
            else if (Convert.ToInt32(Session["userRole"]) == 3) // si es cliente
            {
                var user = Session["userID"].ToString();
                var proyectos = db.Project.Where(p => p.idClientFK == user);
                return View(proyectos.ToList());
            }
            return null;
            
           
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            var WorksInController = new WorksInsController();
            var lider = WorksInController.GetLiderName(id);
            ViewBag.idLiderFK = lider;
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
            var availableEmployees = EmployeesController.AvailableEmployees();
            ViewBag.idEmployeeFKPK = new SelectList(availableEmployees, "idEmployeePK", "employeeName");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProjectPK,projectName,objective,estimatedCost,realCost,startingDate,finishingDate,budget,estimatedDuration,idClientFK")] Project project, [Bind(Include = "idEmployeeFKPK")] WorksIn employee)
        {
            var EmployeesController = new EmployeesController();
            var availableEmployees = EmployeesController.AvailableEmployees();
            if ( db.Project.Any(x => x.projectName == project.projectName))
            {
                ModelState.AddModelError("projectName", "Ya existe un proyecto registrado con ese nombre");
                ViewBag.idClientFK = new SelectList(db.Client, "idClientPK", "clientName");
                ViewBag.idEmployeeFKPK = new SelectList(availableEmployees, "idEmployeePK", "employeeName");
                return View(project);
            }
            if ( ModelState.IsValid )
            {
                db.Project.Add(project);
                employee.idProjectFKPK = project.idProjectPK;
                employee.role = 1;
                db.WorksIn.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idClientFK = new SelectList(db.Client, "idClientPK", "clientName", project.idClientFK);
            ViewBag.idEmployeeFKPK = new SelectList(availableEmployees, "idEmployeePK", "employeeName", employee);
            return View(project);
        }

        // GET: Projects/Edit/5
        // MOD: el proyecto con el id especificado.
        public ActionResult Edit(int? id)
        {
            var EmployeesController = new EmployeesController();
            var availableEmployees = EmployeesController.AvailableEmployeesAndLider(id);
            var WorksInController = new WorksInsController();
            var lider = WorksInController.GetLiderID(id);
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
            ViewBag.idEmployeeFKPK = new SelectList(availableEmployees, "idEmployeePK", "employeeName", lider);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProjectPK,projectName,objective,estimatedCost,realCost,startingDate,finishingDate,budget,estimatedDuration,idClientFK")] Project project, [Bind(Include = "idEmployeeFKPK")] WorksIn employee)
        {
            var EmployeesController = new EmployeesController();
            var availableEmployees = EmployeesController.AvailableEmployeesAndLider(project.idProjectPK);
            var WorksInController = new WorksInsController();
            var lider = WorksInController.GetLiderID(project.idProjectPK);
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                employee.idProjectFKPK = project.idProjectPK;
                employee.role = 1;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idClientFK = new SelectList(db.Client, "idClientPK", "clientName", project.idClientFK);
            ViewBag.idEmployeeFKPK = new SelectList(availableEmployees, "idEmployeePK", "employeeName", lider);
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