using FeLuisesScrumDEV.Models;
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
        public ActionResult Index(string projectId)
        {
            List<CalendarData> listCalendarData = new List<CalendarData>();

            // Obtengo el id del usuario
            string id = Session["userID"].ToString();

            // Call SP with userId and projectId as parameters

            // Iterate the SP result to add the tuples to listCalendarData

            return View(listCalendarData);
        }
    }
}