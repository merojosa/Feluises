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
        //EFE: Lista los proyectos mostrando algunos atributos. Filtra según el empleado o cliente
        //REQ: Un usuario loggueado
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
        //EFE: Detalles del proyecto seleccionado
        public ActionResult Details(int? id)
        {
            var WorksInController = new WorksInsController(); //controlador para obtener el líder del proyecto
            var lider = WorksInController.GetLiderName(id); //método que botiene líder
            ViewBag.idLiderFK = lider; //viewbag que es llamado para desplegar el lider obtenido
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
        //EFE: Crea un nuevo proyecto.
        public ActionResult Create()
        {
            ViewBag.idClientFK = new SelectList(db.Client, "idClientPK", "clientName");
            var EmployeesController = new EmployeesController(); //Controlador de empleados
            var availableEmployees = EmployeesController.AvailableEmployees(); //retorna los desarrolladores disponibles.
            ViewBag.idEmployeeFKPK = new SelectList(availableEmployees, "idEmployeePK", "employeeName"); //viewbag para desplegar el dropdown
            return View();
        }

        // POST: Projects/Create
        //EFE: Valida los campos de creación de un proyecto
        //REQ: Campos obligatorios
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProjectPK,projectName,objective,estimatedCost,realCost,startingDate,finishingDate,budget,estimatedDuration,idClientFK")] Project project, [Bind(Include = "idEmployeeFKPK")] WorksIn employee)
        {
            var EmployeesController = new EmployeesController(); //Controlador de empleados
            var availableEmployees = EmployeesController.AvailableEmployees(); //retorna los desarrolladores disponibles.
            if ( db.Project.Any(x => x.projectName == project.projectName)) //si el nombre del proyecto ya existe
            {
                ModelState.AddModelError("projectName", "Ya existe un proyecto registrado con ese nombre");
                ViewBag.idClientFK = new SelectList(db.Client, "idClientPK", "clientName"); //viewbag para desplegar el dropdown de clientes
                ViewBag.idEmployeeFKPK = new SelectList(availableEmployees, "idEmployeePK", "employeeName"); //viewbag para desplegar el dropdown
                return View(project); //no guarda cambios y actualiza la vista.
            }
            if ( ModelState.IsValid ) //si el modelo es válido
            {
                db.Project.Add(project); //agrega el proyecto a la tabla
                employee.idProjectFKPK = project.idProjectPK;
                employee.role = 1;
                db.WorksIn.Add(employee); //agrega la relación de lider a la tabla worksIn
                db.SaveChanges(); //guarda cambios
                return RedirectToAction("Index"); //vuelve al index.
            }
            //Si no entra en ninguno de los 2 ifs entonces actualiza la vista con los mismos viewbags.
            ViewBag.idClientFK = new SelectList(db.Client, "idClientPK", "clientName", project.idClientFK);
            ViewBag.idEmployeeFKPK = new SelectList(availableEmployees, "idEmployeePK", "employeeName", employee);
            return View(project);
        }

        // GET: Projects/Edit/5
        //EFE: Edita el proyecto seleccionado
        // MOD: el proyecto con el id especificado.
        public ActionResult Edit(int? id)
        {
            var EmployeesController = new EmployeesController(); //Controlador de empleados
            var availableEmployees = EmployeesController.AvailableEmployeesAndLider(id); //retorna la lista de desarrolladores disponibles incluyendo al líder actual del proyecto
            var WorksInController = new WorksInsController(); //Controlador de relaciones desarrollador/proyecto.
            var lider = WorksInController.GetLiderID(id); //Retorna el id del líder el proyecto actual para ponerlo como default en el dropdown
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
            ViewBag.idEmployeeFKPK = new SelectList(availableEmployees, "idEmployeePK", "employeeName", lider); //dropdown con lider por defecto
            return View(project);
        }

        // POST: Projects/Edit/5
        //EFE: Verifica y valida todos los campos de edición
        //REQ; Campos obligatorios.
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
            var aux = employee.idEmployeeFKPK;
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                if (employee.idEmployeeFKPK == lider)
                {
                    //Si el nuevo id es igual al ya asignado, entonces no hace nada.
                }
                else
                {   //en caso contrario cambia los datos.
                    if(lider == null) //si no hay lider asignado, crea una nueva relación con el líder elegido
                    {
                        employee.idProjectFKPK = project.idProjectPK;
                        employee.role = 1;
                        db.WorksIn.Add(employee);
                    } else
                    { //si ya hay uno asignado
                        
                        var exLider = db.Employee.Find(lider);
                        exLider.availability = 0; //cambia la disponibilidad del antiguo a disponible.
                        var modificating = db.WorksIn.Find(lider, project.idProjectPK);
                        db.WorksIn.Remove(modificating); //remueve la relación del líder anterior (dado que no es modificable)
                        employee.idProjectFKPK = project.idProjectPK;
                        employee.role = 1;
                        db.WorksIn.Add(employee); //y crea una nueva con el nuevo líder.
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idClientFK = new SelectList(db.Client, "idClientPK", "clientName", project.idClientFK);
            ViewBag.idEmployeeFKPK = new SelectList(availableEmployees, "idEmployeePK", "employeeName", lider);
            return View(project);
        }

        // GET: Projects/Delete/5
        //EFE: Elimina el proyecto seleccionado
        public ActionResult Delete(int? id)
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

        // POST: Projects/Delete/5
        //EFE: Verifica si se elimió el proyecto seleccionado.
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