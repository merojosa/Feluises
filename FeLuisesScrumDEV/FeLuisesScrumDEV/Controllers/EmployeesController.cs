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
    public class EmployeesController : Controller
    {
        private FeLuisesEntities db = new FeLuisesEntities();

        // GET: Employees
        public ActionResult Index()
        {
            return View(db.Employee.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idEmployeePK,employeeName,employeeLastName,employeeSecondLastName,employeeBirthDate,employeeHireDate,developerFlag,tel,email,province,canton,district,exactDirection,pricePerHour,availability")] Employee employee)
        {
            if (db.Client.Any(x => x.idClientPK == employee.idEmployeePK) || db.Employee.Any(x => x.idEmployeePK == employee.idEmployeePK))
            {
                ModelState.AddModelError("idEmployeePK", "Ya existe un usuario registrado con dicha cédula");
                return View(employee);
            }
            if (ModelState.IsValid)
            {
                db.Employee.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEmployeePK,employeeName,employeeLastName,employeeSecondLastName,employeeBirthDate,employeeHireDate,developerFlag,tel,email,province,canton,district,exactDirection,pricePerHour,availability")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }
        // EF: Retorna una lista con los módulos asociados a dicho proyecto
        // REQ: Que exista dicho proyecto
        public List<Employee> AvailableEmployees()
        {
            var availableEmployees = db.Employee.Where(m => m.availability == 0);
            if (availableEmployees == null)
            {
                return null;
            }
            return availableEmployees.ToList();
        }
        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Employee employee = db.Employee.Find(id);
            db.Employee.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public string getEmployeeName(string id)
        {
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return null;
            }
            else
            {
                return employee.employeeName;
            }
        }

        public List<Employee> AvailableEmployeesAndLider(int? id)
        {
            var availableEmployees = db.Employee.Where(m => m.availability == 0);
            if (availableEmployees == null)
            {
                return null;
            }
            var list = availableEmployees.ToList();
            var WorksInController = new WorksInsController();
            var lider = WorksInController.GetLiderID(id);
            var liderEmployee = db.Employee.Where(m => m.idEmployeePK == lider);
            list.Add(liderEmployee.ToList().First());
            return list;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public SelectList EmployeeFromTeamSelectList(int? idProject)
        {

            //var worksInController = new WorksInsController();
            //List<WorksIn> worksInRegisters = worksInController.GetMembers((int)idProject);
            //IEnumerable<Employee> team = db.Employee.Where(e => worksInRegisters.Exists(x => x.idEmployeeFKPK == e.idEmployeePK));
            IQueryable<WorksIn> test = db.WorksIn.Where(w => w.idProjectFKPK == idProject);
            IQueryable<Employee> team = db.Employee.Where(e => test.Any(t => t.idEmployeeFKPK == e.idEmployeePK) == true);
            return new SelectList(team, "idEmployeePK", "employeeName");
        }
    }
}
