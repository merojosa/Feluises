using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FeLuisesScrumDEV.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar
        public ActionResult Index()
        {
            // Obtengo datos por medio del procedimiento almacenado

            // Mando los datos a la vista
            return View();
        }
    }
}