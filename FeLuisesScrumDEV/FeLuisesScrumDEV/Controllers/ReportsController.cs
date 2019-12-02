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

        //EFE: Muestra el reporte de ver todos los desarrolladores disponibles y ocupadodos, estos ultimos asociados a un proyecto
        public ActionResult DeveloperState()
        {
            ViewBag.freeEmployees = new SelectList(db.Employee.Where(e => e.availability == 0), "employeeName", "employeeLastName"); //Empleados disponibles
            if (Convert.ToInt32(Session["userRole"]) == 0) //caso jefe desarrollador
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
            }else{ //caso lider
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
        public ActionResult numReqPerDev()
        {


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
        public ActionResult devsHistoryOnProjects([Bind(Include = "employee")]string employee)
        {
            var query = from wi in db.WorksIn
                        join p in db.Project on wi.idProjectFKPK equals p.idProjectPK
                        join e in db.Employee on wi.idEmployeeFKPK equals e.idEmployeePK
                        join r in db.Requeriment on e.idEmployeePK equals r.idEmployeeFK
                        where r.idProjectFKPK == p.idProjectPK && (e.employeeName.ToLower().Contains(employee.ToLower()) || e.employeeLastName.ToLower().Contains(employee.ToLower()))
                        group new { wi, p, e, r } by new { e.employeeName, e.employeeLastName, p.projectName, wi.role } into wiGroup
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
            var leaderÌD = from wi in db.WorksIn
                         where wi.idProjectFKPK == projectID
                         && wi.role == 1
                         select wi.idEmployeeFKPK;
            var leader = from e in db.Employee
                         where e.idEmployeePK == leaderÌD.FirstOrDefault()
                         select e.employeeName;
            ViewBag.leader = leader.FirstOrDefault();

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



        //Ver el estado y responsables de un requerimiento. Según un cliente
        public ActionResult stateResponsableRequirement()
        {
            var usr = Session["userID"].ToString();
            var client = from c in db.Client
                         where usr == c.idClientPK
                         select c.clientName;

            ViewBag.clientName = client.FirstOrDefault();
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
                    Nombre = r.objective, //para commit
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
        public ActionResult totalRequirements([Bind(Include = "cliente")]string cliente, [Bind(Include = "Proyecto")]string proyecto)
        {
            if (Convert.ToInt32(Session["userRole"]) == 0)
            { //si es jefe desarrollador
                var query = from r in db.Requeriment //Desde Requerimiento
                            join e in db.Employee on r.idEmployeeFK equals e.idEmployeePK //Empleado
                            join p in db.Project on r.idProjectFKPK equals p.idProjectPK //Proyecto
                            join c in db.Client on p.idClientFK equals c.idClientPK //Y cliente
                            where c.clientName.ToLower().Contains(cliente.ToLower()) || c.clientLastName.ToLower().Contains(cliente.ToLower()) //Donde el nombre del cliente es similar al buscado
                            group new { r, e, p, c } by new { p.projectName, c.clientName, c.clientLastName, p.finishingDate } into rGroup
                            select new //Selecciona los atributos
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
                if (proyecto == "") //Y no escribió nada en el buscador
                {
                    var actualUsr = Session["userID"].ToString();
                    var query = from r in db.Requeriment
                                join e in db.Employee on r.idEmployeeFK equals e.idEmployeePK
                                join p in db.Project on r.idProjectFKPK equals p.idProjectPK
                                join c in db.Client on p.idClientFK equals c.idClientPK
                                where c.idClientPK == actualUsr //Recupera los proyectos para el cliente loggeado
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
                { //Si escribió algo en la búsqueda
                    var actualUsr = Session["userID"].ToString();
                    var query = from r in db.Requeriment
                                join e in db.Employee on r.idEmployeeFK equals e.idEmployeePK
                                join p in db.Project on r.idProjectFKPK equals p.idProjectPK
                                join c in db.Client on p.idClientFK equals c.idClientPK
                                where c.idClientPK == actualUsr && p.projectName.ToLower().Contains(proyecto.ToLower()) //busca proyectos similares al nombrado, especificamente para el cliente loggeado.
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