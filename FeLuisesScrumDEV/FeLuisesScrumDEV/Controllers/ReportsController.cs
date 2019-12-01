using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FeLuisesScrumDEV.Models;
using FeLuisesScrumDEV.viewModel;

namespace FeLuisesScrumDEV.Controllers
{
    public class ReportsController : Controller
    {
        private FeLuisesEntities db = new FeLuisesEntities();

        //La pantalla en la que estan todos los accesos a los reportes
        public ActionResult Index()
        {
            return View();
        }

        //Ver todos los desarrolladores, disponibles y ocupados, en su proyecto
        public ActionResult DeveloperState()
        {
            return View();
        }

        //Conocer los periodos de desocupación de los desarrolladores.
        public ActionResult InactivityPeriod()
        {
            return View();
        }

        //Buscar desarrolladores de acuerdo al tipo de conocimiento y disponibilidad.
        public ActionResult devKnowledgeDisponibility()
        {
            return View();
        }

        //Cantidad de requerimientos por desarrollador para un proyecto específico
        public ActionResult numRequirmnetsPerDev()
        {
            return View();
        }

        //Comparar la duración estimada y real para requerimiento de un desarrollador.
        public ActionResult estimatedRealTimeDev()
        {
            return View();
        }

        //Total de horas estimadas y reales requeridas por un proyecto.
        public ActionResult totalHours()
        {
            return View();
        }


        //Historial de participación de un desarrollador en diferentes proyectos.
        public ActionResult devsHistoryOnProjects([Bind(Include = "employee")]string employee)
        {
            var query = from wi in db.WorksIn
                        join p in db.Project on wi.idProjectFKPK equals p.idProjectPK
                        join e in db.Employee on wi.idEmployeeFKPK equals e.idEmployeePK
                        join r in db.Requeriment on e.idEmployeePK equals r.idEmployeeFK
                        where r.idProjectFKPK == p.idProjectPK && (e.employeeName.ToLower().Contains(employee.ToLower()) || e.employeeLastName.ToLower().Contains(employee.ToLower()))
                        group new { wi, p, e, r } by new {e.employeeName, e.employeeLastName, p.projectName, wi.role} into wiGroup
                        select new
                        {
                            Nombre_Empleado = wiGroup.Key.employeeName,
                            Apellido_Empleado = wiGroup.Key.employeeLastName,
                            Nombre_Proyecto = wiGroup.Key.projectName,
                            rol = wiGroup.Key.role,
                            Horas_Trabajadas = wiGroup.Sum(w => w.r.realDuration)
                        };
            var liderQuery = from wi in db.WorksIn
                             join p in db.Project on wi.idProjectFKPK equals p.idProjectPK
                             join e in db.Employee on wi.idEmployeeFKPK equals e.idEmployeePK
                             where (e.employeeName.ToLower().Contains(employee.ToLower()) || e.employeeLastName.ToLower().Contains(employee.ToLower())) && wi.role == 1
                             group new { wi, p, e } by new { e.employeeName, e.employeeLastName, p.projectName, wi.role } into leadGroup
                             select new
                             {
                                 Nombre_Empleado = leadGroup.Key.employeeName,
                                 Apellido_Empleado = leadGroup.Key.employeeLastName,
                                 Nombre_Proyecto = leadGroup.Key.projectName,
                                 rol = leadGroup.Key.role
                             };
            var results = query.ToList().Select(r => new GetHistory_Results_Mapped
            {
                Nombre_Empleado = r.Nombre_Empleado,
                Apellido_Empleado = r.Apellido_Empleado,
                Nombre_Proyecto = r.Nombre_Proyecto,
                Rol = r.rol,
                Horas_trabajadas = r.Horas_Trabajadas
            }).ToList();

            var results2 = liderQuery.ToList().Select(p => new GetHistory_Results_Mapped
            {
                Nombre_Empleado = p.Nombre_Empleado,
                Apellido_Empleado = p.Apellido_Empleado,
                Nombre_Proyecto = p.Nombre_Proyecto,
                Rol = p.rol
            }).ToList();
            results = results.Concat(results2).ToList();
            return View(results);
        }

        //Ver el estado de avance de un requerimiento.
        public ActionResult requirmentState()
        {
            return View();
        }

        //Comparar las duraciones reales y estimadas entre requerimientos de un mismo nivel de complejidad.
        public ActionResult compareReqStatsbyComplexity()
        {
            var query = from r in db.Requeriment
                        where r.status == 3
                        group r by r.complexity into rGroup
                        select new
                        {
                            Dificultad = rGroup.Key,
                            Total = rGroup.Count(),
                            Minima_Diff = rGroup.Min(x => Math.Abs((int)x.estimatedDuration - (int)x.realDuration)),
                            Max_Diff = rGroup.Max(x => Math.Abs((int)x.estimatedDuration - (int)x.realDuration)),
                            Avg_Diff = rGroup.Average(x => Math.Abs((int)x.estimatedDuration - (int)x.realDuration)),
                            Avg_Real = rGroup.Average(x => x.realDuration),
                            Avg_Est = rGroup.Average(x => x.estimatedDuration)
                        };
            var results = query.ToList().Select(r => new GetReqStatsbyComplexity_Result_Mapped
            {
                Dificultad = r.Dificultad,
                Total = r.Total,
                Minima_Diff = r.Minima_Diff,
                Max_Diff = r.Max_Diff,
                Avg_Diff = (int)r.Avg_Diff,
                Avg_Real = (int)r.Avg_Real,
                Avg_Est = (int)r.Avg_Est
            }).ToList();
            return View(results);
        }

        public ActionResult devsKnowledgeLaborSeniority()
        {
            return View();
        }

        //Cantidad de desarrolladores con conocimientos específicos y promedio de antigüedad laboral.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult devsKnowledgeLaborSeniority([Bind(Include = "Knowledge")]string knowledge)
        {
            var now = DateTime.Now;
            var basequery = db.DeveloperKnowledge.Include(c => c.idEmployeeFKPK);
            var query = from dk in basequery
                        where dk.devKnowledgePK.ToLower().Contains(knowledge.ToLower())
                        group dk by dk.devKnowledgePK into dkGroup
                        orderby dkGroup.Key descending
                        select new
                        {
                            Conocimiento = dkGroup.Key,
                            Total = dkGroup.Count(),
                            Promedio_Antiguedad = (int)dkGroup.Average(x => DbFunctions.DiffDays(x.Employee.employeeHireDate,now))//(long)dkGroup.Average(x => DateTime.Now.Ticks-(x.Employee.employeeHireDate).Ticks)
                        };
            var results = query.ToList().Select(x => new GetKnowledgesSP_Result_Mapped
            {
                Conocimiento = x.Conocimiento,
                Promedio_Antiguedad = x.Promedio_Antiguedad,
                Total = x.Total
            }).ToList();
            return View(results);
        }



        //Ver el estado y responsables de un requerimiento.
        public ActionResult stateResponsableRequirement()
        {
            return View();
        }

        //Ver el total de requerimientos de un proyecto.
        public ActionResult totalRequirements([Bind(Include = "cliente")]string cliente, [Bind(Include = "Proyecto")]string proyecto)
        {
            if (Convert.ToInt32(Session["userRole"]) == 0)
            { //si es jefe
                var query = from r in db.Requeriment
                            join e in db.Employee on r.idEmployeeFK equals e.idEmployeePK
                            join p in db.Project on r.idProjectFKPK equals p.idProjectPK
                            join c in db.Client on p.idClientFK equals c.idClientPK
                            where c.clientName.ToLower().Contains(cliente.ToLower()) || c.clientLastName.ToLower().Contains(cliente.ToLower()) 
                            group new { r, e, p, c } by new { p.projectName, c.clientName, c.clientLastName, p.finishingDate } into rGroup
                            select new
                            {
                                Nombre_Proyecto = rGroup.Key.projectName,
                                Nombre_Cliente = rGroup.Key.clientName,
                                Apellido_Cliente = rGroup.Key.clientLastName,
                                Terminados = rGroup.Count(x => x.r.status == 3),
                                Proceso = rGroup.Count(x => x.r.status != 3),
                                Fecha_Finalizacion = rGroup.Key.finishingDate
                            };
                var results = query.ToList().Select(x => new GetFinished_Reqs_Mapped
                {
                    Nombre_Proyecto = x.Nombre_Proyecto,
                    Nombre_Cliente = x.Nombre_Cliente,
                    Apellido_Cliente = x.Apellido_Cliente,
                    Requerimientos_Terminados = x.Terminados,
                    Requerimientos_En_Proceso = x.Proceso,
                    Fecha_Finalizacion = x.Fecha_Finalizacion
                }).ToList();
                return View(results);
            }
            else if (Convert.ToInt32(Session["userRole"]) == 3) // si es cliente
            {
                if(proyecto == "")
                {
                    var actualUsr = Session["userID"].ToString();
                    var query = from r in db.Requeriment
                                join e in db.Employee on r.idEmployeeFK equals e.idEmployeePK
                                join p in db.Project on r.idProjectFKPK equals p.idProjectPK
                                join c in db.Client on p.idClientFK equals c.idClientPK
                                where c.idClientPK == actualUsr
                                group new { r, e, p, c } by new { p.projectName, c.clientName, c.clientLastName, p.finishingDate } into rGroup
                                select new
                                {
                                    Nombre_Proyecto = rGroup.Key.projectName,
                                    Nombre_Cliente = rGroup.Key.clientName,
                                    Apellido_Cliente = rGroup.Key.clientLastName,
                                    Terminados = rGroup.Count(x => x.r.status == 3),
                                    Proceso = rGroup.Count(x => x.r.status != 3),
                                    Fecha_Finalizacion = rGroup.Key.finishingDate
                                };
                    var results = query.ToList().Select(x => new GetFinished_Reqs_Mapped
                    {
                        Nombre_Proyecto = x.Nombre_Proyecto,
                        Nombre_Cliente = x.Nombre_Cliente,
                        Apellido_Cliente = x.Apellido_Cliente,
                        Requerimientos_Terminados = x.Terminados,
                        Requerimientos_En_Proceso = x.Proceso,
                        Fecha_Finalizacion = x.Fecha_Finalizacion
                    }).ToList();
                    return View(results);
                }
                else
                {
                    var actualUsr = Session["userID"].ToString();
                    var query = from r in db.Requeriment
                                join e in db.Employee on r.idEmployeeFK equals e.idEmployeePK
                                join p in db.Project on r.idProjectFKPK equals p.idProjectPK
                                join c in db.Client on p.idClientFK equals c.idClientPK
                                where c.idClientPK == actualUsr && p.projectName.ToLower().Contains(proyecto.ToLower())
                                group new { r, e, p, c } by new { p.projectName, c.clientName, c.clientLastName, p.finishingDate } into rGroup
                                select new
                                {
                                    Nombre_Proyecto = rGroup.Key.projectName,
                                    Nombre_Cliente = rGroup.Key.clientName,
                                    Apellido_Cliente = rGroup.Key.clientLastName,
                                    Terminados = rGroup.Count(x => x.r.status == 3),
                                    Proceso = rGroup.Count(x => x.r.status != 3),
                                    Fecha_Finalizacion = rGroup.Key.finishingDate
                                };
                    var results = query.ToList().Select(x => new GetFinished_Reqs_Mapped
                    {
                        Nombre_Proyecto = x.Nombre_Proyecto,
                        Nombre_Cliente = x.Nombre_Cliente,
                        Apellido_Cliente = x.Apellido_Cliente,
                        Requerimientos_Terminados = x.Terminados,
                        Requerimientos_En_Proceso = x.Proceso,
                        Fecha_Finalizacion = x.Fecha_Finalizacion
                    }).ToList();
                    return View(results);
                }
            }
            return View();
        }
    }
}