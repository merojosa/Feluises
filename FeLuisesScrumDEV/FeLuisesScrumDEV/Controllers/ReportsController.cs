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
            var results = db.GetReqStatsbyComplexity().ToList();
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "No asignado" },
                new SelectListItem { Value = "1", Text = "Simple" },
                new SelectListItem { Value = "2", Text = "Mediano" },
                new SelectListItem { Value = "3", Text = "Complejo" },
                new SelectListItem { Value = "4", Text = "Muy complejo" }
            };
            ViewBag.complexity = list;
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
            var results = db.GetKnowledgesSP(knowledge).ToList();
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