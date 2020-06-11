using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeLuisesScrumDEV.Models
{
    public class CalendarData
    {
        public int idProjectFKPK { get; set; }
        public int idModuleFKPK { get; set; }
        public int idRequerimentPK { get; set; }
        public short? status { get; set; }
        public System.DateTime startingDate { get; set; }
        public System.DateTime? endDate { get; set; }
    }
}