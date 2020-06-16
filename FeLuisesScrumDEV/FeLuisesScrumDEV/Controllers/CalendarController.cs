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
        private FeLuisesEntities db = new FeLuisesEntities();

        // GET: Calendar
        public ActionResult Index(int? projectId)
        {
            // Obtengo el id del usuario
            string clientId = Session["userID"].ToString();

            List<CalendarData> listCalendarData = getRequeriments(clientId, 1);

            // Iterate the SP result to add the tuples to listCalendarData

            return View("Index", listCalendarData);
        }

        private List<CalendarData> getRequeriments(string clientId, int? projectId)
        {
            List<CalendarData> listCalendarData = new List<CalendarData>();

            var tuples = db.SP_GetRequirements(clientId, projectId);

            CalendarData calendarData = null;

            foreach (var item in tuples)
            {
                calendarData = new CalendarData();
                calendarData.idProjectFKPK = item.idProjectFKPK;
                calendarData.idModuleFKPK = item.idModuleFKPK;
                calendarData.idRequerimentPK = item.idRequerimentPK;
                calendarData.status = item.status;
                calendarData.startingDate = item.startingDate;
                calendarData.endDate = item.endDate;

                listCalendarData.Add(calendarData);
            }

            return listCalendarData;
        }
    }
}