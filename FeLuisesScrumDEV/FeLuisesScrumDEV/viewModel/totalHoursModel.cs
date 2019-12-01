using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeLuisesScrumDEV.Models;

namespace FeLuisesScrumDEV.viewModel
{
    public class totalHoursModel
    {
        public string LiderNombre { get; set; }
        public string NombreProyecto { get; set; }
        public Nullable<int> HorasEstimadas { get; set; }
        public Nullable<int> HorasReales { get; set; }
        //public Nullable<int> Diferencia { get; set; }
    }
}