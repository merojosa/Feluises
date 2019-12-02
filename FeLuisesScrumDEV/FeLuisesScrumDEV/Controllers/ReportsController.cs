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


        //Cantidad de requerimientos por desarrollador para un proyecto específico
        public ActionResult numReqPerDev()
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

        //Ver el estado de avance de un requerimiento. Quizá esta /*/*/*/*/*/*/*/*/*/**/*/
        public ActionResult requirmentState()
        {
            var dev = Session["userID"].ToString();
           

            var myProject = from p in db.Project
                            join wi in db.WorksIn on p.idProjectPK equals wi.idProjectFKPK
                            join e in db.Employee on wi.idEmployeeFKPK equals e.idEmployeePK
                            where wi.idEmployeeFKPK == dev
                            select p.idProjectPK; //Saca el proyecto del líder

            var gen = from p in db.Project
                    join wi in db.WorksIn on p.idProjectPK equals wi.idProjectFKPK
                    join e in db.Employee on wi.idEmployeeFKPK equals e.idEmployeePK
                    where wi.idEmployeeFKPK == dev
                    select p.projectName; //Saca el nombre

            ViewBag.general = gen.FirstOrDefault();

            var query = //La consulta :v 
                from p in db.Project
                join m in db.Module on p.idProjectPK equals m.idProjectFKPK
                join r in db.Requeriment on m.idModulePK equals r.idModuleFKPK
                join e in db.Employee on r.idEmployeeFK equals e.idEmployeePK
                where r.idProjectFKPK == p.idProjectPK
                && p.idProjectPK == myProject.FirstOrDefault()
                && e.idEmployeePK == dev
                select new
                {
                    Nombre = r.objective,
                    Fecha_de_inicio = r.startingDate,
                    Fecha_de_finalizacion = r.endDate,
                    Duracion = r.realDuration,
                    Estado = r.status,
                    Nombre_Proyecto = p.projectName
                };

            var results = query.ToList().Select(r => new requirmentStatus_Result_Mapped
            {
                Nombre_Requerimiento = r.Nombre,
                Fecha_de_inicio = r.Fecha_de_inicio,
                Fecha_de_Finalizacion = r.Fecha_de_finalizacion,
                Duracion_Estimada = r.Duracion,
                Estado = r.Estado,
                Nombre_Proyecto = r.Nombre_Proyecto
            });

            return View(results);
        }
        //EFE: Retorna una lista con los empleados del equipo
        public SelectList EmployeeList()
        {
            var idLeader = Session["userID"].ToString();

            var project = from p in db.Project
                          join wi in db.WorksIn on p.idProjectPK equals wi.idProjectFKPK
                          join e in db.Employee on wi.idEmployeeFKPK equals e.idEmployeePK
                          where wi.role == 1
                          && wi.idEmployeeFKPK == idLeader
                          select p.idProjectPK; //Saca el proyecto del líder

            var query = from p in db.Project
                        join wi in db.WorksIn on p.idProjectPK equals wi.idProjectFKPK
                        join e in db.Employee on wi.idEmployeeFKPK equals e.idEmployeePK
                        where project.FirstOrDefault() == p.idProjectPK
                        select new
                        {
                            e.idEmployeePK,
                            e.employeeName
                        };
            var list = query.ToList();
            List<SelectListItem> dropdown = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "Todos" },
            };
            foreach (var item in list)
            {
                dropdown.Add(new SelectListItem { Value = item.idEmployeePK.ToString(), Text = item.employeeName });
            }
            return new SelectList(dropdown, "Value", "Text");
        }

        //Vista parcial en el caso de ser un leader el que ingresa al reporte
        public PartialViewResult requirmentStatusLeader(int? devID)
        {
            var project = from p in db.Project
                          join wi in db.WorksIn on p.idProjectPK equals wi.idProjectFKPK
                          join e in db.Employee on wi.idEmployeeFKPK equals e.idEmployeePK
                          where wi.role == 0
                          && wi.idEmployeeFKPK == devID.ToString()
                          select p.idProjectPK; //Saca el proyecto del desarrollador

            if (devID == 0)
            {
                var user = Session["userID"].ToString();
                var projectN = from p in db.Project
                               join wi in db.WorksIn on p.idProjectPK equals wi.idProjectFKPK
                               join e in db.Employee on wi.idEmployeeFKPK equals e.idEmployeePK
                               where wi.role == 1
                               && wi.idEmployeeFKPK == user
                               select p.idProjectPK; //Saca el proyecto del desarrollador

                ViewBag.filtro = 1; //Sin dev específico
                var query = //La consulta :v 
                from p in db.Project
                join m in db.Module on p.idProjectPK equals m.idProjectFKPK
                join r in db.Requeriment on m.idModulePK equals r.idModuleFKPK
                join e in db.Employee on r.idEmployeeFK equals e.idEmployeePK
                where r.idProjectFKPK == p.idProjectPK
                && p.idProjectPK == projectN.FirstOrDefault()
                select new
                {
                    Nombre = r.objective,
                    Fecha_de_inicio = r.startingDate,
                    Fecha_de_finalizacion = r.endDate,
                    Duracion = r.realDuration,
                    Estado = r.status,
                    Nombre_Proyecto = p.projectName,
                    Nombre_Desarrollador = e.employeeName
                };
                var results = query.ToList().Select(r => new requirmentStatus_Result_Mapped
                {
                    Nombre_Requerimiento = r.Nombre,
                    Fecha_de_inicio = r.Fecha_de_inicio,
                    Fecha_de_Finalizacion = r.Fecha_de_finalizacion,
                    Duracion_Estimada = r.Duracion,
                    Estado = r.Estado,
                    Nombre_Proyecto = r.Nombre_Proyecto,
                    Nombre_Desarrollador = r.Nombre_Desarrollador
                });
                return PartialView("requirmentStatusLeader", results);
            }
            else
            {
                ViewBag.filtro = 0; // Con dev específico
                var query = //La consulta :v 
                        from p in db.Project
                        join m in db.Module on p.idProjectPK equals m.idProjectFKPK
                        join r in db.Requeriment on m.idModulePK equals r.idModuleFKPK
                        join e in db.Employee on r.idEmployeeFK equals e.idEmployeePK
                        where r.idProjectFKPK == p.idProjectPK
                        && p.idProjectPK == project.FirstOrDefault()
                        && e.idEmployeePK == devID.ToString()
                        select new
                        {
                            Nombre = r.objective,
                            Fecha_de_inicio = r.startingDate,
                            Fecha_de_finalizacion = r.endDate,
                            Duracion = r.realDuration,
                            Estado = r.status,
                            Nombre_Proyecto = p.projectName
                        };
                var results = query.ToList().Select(r => new requirmentStatus_Result_Mapped
                {
                    Nombre_Requerimiento = r.Nombre,
                    Fecha_de_inicio = r.Fecha_de_inicio,
                    Fecha_de_Finalizacion = r.Fecha_de_finalizacion,
                    Duracion_Estimada = r.Duracion,
                    Estado = r.Estado,
                    Nombre_Proyecto = r.Nombre_Proyecto
                });
                return PartialView("requirmentStatusLeader", results);
            }
        }
        //EFE: Realiza la consulta desde el Jefe Desarrollador
        public PartialViewResult requirmentStatusBoss(int? projectID)
        {

            var query = //La consulta :v 
                from p in db.Project
                join m in db.Module on p.idProjectPK equals m.idProjectFKPK
                join r in db.Requeriment on m.idModulePK equals r.idModuleFKPK
                join e in db.Employee on r.idEmployeeFK equals e.idEmployeePK
                where r.idProjectFKPK == p.idProjectPK
                && p.idProjectPK == projectID
                select new
                {
                    Nombre = r.objective,
                    Fecha_de_inicio = r.startingDate,
                    Fecha_de_finalizacion = r.endDate,
                    Duracion = r.realDuration,
                    Estado = r.status,
                    Nombre_Desarrollador = e.employeeName
                };

            var results = query.ToList().Select(r => new requirmentStatus_Result_Mapped
            {
                Nombre_Requerimiento = r.Nombre,
                Fecha_de_inicio = r.Fecha_de_inicio,
                Fecha_de_Finalizacion = r.Fecha_de_finalizacion,
                Duracion_Estimada = r.Duracion,
                Estado = r.Estado,
                Nombre_Desarrollador = r.Nombre_Desarrollador
            });
            return PartialView("requirmentStatusBoss", results);
        }

        //Ver el estado de avance de un requerimiento. Quizá esta /*/*/*/*/*/*/*/*/*/**/*/

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
        public PartialViewResult stateResponsableReqClient(int? idProject)
        {

            var rClient = Session["userID"].ToString();
            var query =
                from c in db.Client
                join p in db.Project on c.idClientPK equals p.idClientFK
                join m in db.Module on p.idProjectPK equals m.idProjectFKPK
                join r in db.Requeriment on m.idModulePK equals r.idModuleFKPK
                join e in db.Employee on r.idEmployeeFK equals e.idEmployeePK
                where r.idProjectFKPK == p.idProjectPK
                && c.idClientPK == rClient
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


        //EFE: Crea una lista con los proyectos de un cliente o jefe desarrollador
        public SelectList ProjectsList(int rol)
        {
            var actualUsr = Session["userID"];
            if (rol == 3) //Cliente
            {
                var query = from a in db.Project
                            where a.idClientFK == actualUsr.ToString()
                            select new
                            {
                                a.idProjectPK,
                                a.projectName
                            };
                var list = query.ToList();

                List<SelectListItem> dropdown = new List<SelectListItem> { };
                foreach (var item in list)
                {
                    dropdown.Add(new SelectListItem { Value = item.idProjectPK.ToString(), Text = item.projectName });
                }
                return new SelectList(dropdown, "Value", "Text");
            }
            else if (rol == 0) // Jefe
            {
                var query = from a in db.Project
                            select new
                            {
                                a.idProjectPK,
                                a.projectName
                            };
                var list = query.ToList();

                List<SelectListItem> dropdown = new List<SelectListItem> { };
                foreach (var item in list)
                {
                    dropdown.Add(new SelectListItem { Value = item.idProjectPK.ToString(), Text = item.projectName });
                }
                return new SelectList(dropdown, "Value", "Text");
            }

            return null;
        }

        //Ver el total de requerimientos de un proyecto.
        public ActionResult totalRequirements()
        {
            return View();
        }

    }
}