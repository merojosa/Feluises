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

        /*
         * Efecto: Carga el índice de Equipo.
         * Requiere: NA.
         * Modifica: NA.
         */
        public ActionResult Index()
        {
            var worksIn = db.WorksIn.Include(w => w.Employee).Include(w => w.Project);
            return View(worksIn.ToList());
        }

        /*
         * Efecto: Carga la vista de detalles de un equipo.
         * Requiere: El id de un equipo para mostrar la vista.
         * Modifica: NA.
         */
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

        /*
         * Efecto: Genera la vista del Create de equipos, en la cual se pueden asociar empleados a un proyecto.
         * Requiere: NA
         * Modifica: La vista del Create de equipos.
         */
        public ActionResult Create()
        {
            //Viebags para acceder a información dentro de la vista
            ViewBag.auxLastName = new SelectList(db.Employee.Where(e => (e.availability == 0 && e.developerFlag == 1 && e.idEmployeePK != "000000000")), "idEmployeePK", "employeeLastName");
            ViewBag.idEmployeeFKPK = new SelectList(db.Employee.Where(e => (e.availability == 0 && e.developerFlag == 1 && e.idEmployeePK != "000000000")), "idEmployeePK", "employeeName");
            ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName");
            ViewBag.knowledges = new SelectList(db.DeveloperKnowledge, "idEmployeeFKPK", "devKnowledgePK");
            return View();
        }

        /*
         * Efecto: Procesa la informacion recibida por la vista mediante post, agregando los empleados al proyecto
         * Requiere: Los miembros del equipo y el id del proyecto (Lo pasa la vista mediante AJAX)
         * Modifica: La tabla WorksIn, que es nuestra tabla de equipos
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
                            role = 0 //Se especifica el rol del empleado, en este caso desarrollador
                        });
                        db.Employee.Find(developer).availability = 1;
                    }
                    
                    try
                    {
                        db.SaveChanges();
                    }//Permite atrapar errores de validacion en el modelo antes de guardar los cambios en la bd.
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
                    //Retorna un JSON que es recibido por el success de la vista
                    return Json(new
                    {
                        redirectUrl = Url.Action("Index", "WorksIns"),
                        isRedirect = true
                    });
                }
                else{
                    return RedirectToAction("Index", "WorksIns"); //En caso de nulos redirecciona al indice
                }

            }else{
                return RedirectToAction("Index", "WorksIns"); //En caso de nulos redirecciona al indice
            }
        }

        /*
         * Efecto: Genera la vista del Edit de equipos, en la cual se pueden asociar y desasociar empleados de un proyecto.
         * Requiere: NA
         * Modifica: La vista del Create de equipos.
         */
        public ActionResult Edit()
        {
            ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName");
			
            //Estos son los disponibles
            ViewBag.auxLastName = new SelectList(db.Employee.Where(e => (e.availability == 0 && e.developerFlag == 1 && e.idEmployeePK != "000000000")), "idEmployeePK", "employeeLastName");
            ViewBag.idEmployeeFKPK = new SelectList(db.Employee.Where(e => (e.availability == 0 && e.developerFlag == 1 && e.idEmployeePK != "000000000")), "idEmployeePK", "employeeName");
            ViewBag.knowledges = new SelectList(db.DeveloperKnowledge, "idEmployeeFKPK", "devKnowledgePK");
            return View();
        }


        /*
         * Efecto: Procesa la informacion recibida por la vista mediante post, quitando los empleados anteriores 
         *     y agregando los nuevos al equipo.
         * Requiere: Los nuevos miembros del equipo y el id del proyecto(Lo pasa la vista mediante AJAX)
         * Modifica: La tabla WorksIn, que es nuestra tabla de equipos
         */
        [HttpPost]
        public ActionResult Edit(string[] teamMembers, string currentProject)
        {
            if (currentProject != null)
            {
                //Recibimos el id del proyecto como un string desde la vista, hay que pasarlo a int
                int idProject = Int32.Parse(currentProject);
                int auxOldProject;

                //Acá borramos los empleados anteriores para evitar los conflictos que puedan generar con los que vienen de la vista.
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
                            role = 0 //Se especifica el rol del empleado, desarrollador para este caso.
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
                    //Retorna un JSON que es recibido por el success de la vista
                    return Json(new
                    {
                        redirectUrl = Url.Action("Index", "WorksIns"),
                        isRedirect = true
                    });

                }else{
                    return RedirectToAction("Index", "WorksIns");//En caso de nulos redirecciona al indice
                }

            }
            else{
                return RedirectToAction("Index", "WorksIns");//En caso de nulos redirecciona al indice
            }
        }

        /*
         * Efecto: Retorna la vista para eliminar un empleado dentro de un equipo
         * Requiere: El id del empleado
         * Modifica: NA
         */
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

        /*
        * Efecto: Elimina un empleado dentro de un equipo
        * Requiere: El id del empleado
        * Modifica: La tabla WorksIn, que es nuestra tabla de equipos
        */
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            WorksIn worksIn = db.WorksIn.Find(id);
            db.WorksIn.Remove(worksIn);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public string GetLiderID(int? IdProjectPK)
        {
            if (IdProjectPK == null)
            {
                return null;
            }
            else
            {
                WorksIn worksIn = db.WorksIn.Where(p => p.idProjectFKPK == IdProjectPK && p.role == 1).ToList().First();
                if(worksIn == null)
                {
                    return null;
                }
                else
                {
                    return worksIn.idEmployeeFKPK;
                }
            }
        }

        public string GetLiderName(int? IdProjectPK)
        {
            if (IdProjectPK == null)
            {
                return null;
            }
            else
            {
                WorksIn worksIn = db.WorksIn.Where(p => p.idProjectFKPK == IdProjectPK && p.role == 1).ToList().First();
                if (worksIn == null)
                {
                    return null;
                }
                else
                {
                    var EmployeesController = new EmployeesController();
                    return EmployeesController.getEmployeeName(worksIn.idEmployeeFKPK);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /*
        * Efecto: Selecciona los empleados dentro de un equipo y los despliega en la vista de editar
        * Requiere: El id del proyecto(Lo pasa la vista mediante AJAX)
        * Modifica: La vista de editar equipos
        */
        public ActionResult bringTeam(string currentProject)
		{
            //Listas y variables que nos ayudan a obtener los valores necesarios para obtener el equipo de un proyecto
            List<string> idMembers = new List<string>();
            List<string> nameMembers = new List<string>();
            List<string[]> skillsMembers = new List<string[]>();
            string auxName;
            List<string> auxSkill = new List<string>();
            int thisProject = Int32.Parse(currentProject);
            ViewBag.currentTeam = new SelectList(db.WorksIn.Where(e => (e.idProjectFKPK == thisProject)), "idEmployeeFKPK", "idProjectFKPK");
            ViewBag.skillsTeam = new SelectList(db.DeveloperKnowledge, "idEmployeeFKPK", "devKnowledgePK");
            foreach (var member in ViewBag.currentTeam) //Para cada miembro del equipo
            {
                var Employee = db.Employee.Find(member.Value);
                idMembers.Add(member.Value);
                auxName = Employee.employeeName + " " + Employee.employeeLastName;
                nameMembers.Add(auxName);
                foreach(var skill in ViewBag.skillsTeam) //busque sus habilidades
                {
                    if(member.Value == skill.Value && skill.Text != null)
                    {
                        auxSkill.Add(skill.Text);
                    }
                }
                skillsMembers.Add(auxSkill.ToArray());
                auxSkill.Clear();
            }
            //Retonamos un JSON con los valores que necesitamos para poder mostrar el equipo en la vista para un equipo
			return Json(new
			{
				ids = idMembers.ToArray(),
                names = nameMembers.ToArray(),
                knowledges = skillsMembers.ToArray()
            });
		}
	}
}
		 
