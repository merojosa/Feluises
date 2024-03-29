﻿using FeLuisesScrumDEV.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.Entity;
using System;
using System.Linq;

namespace FeLuisesScrumDEV.Controllers
{
    public class CalendarController : Controller
    {
        private FeLuisesEntities db = new FeLuisesEntities();

        // GET: Calendar
        public ActionResult Index(int? projectId)
        {
            var projects = db.Project;
            ViewBag.idProjectFKPK = new SelectList(projects, "idProjectPK", "projectName");

            // Obtengo el id del usuario
            string clientId = Session["userID"].ToString();

            List<CalendarData> listCalendarData = getRequeriments(clientId, projectId == null ? projects.First().idProjectPK : projectId);

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
                calendarData.startingDate = item.startingDate.ToString("yyyy-MM-dd");
                if (item.endDate == null)
                {
                    calendarData.endDate = "";
                }
                else
                {
                    calendarData.endDate = ((DateTime)(item.endDate)).ToString("yyyy-MM-dd");
                }
                

                listCalendarData.Add(calendarData);
            }

            return listCalendarData;
        }
    }
}