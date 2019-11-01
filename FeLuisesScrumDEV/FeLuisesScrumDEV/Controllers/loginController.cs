using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeLuisesScrumDEV.Models;

namespace FeLuisesScrumDEV.Controllers
{
    public class loginController : Controller
    {

        //private FeLuisesEntities db = new FeLuisesEntities();
        // GET: login
        public ActionResult Index()
        {
            return View();
        }

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
             //EFE: Auntentica al usuario y le asigna su respectivo roll
             //REQ: Una usr y pass válido
            using (FeLuisesEntities db = new FeLuisesEntities())
            {
                var masterChiefID = 987654321; // Cambiar si hay otro jefe
                var userDetails = db.Credentials.Where(x => x.userName == loginModel.userName && x.password == loginModel.password).FirstOrDefault();
                if (userDetails == null)
                {
                    ModelState.AddModelError("", "Invalid username or password");
                    return View("Index");
                }else{
                    Session["userID"] = userDetails.userName; // esto pues as'i se llama en tabla
                    Boolean isEmployee = false;

                    //Sea cliente o empleado, hay que sacar su rol e id
                    var Employee = db.Employee.Find(userDetails.userName);
                    var masterChief = db.Credentials.Find(userDetails.userName); // solo el id
                    var chief = false;
                    if (Convert.ToInt32(masterChief.userName) == masterChiefID) // Si se esta loggueando el Chief
                    {
                        Session["userName"] ="Jefe Desaarrollador";
                        Session["userRole"] = 0;
                        chief = true;
                    } else if (Employee == null)                    //if (Employee == null)
                    {
                        //Tiene que ser cliente
                        var Client = db.Client.Find(userDetails.userName);
                        Session["userName"] = Client.clientName + " " + Client.clientLastName + " Cliente" ;
                        Session["userRole"] = 3; // Es cliente
                    }
                    else if(Employee != null)
                    {
                        isEmployee = true;

                        if (Employee.developerFlag == 1) //Desarrollador
                        {
                            Session["userName"] = Employee.employeeName + " " + Employee.employeeLastName + " Desarrollador";
                            Session["userRole"] = 1;
                        } 
                        if (Employee.developerFlag == 2) // Líder CAMBIAR CONSULTA 
                        {
                            Session["userName"] = Employee.employeeName + " " + Employee.employeeLastName + "  Líder";
                            Session["userRole"] = 2;
                        } 
                    }

                    // para que lo lleve al index que le corresponde
                    if (chief)
                        return RedirectToAction("Index", "Projects");
                    if (isEmployee)
                    {
                        return RedirectToAction("Index", "Employees");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Clients");
                    }

                }
            }
        }

        //EFE: Resetea las variables de session
        [HttpGet]
        public ActionResult Logout()
        {
            Session["userName"] = null;
            Session["userRole"] = null;
            return RedirectToAction("Index", "LogIn");
        }
    }
}