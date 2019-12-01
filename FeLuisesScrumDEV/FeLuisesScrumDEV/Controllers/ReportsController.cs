using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeLuisesScrumDEV.Models;
using System.Data.Entity.SqlServer;
using System.Data.Entity;
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
            ViewBag.freeEmployees = new SelectList(db.Employee.Where(e => e.availability == 0), "employeeName", "employeeLastName");
            if (Convert.ToInt32(Session["userRole"]) == 0)
            {
                var query = (from E in db.Employee
                             join W in db.WorksIn on E.idEmployeePK equals W.idEmployeeFKPK
                             join P in db.Project on W.idProjectFKPK equals P.idProjectPK
                             join R in db.Requeriment on P.idProjectPK equals R.idProjectFKPK
                             where W.role == 1
                             select new
                             {
                                 Nombre_Desarrollador = E.employeeName + " " + E.employeeLastName,
                                 Nombre_Proyecto = P.projectName,
                                 Nombre_Requerimiento = R.objective,
                                 Fecha_Inicio = P.startingDate,
                                 Fecha_EstimadaFin = DbFunctions.AddDays(P.startingDate, R.estimatedDuration / 8)
                             }).GroupBy(q => new { q.Nombre_Desarrollador, q.Nombre_Proyecto, q.Nombre_Requerimiento, q.Fecha_Inicio, q.Fecha_EstimadaFin })
                             .OrderByDescending(q => q.Key.Fecha_EstimadaFin).ToList();

                var EmployeesInProyects = query.ToList().Select(e => new GetDevs
                {
                    Nombre_Desarrollador = e.Key.Nombre_Desarrollador,
                    Nombre_Proyecto = e.Key.Nombre_Proyecto,
                    Nombre_Requerimiento = e.Key.Nombre_Requerimiento,
                    Fecha_Inicio = e.Key.Fecha_Inicio.Value.Date.ToShortDateString(),
                    Fecha_EstimadaFin = e.Key.Fecha_EstimadaFin.Value.Date.ToShortDateString()
                });
                return View(EmployeesInProyects);
            }else{ // if (Convert.ToInt32(Session["userRole"]) == 2)
                string id = Session["userID"].ToString();
                var query = (from E in db.Employee
                             join W in db.WorksIn on E.idEmployeePK equals W.idEmployeeFKPK
                             join P in db.Project on W.idProjectFKPK equals P.idProjectPK
                             join R in db.Requeriment on P.idProjectPK equals R.idProjectFKPK
                             where W.role == 1 && P.idProjectPK == W.idProjectFKPK && E.idEmployeePK == W.idEmployeeFKPK && E.idEmployeePK == id
                             select new
                             {
                                 Nombre_Desarrollador = E.employeeName + " " + E.employeeLastName,
                                 Nombre_Proyecto = P.projectName,
                                 Nombre_Requerimiento = R.objective,
                                 Fecha_Inicio = P.startingDate,
                                 Fecha_EstimadaFin = DbFunctions.AddDays(P.startingDate, R.estimatedDuration / 8)
                             }).GroupBy(q => new { q.Nombre_Desarrollador, q.Nombre_Proyecto, q.Nombre_Requerimiento, q.Fecha_Inicio, q.Fecha_EstimadaFin })
                             .OrderByDescending(q => q.Key.Fecha_EstimadaFin).ToList();

                var EmployeesInProyects = query.ToList().Select(e => new GetDevs
                {
                    Nombre_Desarrollador = e.Key.Nombre_Desarrollador,
                    Nombre_Proyecto = e.Key.Nombre_Proyecto,
                    Nombre_Requerimiento = e.Key.Nombre_Requerimiento,
                    Fecha_Inicio = e.Key.Fecha_Inicio.Value.Date.ToShortDateString(),
                    Fecha_EstimadaFin = e.Key.Fecha_EstimadaFin.Value.Date.ToShortDateString()
                });
                return View(EmployeesInProyects);
            }

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
        public ActionResult paticipationHistory()
        {
            return View();
        }

        //Ver el estado de avance de un requerimiento.
        public ActionResult requirmentState()
        {
            return View();
        }

        //Comparar las duraciones reales y estimadas entre requerimientos de un mismo nivel de complejidad.
        public ActionResult compareDuration()
        {
            return View();
        }


        //Cantidad de desarrolladores con conocimientos específicos y promedio de antigüedad laboral.
        public ActionResult devsInKnownledgeAndAtiguedad()
        {
            return View();
        }



        //Ver el estado y responsables de un requerimiento.
        public ActionResult stateResponsableRequirement()
        {
            return View();
        }

        //Ver el total de requerimientos de un proyecto.
        public ActionResult totalRequirements()
        {
            return View();
        }








    }
}