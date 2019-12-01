using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        // Esta consulta va a sacar todos los requerimientos y agruparlos en cuanto a complejidad para así
        // poder calcular datos estadisticos tales como el total de requerimientos asignados a dicha complejidad, la minima diferencia entre estimación y duración real,
        // maxima diferencia, promedio de dicha diferencia y promedio de la estimación y la duración real
        public ActionResult CompareReqStatsbyComplexity()
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
            // Se realiza la transición de variables anonimas a viewModels para poder pasar a la vista de manera correcta
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

        public ActionResult DevsKnowledgeLaborSeniority()
        {
            return View();
        }

        //Cantidad de desarrolladores con conocimientos específicos y promedio de antigüedad laboral.
        // Se procede en esta consulta a dado uno o varios conocimientos calcular la cantidad de empleados que poseen dicho conocimiento
        // Y el promedio de la antiguedad laboral
        public PartialViewResult viewKnowledges(string knowledge, int? mode)
        {
            if(string.IsNullOrWhiteSpace(knowledge))
                return PartialView("viewKnowledges", null);
            var now = DateTime.Now;
            var basequery = db.DeveloperKnowledge.Include(c => c.idEmployeeFKPK);
            if (mode == null)
            {
                // En caso de que no se especifique el modo se toma como que se estan pidiendo todos los conocimientos disponibles
                // No se logro separar la query en segmentos por lo que tuvo que replicarse innecesariamente el código 
                var query = from dk in basequery
                            group dk by dk.devKnowledgePK into dkGroup
                            orderby dkGroup.Key descending
                            select new
                            {
                                Conocimiento = dkGroup.Key,
                                Total = dkGroup.Count(),
                                Promedio_Antiguedad = (int)dkGroup.Average(x => DbFunctions.DiffDays(x.Employee.employeeHireDate, now))
                            };
                // Se utilizan variables anonimas para pasar de los resultados de las consultas a los viewModel con el fin de poder pasarlos a las vistas
                var results = query.ToList().Select(x => new GetKnowledgesSP_Result_Mapped
                {
                    Conocimiento = x.Conocimiento,
                    Promedio_Antiguedad = x.Promedio_Antiguedad,
                    Total = x.Total
                }).ToList();
                return PartialView("viewKnowledges", results);
            }
            else
            {
                // si se especifica cualquier cosa en el modo se toma en cuenta como que se pregunta por unos conocimientos en especifico
                var query = from dk in basequery
                            where dk.devKnowledgePK.ToLower().Contains(knowledge.ToLower())
                            group dk by dk.devKnowledgePK into dkGroup
                            orderby dkGroup.Key descending
                            select new
                            {
                                Conocimiento = dkGroup.Key,
                                Total = dkGroup.Count(),
                                Promedio_Antiguedad = (int)dkGroup.Average(x => DbFunctions.DiffDays(x.Employee.employeeHireDate, now))//(long)dkGroup.Average(x => DateTime.Now.Ticks-(x.Employee.employeeHireDate).Ticks)
                            };
                // se realiza la transición para llenar los viewModel
                var results = query.ToList().Select(x => new GetKnowledgesSP_Result_Mapped
                {
                    Conocimiento = x.Conocimiento,
                    Promedio_Antiguedad = x.Promedio_Antiguedad,
                    Total = x.Total
                }).ToList();
                return PartialView("viewKnowledges", results);
            }
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