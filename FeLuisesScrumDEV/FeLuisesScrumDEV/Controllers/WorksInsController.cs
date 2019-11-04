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
        public ActionResult Index()
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
        public ActionResult Index(string[] teamMembers, string currentProject)
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
        * Modifica: La vista de index equipos
        */
        public ActionResult bringTeam(string currentProject)
		{
            //Listas y variables que nos ayudan a obtener los valores necesarios para obtener el equipo de un proyecto
            List<string> idMembers = new List<string>();
            List<string> nameMembers = new List<string>();
            List<string[]> skillsMembers = new List<string[]>();
            string auxName;
            string auxLeaderName = "";
            List<string> auxSkill = new List<string>();
            int thisProject = Int32.Parse(currentProject);
            ViewBag.currentTeam = new SelectList(db.WorksIn.Where(e => (e.idProjectFKPK == thisProject)), "idEmployeeFKPK", "idProjectFKPK");
            ViewBag.skillsTeam = new SelectList(db.DeveloperKnowledge, "idEmployeeFKPK", "devKnowledgePK");
            foreach (var member in ViewBag.currentTeam) //Para cada miembro del equipo
            {
                var Employee = db.Employee.Find(member.Value);
                auxName = Employee.employeeName + " " + Employee.employeeLastName;
                var auxLeader = db.WorksIn.Find(member.Value, thisProject);
                if(auxLeader.role == 1)
                {
                    auxLeaderName = auxName;
                }
                else{
                    idMembers.Add(member.Value);

                    nameMembers.Add(auxName);
                    foreach (var skill in ViewBag.skillsTeam) //busque sus habilidades
                    {
                        if (member.Value == skill.Value && skill.Text != null)
                        {
                            auxSkill.Add(skill.Text);
                        }
                    }
                    skillsMembers.Add(auxSkill.ToArray());
                    auxSkill.Clear();
                }  
            }
            //Retonamos un JSON con los valores que necesitamos para poder mostrar el equipo en la vista para un equipo
            return Json(new
            {
                ids = idMembers.ToArray(),
                names = nameMembers.ToArray(),
                knowledges = skillsMembers.ToArray(),
                leaderName = auxLeaderName
            }); 
		}

        /*
       * Efecto: Selecciona el nombre del lider de equipo y lo despliega en la vista de crear
       * Requiere: El id del proyecto(Lo pasa la vista mediante AJAX)
       * Modifica: La vista de crear equipos
       */
        public ActionResult bringLeader(string currentProject)
        {
            int thisProject = Int32.Parse(currentProject);
            string auxLeader = "";
            var leaders = new SelectList(db.WorksIn.Where(e => (e.idProjectFKPK == thisProject && e.role == 1)), "idEmployeeFKPK", "idProjectFKPK");
            foreach(var myLeader in leaders)
            {
                if(myLeader.Text == currentProject)
                {
                    var leaderEntity = db.Employee.Find(myLeader.Value);
                    if(leaderEntity != null)
                    {
                        auxLeader = leaderEntity.employeeName + " " + leaderEntity.employeeLastName;
                    }
                    
                    break;
                }
            }

            return Json(new
            {
                leadersName = auxLeader
            });
        }

        // EF: Consulta si existe un usuario dentro de cierto proyecto y cual es su rol en el
        // REQ: Que se introduzca las llaves de la tupla a consultar
        public int isEmployee(string idEmployeeFKPK, int idProjectFKPK )
        {
            var employee = db.WorksIn.Where(e => e.idEmployeeFKPK == idEmployeeFKPK && e.idProjectFKPK == idProjectFKPK);
            if (employee.Any() == false)
                return -1; // en caso de que no se encuentre una tupla tal, retorna -1 diciendo que no existe ninguna coincidencia
            else if (employee.Any(e => e.role == 0))
                return 0; // en caso de que se encuentra y el rol dice que es un desarrollador
            else
                return 1; // en caso de que se encuentra y el rol dice que es un lider
        }

        public List<WorksIn> GetMembers(int idProjectFKPK)
        {
            return db.WorksIn.Where(w => w.idProjectFKPK == idProjectFKPK).ToList();
        }
    }
}
		 
