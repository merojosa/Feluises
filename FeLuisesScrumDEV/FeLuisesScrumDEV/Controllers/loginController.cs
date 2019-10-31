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
             * Developerflag:
             *                  0 = Jefe Desarrollador
             *                  1 = Líder 
             *                  2 = Desarrollador
             *  Cliente     3
             * 
             */
            using (FeLuisesEntities db = new FeLuisesEntities())
            {
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

                if (Employee == null)
                    {
                        //Tiene que ser cliente
                        var Client = db.Client.Find(userDetails.userName);
                        Session["userName"] = Client.clientName + " " + Client.clientLastName + " Cliente" ;
                        Session["userRole"] = 3; // Es cliente
                    }
                    else
                    {
                        isEmployee = true;
                        var employeeType = Employee.developerFlag; // 1 = líder 2 = desarrollador
                        if (employeeType == 1)
                        {
                            Session["userName"] = Employee.employeeName + " " + Employee.employeeLastName + " Líder";
                            Session["userRole"] = 1;
                        } else if ( employeeType == 2)
                        {
                            Session["userName"] = Employee.employeeName + " " + Employee.employeeLastName + "  Desarrollador";
                            Session["userRole"] = 2;
                        } else if(employeeType == 0)
                        {
                            Session["userName"] = Employee.employeeName + " " + Employee.employeeLastName + "  Jefe";
                            Session["userRole"] = 0;
                        }
                    }

                    
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
    }
}