using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
        private string[] teamMembersResult;

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
            //var Empleados = db.Employee.Where(e => (e.availability == 0 && e.developerFlag == 1));
            //Empleados.
            ViewBag.idEmployeeFKPK = new SelectList(db.Employee.Where(e => (e.availability == 0 && e.developerFlag == 1)), "idEmployeePK", "employeeName");
            ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName");
            return View();
        }

        // POST: WorksIns/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "idEmployeeFKPK,idProjectFKPK,role")] WorksIn worksIn)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.WorksIn.Add(worksIn);
        //        db.SaveChanges();
        //       
        //    }

        //    ViewBag.idEmployeeFKPK = new SelectList(db.Employee, "idEmployeePK", "employeeName", worksIn.idEmployeeFKPK);
        //    ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName", worksIn.idProjectFKPK);
        //    return View(worksIn);
        //}

        [HttpPost]
        public ActionResult Create(string[] teamMembers, string currentProject)
        {

            int idProject = Int32.Parse(currentProject);

            foreach (var developer in teamMembers)
            {
                try
                {
                    db.WorksIn.Add(new WorksIn {
                        idEmployeeFKPK = developer,
                        idProjectFKPK = idProject
                    });
                    db.Employee.Find(developer).availability = 1;
                    db.SaveChanges();
                }catch(Exception)
                {
                    continue;
                }
                //catch (DbEntityValidationException e)
                //{
                //    foreach (var eve in e.EntityValidationErrors)
                //    {
                //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //        foreach (var ve in eve.ValidationErrors)
                //        {
                //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //                ve.PropertyName, ve.ErrorMessage);
                //        }
                //    }
                //    throw;
                //}
            }

            return View("Index");
            //var worksIn = db.WorksIn.Include(w => w.Employee).Include(w => w.Project);
            //return View(worksIn.ToList());
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
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
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

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
