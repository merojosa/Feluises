using System;
using System.Web;
using System.Web.Mvc;
using FeLuisesScrumDEV.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using FeLuisesScrumDEV.Models;

namespace TestFeLuisesScrumDEV
{
    [TestClass]
    public class CalendarControllerTest
    {
        private const string existentUserId = "333333333";
        private const int existentProjectId = 1;

        private CalendarController initCalendarController(string userId)
        {
            var mockContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockContext.Setup(p => p.HttpContext.Session["userID"]).Returns(userId);

            CalendarController calendarController = new CalendarController();
            calendarController.ControllerContext = mockContext.Object;

            return calendarController;
        }

        [TestMethod]
        public void indexNotNullTest()
        {
            CalendarController calendarController = initCalendarController(existentUserId);
            ViewResult result = calendarController.Index(existentProjectId) as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void indexViewTest()
        {
            CalendarController calendarController = initCalendarController(existentUserId);
            ViewResult result = calendarController.Index(existentProjectId) as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        // Existe usuario, existe proyecto
        [TestMethod]
        public void getRequirementsTest()
        {
            CalendarController calendarController = initCalendarController(existentUserId);
            ViewResult result = calendarController.Index(existentProjectId) as ViewResult;

            List<CalendarData> list = (List<CalendarData>)result.Model;
            Assert.IsTrue(list.Count > 0);
        }

        // Existe usuario, no hay proyecto
        [TestMethod]
        public void getRequirementsNoProjectTest()
        {
            CalendarController calendarController = initCalendarController(existentUserId);
            ViewResult result = calendarController.Index(-1) as ViewResult;

            List<CalendarData> list = (List<CalendarData>)result.Model;
            Assert.IsFalse(list.Count > 0);
        }

        // No hay usuario, existe proyecto
        [TestMethod]
        public void getRequirementsNoUserTest()
        {
            CalendarController calendarController = initCalendarController("");
            ViewResult result = calendarController.Index(existentProjectId) as ViewResult;

            List<CalendarData> list = (List<CalendarData>)result.Model;
            Assert.IsFalse(list.Count > 0);
        }


        // No hay usuario, no hay proyecto
        [TestMethod]
        public void getRequirementsNoUserNoProjectTest()
        {
            CalendarController calendarController = initCalendarController("");
            ViewResult result = calendarController.Index(null) as ViewResult;

            List<CalendarData> list = (List<CalendarData>)result.Model;
            Assert.IsFalse(list.Count > 0);
        }
    }
}
