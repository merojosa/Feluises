using FeLuisesScrumDEV.Controllers.Services;
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

            List<CalendarData> listCalendarData = new List<CalendarData>();

            // Detecto si es cliente u otro rol
            // Session["userRole"]

            // Si es cliente, hago consulta para obtener los proyectos de ese cliente

            // Si es otro rol, obtengo todos los proyectos existentes

            // Obtengo el id cliente
            // string id = Session["userID"].ToString();

            // Llamo al procedimiento almacenado

            // Itero para poblar la lista del calendario

            // Mando los datos a la vista
            return View(listCalendarData);
        }
    }
}