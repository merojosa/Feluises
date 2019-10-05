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
        public ActionResult Autherize(FeLuisesScrumDEV.Models.Credentials loginModel)
        {
            using (FeLuisesEntities db = new FeLuisesEntities())
            {
                var userDetails = db.Credentials.Where(x => x.userName == loginModel.userName && x.password == loginModel.password).FirstOrDefault();
                if (userDetails == null)
                {
                    //userModel.LoginErrorMessage = "Wrong username or password.";
                    return View("Index");
                }
                else
                {
                    //Session["userID"] = userDetails.userID;
                    //Session["userName"] = userDetails.UserName;
                    Boolean isEmployee = false;

                    //Sea cliente o empleado, hay que sacar su rol e id
                    var Employee = db.Employee.Find(userDetails.userName);
                    if (Employee == null)
                    {
                        //Tiene que ser cliente
                        var Client = db.Client.Find(userDetails.userName);
                    }
                    else
                    {
                        isEmployee = true;
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