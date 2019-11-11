using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FeLuisesScrumDEV.Models;
using System.ComponentModel.DataAnnotations;
using FeLuisesScrumDEV.viewModel;

namespace FeLuisesScrumDEV.Controllers
{
    public class RequerimentsController : Controller
    {
        private FeLuisesEntities db = new FeLuisesEntities();

        // GET: Requeriments
        //EFE: Lista todos los requerimientos de un proyecto y un m{odulo dado. Filta según el usuario
        //REQ: Proyecto y módulo existente
        public ActionResult Index()
        {
            var requeriment = db.Requeriment.Include(r => r.Employee).Include(r => r.Module);
            ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName");
            return View(requeriment.ToList());
        }

        // GET: Requeriments/Details/5
        //EFE: Detalles del Requerimiento seleccionado.
        public ActionResult Details(int? idProjectFKPK, int? idModuleFKPK, int? idRequerimentPK)
        {
            if (idProjectFKPK == null || idModuleFKPK == null || idRequerimentPK == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requeriment requeriment = db.Requeriment.Find(idProjectFKPK, idModuleFKPK, idRequerimentPK);
            if (requeriment == null)
            {
                return HttpNotFound();
            }
            return View(requeriment);
        }

        // GET: Requeriments/Create
        //EFE: Crea un requerimieno ya asociado a un módulo y proyecto
        public ActionResult Create(int? idProjectFKPK, int? idModuleFKPK)
        {
            if (idProjectFKPK == null || idModuleFKPK == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var moduleController = new ModulesController();
            var employeeController = new EmployeesController();
            Module module = moduleController.GetModule(idProjectFKPK, idModuleFKPK);
            Requeriment requeriment = new Requeriment { Module = module };
            ViewBag.idEmployeeFK = employeeController.EmployeeFromTeamSelectList((int)idProjectFKPK);
            ViewBag.complexity = SelectListComplexity(null);
            ViewBag.status = SelectListStatus(null);
            return View(requeriment);
        }

        // POST: Requeriments/Create
        //EFE: Valida los campos de creación
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProjectFKPK,idModuleFKPK,idRequerimentPK,idEmployeeFK,estimatedDuration,realDuration,status,startingDate,endDate,complexity,objective")] Requeriment requeriment)
        {
            if (ModelState.IsValid)
            {
                if (requeriment.startingDate < requeriment.endDate)
                {
                    db.Requeriment.Add(requeriment);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else 
                {
                    ModelState.AddModelError("startingDate", "La fecha de inicio no puede ser despues de la fecha de finalización.");
                }
            }
            var employeeController = new EmployeesController();
            ViewBag.idEmployeeFK = employeeController.EmployeeFromTeamSelectList(requeriment.idProjectFKPK);
            ViewBag.complexity = SelectListComplexity(requeriment.complexity);
            ViewBag.status = SelectListStatus(requeriment.status);
            return View(requeriment);
        }


        // GET: Requeriments/Edit/5
        //EFE: Edita un requerimiento seleccionado
        public ActionResult Edit(int? idProjectFKPK, int? idModuleFKPK, int? idRequerimentPK)
        {
            if (idProjectFKPK == null || idModuleFKPK == null || idRequerimentPK == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requeriment requeriment = db.Requeriment.Find(idProjectFKPK, idModuleFKPK, idRequerimentPK);
            if (requeriment == null)
            {
                return HttpNotFound();
            }
            ViewBag.idEmployeeFK = new SelectList(db.Employee, "idEmployeePK", "employeeName", requeriment.idEmployeeFK);
            ViewBag.idModuleFKPK = new SelectList(db.Module.Where(x => x.idProjectFKPK == idProjectFKPK), "idModulePK", "name", requeriment.idModuleFKPK);
            ViewBag.complexity = SelectListComplexity(null);
            ViewBag.status = SelectListStatus(null);
            return View(requeriment);
        }

        // POST: Requeriments/Edit/5
        //EFE: Valida los campos de edición de un requesimiento
        //REQ: Campos obligatorios
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProjectFKPK,idModuleFKPK,idRequerimentPK,idEmployeeFK,objective,estimatedDuration,realDuration,status,startingDate,endDate,complexity")] Requeriment requeriment)
        {
            if (ModelState.IsValid)
            {
                if (requeriment.startingDate < requeriment.endDate)
                {
                    int OidModuleFKPK = Convert.ToInt32(Request["idModuleFKPK"]);
                    if (OidModuleFKPK != requeriment.idModuleFKPK)
                    {
                        var reqToDelete = db.Requeriment.Where(r => r.idModuleFKPK == OidModuleFKPK && r.idProjectFKPK == requeriment.idProjectFKPK && r.idRequerimentPK == requeriment.idRequerimentPK).FirstOrDefault();
                        db.Requeriment.Remove(reqToDelete);
                        db.Requeriment.Add(requeriment);
                    }
                    else
                    {
                        db.Entry(requeriment).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("startingDate","La fecha de inicio no puede ser despues de la fecha de finalización.");
                }
            }
            ViewBag.idEmployeeFK = new SelectList(db.Employee, "idEmployeePK", "employeeName", requeriment.idEmployeeFK);
            ViewBag.idModuleFKPK = new SelectList(db.Module, "idModulePK", "name", requeriment.idProjectFKPK);
            ViewBag.complexity = SelectListComplexity(requeriment.complexity);
            ViewBag.status = SelectListStatus(requeriment.status);
            return View(requeriment);
        }

        // GET: Requeriments/Delete/5
        //EFE: Elimina el requerimiento seleccionado.
        public ActionResult Delete(int? idProjectFKPK, int? idModuleFKPK, int? idRequerimentPK)
        {
            if (idProjectFKPK == null || idModuleFKPK == null || idRequerimentPK == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requeriment requeriment = db.Requeriment.Find(idProjectFKPK, idModuleFKPK, idRequerimentPK);
            if (requeriment == null)
            {
                return HttpNotFound();
            }
            return View(requeriment);
        }

        // POST: Requeriments/Delete/5
        //EFE: Valida que se haya dado.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? idProjectFKPK, int? idModuleFKPK, int? idRequerimentPK)
        {
            Requeriment requeriment = db.Requeriment.Find(idProjectFKPK, idModuleFKPK, idRequerimentPK);
            db.Requeriment.Remove(requeriment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //EF: Libera los recursos no manejados de manera automatica por esta entidad, en este caso libera la instancia que representa la base de datos
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // EF: Retorna una lista con los requerimientos asociados a dicho proyecto que se encuentran dentro de cierto modulo
        // REQ: Que exista dicho proyecto y modulo
        public List<Requeriment> RequerimentList(int? idProjectFKPK, int? idModuleFKPK)
        {
            if (idProjectFKPK == null || idModuleFKPK == null)
            {
                return null;
            }
            var requeriments = db.Requeriment.Where(r => r.idProjectFKPK == idProjectFKPK && r.idModuleFKPK == idModuleFKPK);
            if (requeriments == null)
            {
                return null;
            }
            return requeriments.ToList();
        }

        // EF: Retorna una vista con los modulos asociados al proyecto y permite una seleccion de los modulos disponibles
        // REQ: que existan proyectos en la base de datos
        public PartialViewResult SelectModule(int? idProjectFKPK)
        {
            if (idProjectFKPK == null)
                return null;
            var moduleController = new ModulesController();
            ViewBag.idModuleFKPK = moduleController.ModuleSelectList(idProjectFKPK);
            SelectModuleViewModel model = new SelectModuleViewModel();
            model.idProjectFKPK = (int)idProjectFKPK;
            return PartialView("SelectModule", model);
        }

        // EF: Retorna una vista con los requerimiento asociados a un proyecto y un modulo
        // REQ: que existan proyectos con modulos en la base de datos
        public PartialViewResult GetRequeriments(int? idProjectFKPK, int? idModuleFKPK)
        {
            if (idProjectFKPK == null || idModuleFKPK == null)
            {
                return null;
            }
            List<Requeriment> requerimentsAssociated = RequerimentList(idProjectFKPK, idModuleFKPK);
            return PartialView("GetRequeriments", requerimentsAssociated);
        }

        // EF: Retorna una select list con los distintos estados de dificultad que puede tener un requerimiento
        private SelectList SelectListComplexity(int? defaultValue)
        {
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "No asignado" },
                new SelectListItem { Value = "1", Text = "Simple" },
                new SelectListItem { Value = "2", Text = "Mediano" },
                new SelectListItem { Value = "3", Text = "Complejo" },
                new SelectListItem { Value = "4", Text = "Muy complejo" }
            };
            if (defaultValue == null)
                return new SelectList(list, "Value", "Text");
            else
                return new SelectList(list, "Value", "Text", defaultValue);
        }

        // EF: Retorna una select list con los distintos estados que puede tener un requerimiento
        public SelectList SelectListStatus(int? defaultValue)
        {
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "No iniciado" },
                new SelectListItem { Value = "1", Text = "En proceso" },
                new SelectListItem { Value = "2", Text = "Interrumpido" },
                new SelectListItem { Value = "3", Text = "Completado" }
            };
            if (defaultValue == null)
                return new SelectList(list, "Value", "Text");
            else
                return new SelectList(list, "Value", "Text", defaultValue);
        }
    }
}
