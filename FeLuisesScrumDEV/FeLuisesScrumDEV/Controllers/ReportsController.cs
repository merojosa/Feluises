using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeLuisesScrumDEV.Models;

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


        public IEnumerable<Project> GetProjectsbyClient(string client)
        {
            
            return db.Project.Where(p => p.idClientFK == client).AsEnumerable<Project>();
        }

        //Cantidad de requerimientos por desarrollador para un proyecto específico  (LuisC)
        public ActionResult numReqPerDev()
        {
            var actualUsr = Session["userName"].ToString(); 
            var proyectos = db.Project.Where(p => p.idClientFK== actualUsr);
            var prb = db.Project;
            List<Project> results = db.Project.Where(p => p.idClientFK == actualUsr).ToList();
            //ViewBag.idProjectFKPK = new SelectList(db.Project.Where(p => p.idClientFK == actualUsr), "idProjectPK", "projectName").FirstOrDefault();
            //ViewData["idProjectFKPK"] = new SelectList(db.Project.Where(p => p.idClientFK == actualUsr), "idProjectPK", "projectName").FirstOrDefault();

            var projects = GetProjectsbyClient(actualUsr);
            var caca = new SelectList(projects, "idProjectPK", "projectName");
            ViewBag.idProjectFKPK = caca;


            return View(projects.ToList());
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