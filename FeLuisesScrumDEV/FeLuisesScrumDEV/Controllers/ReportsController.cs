using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeLuisesScrumDEV.Models;
using System.Data.Entity.SqlServer;
//using System.Data.Entity.SqlServerCompact;

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

            ViewBag.EmployeesInProyects = (from E in db.Employee
                                           join W in db.WorksIn on E.idEmployeePK equals W.idEmployeeFKPK
                                           join P in db.Project on W.idProjectFKPK equals P.idProjectPK
                                           join R in db.Requeriment on P.idProjectPK equals R.idProjectFKPK
                                           //group E by new { E.employeeName, E.employeeLastName } into Nombre_Desarrollador
                                           where R.estimatedDuration != null
                                           select new
                                           {
                                               Nombre_Desarrollador = E.employeeName + " " + E.employeeLastName,
                                               Nombre_Proyecto = P.projectName,
                                               Fecha_Inicio = P.startingDate,
                                               Fecha_Estimada = P.startingDate.GetValueOrDefault().AddDays(Convert.ToDouble(R.estimatedDuration / 24))
                                              // Fecha_Estimada = DateAdd("DAY", Convert.ToDouble(R.estimatedDuration / 24))
                                               //Fecha_Estimada = DbFunctions.


                                           }).GroupBy(q => new { q.Nombre_Desarrollador, q.Nombre_Proyecto, q.Fecha_Inicio, q.Fecha_Estimada });


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