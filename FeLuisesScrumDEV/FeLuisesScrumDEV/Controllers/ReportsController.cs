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
        public ActionResult totalRequirements()
        {
            return View();
        }








    }
}