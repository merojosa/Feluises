using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FeLuisesScrumDEV.Models;
using FeLuisesScrumDEV.viewModel;

namespace FeLuisesScrumDEV.Controllers
{
    public class ModulesController : Controller
    {
        private FeLuisesEntities db = new FeLuisesEntities();

        // GET: Modules
        //EF: Genera y devuelve la vista de Modules/Index donde muestra todos los proyectos con sus respectivos modulos
        public ActionResult Index()
        {
           // if (Convert.ToInt32(Session["userRole"]) == 0)
            //{
                List<IndexViewModel> arrangedList = GetProjectsModules();
                ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName");
                return View(arrangedList);
            //} else if (Convert.ToInt32(Session["userRole"]) == 2)
            //{
            /*
            var usr = Session["userName"].ToString();
                var participacion = db.WorksIn.Where(w => w.idEmployeeFKPK == usr);
                List<IndexViewModel> arrangedList = GetProjectsModules();
                ViewBag.idProjectFKPK = new SelectList(db.Project.Where(p => participacion.Any(w => w.idProjectFKPK == p.idProjectPK)), "idProjectPK", "projectName");
                return View(arrangedList);
            }
            return null;
            */
        }

        //EF: Produce una lista con objetos que agrupan los modulos segun su proyecto para mejor visualización
        private List<IndexViewModel> GetProjectsModules()
        {
            // recorre a todos los modulos agrupandolos segun el proyecto y los retorna en una lista que por objeto
            // posee una lista de modulos asociados mas un proyecto
            var arrangedList = new List<IndexViewModel>();
            var module = db.Module.Include(m => m.Project);
            IndexViewModel dummy = new IndexViewModel();
            int lastPKchecked = -1;
            foreach (var item in module)
            {
                if (item.idProjectFKPK == lastPKchecked && item.idModulePK != -1)
                {
                    // en caso de que el modulo pertenezca al mismo proyecto que se esta agregando modulos se agrega a la lista
                    dummy.AssociatedModules.Add(item);
                }
                else
                {
                    // en caso de ser el primer proyecto que se esta iterando como es un proyecto distinto agrega el anterior a la lista que retorna y comienza
                    // a agrupar los del proyecto actual
                    if (lastPKchecked != -1)
                        arrangedList.Add(dummy);
                    dummy = new IndexViewModel();
                    dummy.Project = item.Project;
                    // en caso de que el modulo no sea el modulo comodin que se designo para guardar los requerimientos, se agrega a los modulos asociados de dicho proyecto
                    if (item.idModulePK != -1)
                        dummy.AssociatedModules.Add(item);
                    // se actualiza el valor del ultimo proyecto revisado
                    lastPKchecked = item.idProjectFKPK;
                }
            }
            // Siempre se deja un proyecto al final sin agregar a la lista si hay más de un elemento dado que hay que comparar al previo con el actual, entonces se concatena a la lista
            if (lastPKchecked != -1)
                arrangedList.Add(dummy); ;
            return arrangedList;
        }

        // GET: Modules/Details/5
        // EF: Genera una vista con los detalles del modulo que se desea ver
        // REQ: Que se introduzca las llaves del modulo a consultar
        public ActionResult Details(int? idProjectFKPK, int? idModulePK)
        {
            if (idProjectFKPK == null || idModulePK == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Module.Find(idProjectFKPK, idModulePK);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // GET: Modules/Create
        // EF: Genera un formulario para la creación de un módulo
        public ActionResult Create(int idProjectFKPK)
        {
            Project project = db.Project.Where(p => p.idProjectPK==idProjectFKPK).ToList().First();
            Module module = new Module { Project=project, name="u gei"};
            return View(module);
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        // EF: Genera un módulo en la base de datos con la información brindada
        // REQ: Que se haya llenado el formulario respectivo.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProjectFKPK,idModulePK,name")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Module.Add(module);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName", module.idProjectFKPK);
            return View(module);
        }

        // GET: Modules/Edit/5
        // EF: Genera un formulario para la edición de un módulo
        public ActionResult Edit(int? idProjectFKPK, int? idModulePK)
        {
            if (idProjectFKPK == null || idModulePK == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (idModulePK == -1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            Module module = db.Module.Find(idProjectFKPK, idModulePK);
            if (module == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName", module.idProjectFKPK);
            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        // EF: Genera un cambio a un módulo en la base de datos con la información brindada
        // REQ: Que se haya llenado el formulario respectivo, que la información sea valida y que exista dicho módulo a modificar.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProjectFKPK,idModulePK,name")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idProjectFKPK = new SelectList(db.Project, "idProjectPK", "projectName", module.idProjectFKPK);
            return View(module);
        }

        // GET: Modules/Delete/5
        // EF: Genera una vista para eliminar un módulo
        // REQ: Que exista dicho módulo a eliminar
        public ActionResult Delete(int? idProjectFKPK, int? idModulePK)
        {
            if (idProjectFKPK == null || idModulePK == null || idModulePK == -1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Module.Find(idProjectFKPK, idModulePK);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Delete/5
        // EF: Realiza la acción de eliminación de dicho módulo
        // REQ: Que exista dicho módulo a eliminar
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? idProjectFKPK, int? idModulePK)
        {
            if (idModulePK == -1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            Module module = db.Module.Find(idProjectFKPK,idModulePK);
            db.Module.Remove(module);
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

        // EF: Retorna una lista con los módulos asociados a dicho proyecto
        // REQ: Que exista dicho proyecto
        public List<Module> ModuleList(int? idProjectFKPK)
        {
            if (idProjectFKPK == null)
            {
                return null;
            }
            var modules = db.Module.Where(m => m.idProjectFKPK==idProjectFKPK);
            if (modules == null)
            {
                return null;
            }
            return modules.ToList();
        }

        // EF: Retorna una vista con los módulos asociados al proyecto que se consulta
        public PartialViewResult GetModules(int? idProjectFKPK)
        {
            if(idProjectFKPK == null)
                return null;
            var moduleController = new ModulesController();
            List<Module> modulesAssociated = moduleController.ModuleList(idProjectFKPK);
            return PartialView("GetModules", modulesAssociated);
        }
    }
}
