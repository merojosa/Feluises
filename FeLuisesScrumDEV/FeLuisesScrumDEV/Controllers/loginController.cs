using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FeLuisesScrumDEV.Models;

namespace FeLuisesScrumDEV.Controllers
{
    public class loginController : Controller
    {
        private FeLuisesEntities db = new FeLuisesEntities();

        // GET: login
        //EFE: nada
        public ActionResult Index()
        {
            return View();
        }

        //EFE: Auntentica al usuario y le asigna su respectivo roll
        //REQ: Una usr y pass válido
        [HttpPost]
        public ActionResult Authorize(FeLuisesScrumDEV.Models.Credentials loginModel)
        {
            /*
             * userRole:
             *          0 = Jefe Desarrollador
             *          1 = Desarrollador 
             *          2 = Líder
             *          3 = Cliente
             */

            var masterChiefID = 987654321; // Cambiar si hay otro jefe
            var userDetails = db.Credentials.Where(x => x.userName == loginModel.userName && x.password == loginModel.password).FirstOrDefault();
               

            if (userDetails == null)
            {
                ModelState.AddModelError("", "Cédula o contraseña inválidas");
                return View("Index");
            }else{
                var leaderID1 = 
                    db.WorksIn.Where(e => e.idEmployeeFKPK == loginModel.userName && 
                    db.WorksIn.Any(w => w.idEmployeeFKPK == e.idEmployeeFKPK && w.idProjectFKPK == e.idProjectFKPK && e.role == 1)).FirstOrDefault();
                Session["userID"] = userDetails.userName; // esto pues as'i se llama en tabla

                Boolean isEmployee = false;
                Boolean isChief = false;

                //Sea cliente o empleado, hay que sacar su rol e id
                var Employee = db.Employee.Find(userDetails.userName); // Empleado
                var masterChief = db.Credentials.Find(userDetails.userName); // solo el id

                if (Convert.ToInt32(masterChief.userName) == masterChiefID) // Si se esta loggueando el Chief
                {
                    Session["userName"] ="Jefe Desaarrollador";
                    Session["userRole"] = 0;
                    isChief = true;
                } else if (Employee == null) //if (Employee == null)
                {
                    //Tiene que ser cliente
                    var Client = db.Client.Find(userDetails.userName);
                    Session["userRealName"] = Client.clientName;
                    Session["userName"] = Client.clientName + " " + Client.clientLastName + " Cliente" ;
                    Session["userRole"] = 3; // Es cliente
                }
                else if(Employee != null)
                {
                    isEmployee = true;

                    if (leaderID1 != null) 
                    {
                        Session["userName"] = Employee.employeeName + " " + Employee.employeeLastName + "  Líder";
                        Session["userRole"] = 2;
                    }
                    else if (Employee.developerFlag == 1) //Desarrollador
                    {
                        Session["userName"] = Employee.employeeName + " " + Employee.employeeLastName + " Desarrollador";
                        Session["userRole"] = 1;
                    }
                       
                }

                // para que lo lleve al index que le corresponde
                if (isChief)
                    return RedirectToAction("Index", "Projects");
                if (isEmployee)
                {
                    ViewBag.profile = "EmployeeProfile";
                    return RedirectToAction("Index", "Projects");
                }
                else
                {
                    ViewBag.profile = "ClientProfile";
                    return RedirectToAction("Index", "Projects");
                }

            }
        }

        //EFE: Resetea las variables de session
        //REQ: Variables de session inicializadas
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("userName");
            Session.Remove("userRole");

            Session.Abandon();
            return RedirectToAction("Index", "LogIn");
        }

        //EFE: Retorna la vista del perfil de un cliente
        [HttpGet]
        public ActionResult ClientProfile()
        {
            //string id = Session.SessionID;
            string id = Session["userID"].ToString();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }else{
                Client client = db.Client.Find(id);
                if (client == null)
                {
                    return HttpNotFound();
                }
                return View(client);
            }
            
        }

        //EFE: Retorna la vista del perfil de un empleado
        [HttpGet]
        public ActionResult EmployeeProfile()
        {
            //string id = Session.SessionID;
            string id = Session["userID"].ToString();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }else{
                Employee employee = db.Employee.Find(id);
                if (employee == null)
                {
                    return HttpNotFound();
                }
                return View(employee);
            }

        }
        
        //EFE: Muestra la pantalla de cambiar contraseña
        [HttpGet]
        public ActionResult ChangePassword()
        {
            string id = Session["userID"].ToString();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }else{
                Credentials credentials = db.Credentials.Find(id);
                if (credentials == null)
                {
                    return HttpNotFound();
                }
                return View(credentials);
            }
        }

        //EFE: Guarda la nueva contraseña si la antigua es ingresada correctamente, de lo contrario pide al usuario volver a intentarlo
        //MOD: La tabla de credenciales
        [HttpPost]
        public ActionResult ChangePassword(string oldPass, string newPass)
        {
            string id = Session["userID"].ToString();
            Credentials credentials = db.Credentials.Find(id);
            if(credentials.password == oldPass)
            {
                credentials.password = newPass;
                db.SaveChanges();
                if (Convert.ToInt32(Session["userRole"]) != 3)
                {
                    return Json(new
                    {
                        redirectUrl = Url.Action("EmployeeProfile", "LogIn"),
                        isRedirect = true
                    });
                }
                else{
                    return Json(new
                    {
                        redirectUrl = Url.Action("ClientProfile", "LogIn"),
                        isRedirect = true
                    });
                }
            }else{
                return Json(new
                {
                    isRedirect = false
                });
            }
        }
    }
}