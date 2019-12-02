using System;
using System.Collections.Generic;
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


        //Cantidad de requerimientos por desarrollador para un proyecto específico  (LuisC)
        public ActionResult numReqPerDev()
        {
           // var query

            return View();
        }

        //Comparar la duración estimada y real para requerimiento de un desarrollador.
        public ActionResult estimatedRealTimeDev([Bind(Include = "Proyecto")]string proyecto/*string idEmployeePK, *//*int? idProject*/)//------------------------------se usa
        {
            var actualUsr = Session["userID"]; //Desarrollador
            var rDeveloper = actualUsr.ToString();
            if (Convert.ToInt32(Session["userRole"]) == 1 || Convert.ToInt32(Session["userRole"]) == 2 && proyecto == null) //caso jefe desarrollador
            {
                var query2 =
                from p in db.Project
                join m in db.Module on p.idProjectPK equals m.idProjectFKPK
                join r in db.Requeriment on m.idModulePK equals r.idModuleFKPK
                join e in db.Employee on r.idEmployeeFK equals e.idEmployeePK
                where r.status == 3
                && r.idProjectFKPK == p.idProjectPK
                && r.idEmployeeFK == e.idEmployeePK
                && e.idEmployeePK == rDeveloper
                select new
                {
                    DesarrolladorNombre = e.employeeName + " " + e.employeeLastName,
                    Proyecto = p.projectName,
                    Requerimiento = r.objective,
                    Complejidad = r.complexity,
                    DuracionEstimada = r.estimatedDuration,
                    DuracionReal = r.realDuration,
                    Diferencia = ((int)r.estimatedDuration - (int)r.realDuration)
                };

                var results = query2.ToList().Select(r => new estimatedRealTimeDevModel
                {
                    DesarrolladorNombre = r.DesarrolladorNombre,
                    Proyecto = r.Proyecto,
                    Requerimiento = r.Requerimiento,
                    Complejidad = r.Complejidad,
                    DuracionEstimada = r.DuracionEstimada,
                    DuracionReal = r.DuracionReal,
                    Diferencia = r.Diferencia
                }).ToList();
                return PartialView("estimatedRealTimeDev", results);
            }
            else if (Convert.ToInt32(Session["userRole"]) == 0 && proyecto == null) //Si es un jefe desarrollador
            {
                var query2 =
                from p in db.Project
                join m in db.Module on p.idProjectPK equals m.idProjectFKPK
                join r in db.Requeriment on m.idModulePK equals r.idModuleFKPK
                join e in db.Employee on r.idEmployeeFK equals e.idEmployeePK
                where r.status == 3
                && r.idProjectFKPK == p.idProjectPK
                && r.idEmployeeFK == e.idEmployeePK
                select new
                {
                    DesarrolladorNombre = e.employeeName + " " + e.employeeLastName,
                    Proyecto = p.projectName,
                    Requerimiento = r.objective,
                    Complejidad = r.complexity,
                    DuracionEstimada = r.estimatedDuration,
                    DuracionReal = r.realDuration,
                    Diferencia = ((int)r.estimatedDuration - (int)r.realDuration)
                };

                var results = query2.ToList().Select(r => new estimatedRealTimeDevModel
                {
                    DesarrolladorNombre = r.DesarrolladorNombre,
                    Proyecto = r.Proyecto,
                    Requerimiento = r.Requerimiento,
                    Complejidad = r.Complejidad,
                    DuracionEstimada = r.DuracionEstimada,
                    DuracionReal = r.DuracionReal,
                    Diferencia = r.Diferencia
                }).ToList();
                return PartialView("estimatedRealTimeDev", results);
            }
            else
            {
                var query2 =
                from p in db.Project
                join m in db.Module on p.idProjectPK equals m.idProjectFKPK
                join r in db.Requeriment on m.idModulePK equals r.idModuleFKPK
                join e in db.Employee on r.idEmployeeFK equals e.idEmployeePK
                where r.status == 3
                && r.idProjectFKPK == p.idProjectPK
                && r.idEmployeeFK == e.idEmployeePK
                && p.projectName.ToLower().Contains(proyecto.ToLower())
                select new
                {
                    DesarrolladorNombre = e.employeeName + " " + e.employeeLastName,
                    Proyecto = p.projectName,
                    Requerimiento = r.objective,
                    Complejidad = r.complexity,
                    DuracionEstimada = r.estimatedDuration,
                    DuracionReal = r.realDuration,
                    Diferencia = ((int)r.estimatedDuration - (int)r.realDuration)
                };

                var results = query2.ToList().Select(r => new estimatedRealTimeDevModel
                {
                    DesarrolladorNombre = r.DesarrolladorNombre,
                    Proyecto = r.Proyecto,
                    Requerimiento = r.Requerimiento,
                    Complejidad = r.Complejidad,
                    DuracionEstimada = r.DuracionEstimada,
                    DuracionReal = r.DuracionReal,
                    Diferencia = r.Diferencia
                }).ToList();
                return PartialView("estimatedRealTimeDev", results);
            }
        }

        //Total de horas estimadas y reales requeridas por un proyecto.
        public ActionResult totalHours([Bind(Include = "Proyecto")]string proyecto)//------------------se ocupa
        {
            if (Convert.ToInt32(Session["userRole"]) == 1 || Convert.ToInt32(Session["userRole"]) == 2 && proyecto == null) //caso jefe desarrollador
            {
                var actualUsr = Session["userID"];
                var idLeader = actualUsr.ToString();

                var query2 =
                    (from p in db.Project
                     join m in db.Module on p.idProjectPK equals m.idProjectFKPK
                     join r in db.Requeriment on m.idModulePK equals r.idModuleFKPK
                     join w in db.WorksIn on p.idProjectPK equals w.idProjectFKPK
                     join e in db.Employee on w.idEmployeeFKPK equals e.idEmployeePK
                     where p.status == 3
                           && r.idProjectFKPK == p.idProjectPK
                           && w.idEmployeeFKPK == idLeader
                           && w.role == 1
                     select new
                     {
                         LiderNombre = e.employeeName + " " + e.employeeLastName,
                         NombreProyecto = p.projectName,
                         HorasEstimadas = p.estimatedDuration,
                         HorasReales = p.realDuration,
                         Diferencia = ((int)(p.estimatedDuration) - (int)(p.realDuration))
                     }).GroupBy(q => new { q.LiderNombre, q.NombreProyecto, q.HorasEstimadas, q.HorasReales, q.Diferencia });

                //Nota: COmo se genera este modelo
                var results = query2.ToList().Select(r => new totalHoursModel
                {
                    LiderNombre = r.Key.LiderNombre,
                    NombreProyecto = r.Key.NombreProyecto,
                    HorasEstimadas = r.Key.HorasEstimadas,
                    HorasReales = r.Key.HorasReales,
                    Diferencia = r.Key.Diferencia
                }).ToList();
                return PartialView("totalHours", results);
            }
            else if (Convert.ToInt32(Session["userRole"]) == 0 && proyecto == null)
            {

                var query2 =
                    (from p in db.Project
                     join m in db.Module on p.idProjectPK equals m.idProjectFKPK
                     join r in db.Requeriment on m.idModulePK equals r.idModuleFKPK
                     join w in db.WorksIn on p.idProjectPK equals w.idProjectFKPK
                     join e in db.Employee on w.idEmployeeFKPK equals e.idEmployeePK
                     where p.status == 3
                           && r.idProjectFKPK == p.idProjectPK
                           && w.role == 1
                     select new
                     {
                         LiderNombre = e.employeeName + " " + e.employeeLastName,
                         NombreProyecto = p.projectName,
                         HorasEstimadas = p.estimatedDuration,
                         HorasReales = p.realDuration,
                         Diferencia = ((int)(p.estimatedDuration) - (int)(p.realDuration))
                     }).GroupBy(q => new { q.LiderNombre, q.NombreProyecto, q.HorasEstimadas, q.HorasReales, q.Diferencia });

                var results = query2.ToList().Select(r => new totalHoursModel
                {
                    LiderNombre = r.Key.LiderNombre,
                    NombreProyecto = r.Key.NombreProyecto,
                    HorasEstimadas = r.Key.HorasEstimadas,
                    HorasReales = r.Key.HorasReales,
                    Diferencia = r.Key.Diferencia
                }).ToList();
                return PartialView("totalHours", results);
            }
            else 
            {
                var query2 =
                    (from p in db.Project
                     join m in db.Module on p.idProjectPK equals m.idProjectFKPK
                     join r in db.Requeriment on m.idModulePK equals r.idModuleFKPK
                     join w in db.WorksIn on p.idProjectPK equals w.idProjectFKPK
                     join e in db.Employee on w.idEmployeeFKPK equals e.idEmployeePK
                     where p.status == 3
                           && r.idProjectFKPK == p.idProjectPK
                           && w.role == 1
                           && p.projectName.ToLower().Contains(proyecto.ToLower())
                     select new
                     {
                         LiderNombre = e.employeeName + " " + e.employeeLastName,
                         NombreProyecto = p.projectName,
                         HorasEstimadas = p.estimatedDuration,
                         HorasReales = p.realDuration,
                         Diferencia = ((int)(p.estimatedDuration) - (int)(p.realDuration))
                     }).GroupBy(q => new { q.LiderNombre, q.NombreProyecto, q.HorasEstimadas, q.HorasReales, q.Diferencia });

                var results = query2.ToList().Select(r => new totalHoursModel
                {
                    LiderNombre = r.Key.LiderNombre,
                    NombreProyecto = r.Key.NombreProyecto,
                    HorasEstimadas = r.Key.HorasEstimadas,
                    HorasReales = r.Key.HorasReales,
                    Diferencia = r.Key.Diferencia
                }).ToList();
                return PartialView("totalHours", results);
            }

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



        //Ver el estado y responsables de un requerimiento. Según un cliente
        public ActionResult stateResponsableRequirement()
        {
            //Como se usa una PartialView, la consulta esá abajo
            return View();
        }
        //EFE: Realiza la consulta de: Ver el estado y responsables de un requerimiento. Según un cliente
        //REQ: Id del cliente que se obtiene de la variable de session y el IdProject que se obtiene del dropdown
        public PartialViewResult stateResponsableReqClient(string idClientPK, int? idProject)
        {
            var client = idClientPK;
            var rClient = client.ToString();

            var query =
                from c in db.Client
                join p in db.Project on c.idClientPK equals p.idClientFK
                join m in db.Module on p.idProjectPK equals m.idProjectFKPK
                join r in db.Requeriment on m.idModulePK equals r.idModuleFKPK
                join e in db.Employee on r.idEmployeeFK equals e.idEmployeePK
                where r.idProjectFKPK == p.idProjectPK
                && c.idClientPK == idClientPK
                && p.idProjectPK == idProject
                select new
                {
                    Nombre = r.objective,
                    Estado = r.status,
                    Responable = e.employeeName
                };
            var query2 =
                from c in db.Client
                join p in db.Project on c.idClientPK equals p.idClientFK
                join wi in db.WorksIn on p.idProjectPK equals wi.idProjectFKPK
                join e in db.Employee on wi.idEmployeeFKPK equals e.idEmployeePK
                join r in db.Requeriment on p.idProjectPK equals r.idProjectFKPK
                where c.idClientPK == rClient
                && p.idProjectPK == idProject
                select new
                {
                    Nombre = r.objective,
                    Estado = r.status,
                    Responable = e.employeeName
                };

            var results = query.ToList().Select(r => new StateResponsableReqClient_Result_Mapped
            {
                Nombre = r.Nombre,
                Estado = r.Estado,
                Responsable = r.Responable
            }).ToList();
            return PartialView("stateResponsableReqClient", results);
        }


        //EFE: Crea una lista con los proyectos de un cliente
        public SelectList ProjectsList()
        {
            //var xd = db.Project.Include(p => p.Client);
            var actualUsr = Session["userID"];
            var query = from a in db.Project
                        where a.idClientFK == actualUsr.ToString()
                        select a.projectName;
            return new SelectList(query.ToList());
        }

        //Ver el total de requerimientos de un proyecto.
        public ActionResult totalRequirements()
        {
            return View();
        }
    }
}