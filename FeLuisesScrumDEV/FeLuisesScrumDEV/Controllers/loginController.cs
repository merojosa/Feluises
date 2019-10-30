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
             *  Cliente 4
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
                        Session["userName"] = Client.clientName + " " + Client.clientLastName + " " + Client.clientSecondLastName;
                        Session["userRole"] = 4; // Es cliente
                    }
                    else
                    {
                        isEmployee = true;
                        Session["userName"] = Employee.employeeName + " " + Employee.employeeLastName + " " + Employee.employeeSecondLastName;

                        if ()
                        {

                        }
                         
                        // Paso 1: Ir a empleado y revisar el developer flag. Si es no es desarrollador jefe entonces paso 2
                        // Paso 2: Ir a la tabla works  works in y revisar si devuelve algo preguntando si es lider de desarrrollo. Si devuelve una cantidad de tuplas mayor a 1, es lider desarrollador. Sino paso 3
                        // Paso 3: Ir a la tabla works  works in y revisar si devuelve algo preguntando si es desarrrollador. Si devuelve una cantidad de tuplas mayor a 1, es desarrollador. Sino, llorar amargamente y pasar al paso 4.
                        // Paso 4: Usuario sin rol. Sacarlo de sesióln y devolver a la pantalla inicial.
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