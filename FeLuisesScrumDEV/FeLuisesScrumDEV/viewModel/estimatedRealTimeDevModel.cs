using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeLuisesScrumDEV.Models;

namespace FeLuisesScrumDEV.viewModel
{
    public class estimatedRealTimeDevModel
    {
        public string DesarrolladorNombre { get; set; }
        public string Proyecto { get; set; }
        public string Requerimiento { get; set; }
        public Nullable<int> Complejidad { get; set; }
        public Nullable<int> DuracionEstimada { get; set; }
        public Nullable<int> DuracionReal { get; set; }
        public Nullable<int> Diferencia { get; set; }
    }
}