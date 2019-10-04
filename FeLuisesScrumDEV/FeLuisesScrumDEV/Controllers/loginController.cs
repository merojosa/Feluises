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

        private FeLuisesEntities db = new FeLuisesEntities();
        // GET: login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult action(string idUser, string password)
        {
            Boolean registered = false;
            //Verifica usuario en tabla de credenciales
            var userCredentials = db.Credentials.Find(idUser);
            if (userCredentials != null)
            {
                //Verifacamos el password
                if (userCredentials.password == password)
                {
                    registered = true;
                }

                //Si registrado, busca en empleados o en cliente
                if (registered)
                {
                    Boolean isEmployee = false;
                    //Sea cliente o empleado, hay que sacar su rol e id
                    var Employee = db.Employee.Find(idUser);
                    if(Employee == null)
                    {
                        //Tiene que ser cliente
                        var Client = db.Client.Find(idUser);
                    }else{
                        //Es empleado, entonces tenemos que loguearnos como empleado
                        //Hay que sacar su rol e id
                        isEmployee = true;
                    }

                    if (isEmployee){
                   
                        //ViewBag.role = "Employees";
                        //return View("Index", "Employee");
                        return RedirectToAction("Employees"); //con id y rol de empleado
                    }else{
                        //ViewBag.role = "Client";
                        //return View(); //con id y rol de cliente
                        return RedirectToAction("Clients");
                    }  
                }
            }
            //Este view debe retornar a la misma pantalla de login pero con un mensaje de error
            return View("Login");   
        }


    }
}