﻿using System;
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
        //EFE: Lista todos los empleados que hayan
        public ActionResult Index()
        {
            return View(db.Employee.ToList());
        }

        // GET: Employees/Details/5
        //EFE: muestra los detalles del empleado seleccionado.
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
        // EFE: Crea un nuevo empleado
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        //EFE: Valida los campos de creación
        //REQ: Campos obligatorios
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
                if (employee.employeeBirthDate < employee.employeeHireDate)
                {
                    db.Employee.Add(employee);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("employeeBirthDate", "La fecha de nacimiento no puede ser despues de la fecha de contratación.");
                }
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        //EFE: Detalles del empleado seleccionado.
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
        //EFE: Valida los compos de edición.
        //REQ: Campos obligatorios
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEmployeePK,employeeName,employeeLastName,employeeSecondLastName,employeeBirthDate,employeeHireDate,developerFlag,tel,email,province,canton,district,exactDirection,pricePerHour,availability")] Employee employee)
        {

            if (ModelState.IsValid)
            {
                if (employee.employeeBirthDate < employee.employeeHireDate)
                {
                    db.Entry(employee).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("employeeBirthDate", "La fecha de nacimiento no puede ser despues de la fecha de contratación.");
                }
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        //EFE: Elimia un empleado
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
        // EF: Retorna una lista con los empleados disponibles.
        // REQ: NA
        // MOD: NA
        public List<Employee> AvailableEmployees()
        {
            var availableEmployees = db.Employee.Where(m => m.availability == 0 && m.developerFlag == 1);
            if (availableEmployees == null)
            {
                return null;
            }
            return availableEmployees.ToList();
        }
        // POST: Employees/Delete/5
        //EFE: Verifica si se guardó la acción
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Employee employee = db.Employee.Find(id);
            db.Employee.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // EF: Retorna el nombre del empleado con el id pasado por parámetro
        // REQ: Que exista el id
        // MOD: NA
        public string getEmployeeName(string id)
        {
            Employee employee = db.Employee.Find(id); //método simple que busca el id en la tabla
            if (employee == null) //si no existe retorna null
            {
                return null;
            }
            else //si existe entonces retorna el nombre del empleado.
            {
                return employee.employeeName;
            }
        }
        // EF: Retorna una lista con los empleados disponibles y el lider actual de un proyecto específico.
        // REQ: Que exista el proyecto
        // MOD: NA
        public List<Employee> AvailableEmployeesAndLider(int? id)
        {
            var availableEmployees = db.Employee.Where(m => m.availability == 0 && m.developerFlag == 1);
            if (availableEmployees == null)
            {
                return null;
            }
            var list = availableEmployees.ToList();
            var WorksInController = new WorksInsController();
            var lider = WorksInController.GetLiderID(id);
            var liderEmployee = db.Employee.Where(m => m.idEmployeePK == lider);
            if (liderEmployee.ToList().Count() <= 0)
            {

            }
            else
            {
                list.Add(liderEmployee.ToList().First());
            }
            return list;
        }
        //falta
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //falta
        public SelectList EmployeeFromTeamSelectList(int? idProject, int? devDefault)
        {
            IQueryable<WorksIn> test = db.WorksIn.Where(w => w.idProjectFKPK == idProject && w.role == 0);
            IQueryable<Requeriment> req = db.Requeriment.Where(r => r.idProjectFKPK == idProject);
            IQueryable<Employee> team = db.Employee.Where(e => test.Any(t => t.idEmployeeFKPK == e.idEmployeePK) == true && req.Where(r => r.idEmployeeFK == e.idEmployeePK).Count() <= 10 );
            if( devDefault == null )
                return new SelectList(team, "idEmployeePK", "employeeName");
            else
                return new SelectList(team, "idEmployeePK", "employeeName", devDefault);
        }
    }
}