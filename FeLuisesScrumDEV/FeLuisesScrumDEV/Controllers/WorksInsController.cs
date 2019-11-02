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
        /*
         * Efecto: Genera la vista del Create de equipos.
         * Requiere: NA
         * Modifica: La vista del Create de equipos 
         */
        public ActionResult Create()
        {
            ViewBag.auxLastName = new SelectList(db.Employee.Where(e => (e.availability == 0 && e.developerFlag == 1 && e.idEmployeePK != "000000000")), "idEmployeePK", "employeeLastName");
            ViewBag.idEmployeeFKPK = new SelectList(db.Employee.Where(e => (e.availability == 0 && e.developerFlag == 1 && e.idEmployeePK != "000000000")), "idEmployeePK", "employeeName");
            ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName");
            ViewBag.knowledges = new SelectList(db.DeveloperKnowledge, "idEmployeeFKPK", "devKnowledgePK");
            return View();
        }


        // POST: WorksIns/Create
        /*
         * Efecto: Procesa la informacion recibida por la vista mediante post
         * Requiere: Los miembros del equipo y el id del proyecto
         * Modifica: La tabla WorksIn, similar a equipos
         */
        [HttpPost]
        public ActionResult Create(string[] teamMembers, string currentProject)
        {
            
            
            if(currentProject != null)
            {
                //Recibimos el id del proyecto como un string desde la vista, hay que pasarlo a int
                int idProject = Int32.Parse(currentProject);
                if (teamMembers != null)
                {
                    foreach (var developer in teamMembers)
                    {
                        db.WorksIn.Add(new WorksIn
                        {
                            idEmployeeFKPK = developer,
                            idProjectFKPK = idProject,
                            role = 0 //Se especifica el rol del empleado
                        });
                        db.Employee.Find(developer).availability = 1;

                    }
                    //Permite encontrar errores de validacion en el modelo antes de guardar los cambios en la bd.
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    };

                    return Json(new
                    {
                        redirectUrl = Url.Action("Index", "WorksIns"),
                        isRedirect = true
                    });

                }
                else{
                    return RedirectToAction("Index", "WorksIns");
                }

            }else{
                return RedirectToAction("Index", "WorksIns");
            }
        }

        public ActionResult Edit()
        {
            ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName");
			
            //Estos son los disponibles
            ViewBag.auxLastName = new SelectList(db.Employee.Where(e => (e.availability == 0 && e.developerFlag == 1 && e.idEmployeePK != "000000000")), "idEmployeePK", "employeeLastName");
            ViewBag.idEmployeeFKPK = new SelectList(db.Employee.Where(e => (e.availability == 0 && e.developerFlag == 1 && e.idEmployeePK != "000000000")), "idEmployeePK", "employeeName");
            ViewBag.knowledges = new SelectList(db.DeveloperKnowledge, "idEmployeeFKPK", "devKnowledgePK");
            return View();
        }


        // POST: WorksIns/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string[] teamMembers, string currentProject)
        {
            if (currentProject != null)
            {
                //Recibimos el id del proyecto como un string desde la vista, hay que pasarlo a int
                int idProject = Int32.Parse(currentProject);
                int auxOldProject;

                //Acá borramos los empleados anteriores para evitar los conflictos
                WorksIn worksInEntity;
                ViewBag.oldEmployees = new SelectList(db.WorksIn.Where(t => t.idProjectFKPK == idProject), "idEmployeeFKPK", "idProjectFKPK");
                foreach(var oldEmployee in ViewBag.oldEmployees)
                {
                    auxOldProject = Int32.Parse(oldEmployee.Text);
                    worksInEntity = db.WorksIn.Find(oldEmployee.Value, auxOldProject);
                    db.WorksIn.Remove(worksInEntity);
                    var auxEmployee = db.Employee.Find(oldEmployee.Value);
                    auxEmployee.availability = 0;
                }
                

                if (teamMembers != null)
                {
                    foreach (var developer in teamMembers)
                    {
                        db.WorksIn.Add(new WorksIn
                        {
                            idEmployeeFKPK = developer,
                            idProjectFKPK = idProject,
                            role = 0 //Se especifica el rol del empleado
                        });
                        db.Employee.Find(developer).availability = 1;

                    }
                    //Permite encontrar errores de validacion en el modelo antes de guardar los cambios en la bd.
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    };

                    return Json(new
                    {
                        redirectUrl = Url.Action("Index", "WorksIns"),
                        isRedirect = true
                    });

                }else{
                    return RedirectToAction("Index", "WorksIns");
                }

            }
            else{
                return RedirectToAction("Index", "WorksIns");
            }
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

		public ActionResult bringTeam(string currentProject)
		{
            List<string> idMembers = new List<string>();
            List<string> nameMembers = new List<string>();
            List<string[]> skillsMembers = new List<string[]>();
            string auxName;
            List<string> auxSkill = new List<string>();
            int thisProject = Int32.Parse(currentProject);
            ViewBag.currentTeam = new SelectList(db.WorksIn.Where(e => (e.idProjectFKPK == thisProject)), "idEmployeeFKPK", "idProjectFKPK");
            ViewBag.skillsTeam = new SelectList(db.DeveloperKnowledge, "idEmployeeFKPK", "devKnowledgePK");
            foreach (var member in ViewBag.currentTeam)
            {
                var Employee = db.Employee.Find(member.Value);
                idMembers.Add(member.Value);
                auxName = Employee.employeeName + " " + Employee.employeeLastName;
                nameMembers.Add(auxName);
                foreach(var skill in ViewBag.skillsTeam)
                {
                    if(member.Value == skill.Value && skill.Text != null)
                    {
                        auxSkill.Add(skill.Text);
                    }
                }
                skillsMembers.Add(auxSkill.ToArray());
                auxSkill.Clear();
            }
			return Json(new
			{
				ids = idMembers.ToArray(),
                names = nameMembers.ToArray(),
                knowledges = skillsMembers.ToArray()
            });
		}
	}
}
		 
