using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeLuisesScrumDEV.viewModel
    {
        using System;

        public partial class GetDevs
        {
            public string Nombre_Desarrollador { get; set; }
            public string Nombre_Proyecto { get; set; }
            public string Nombre_Requerimiento { get; set; }
            public string Fecha_Inicio { get; set; }
            public string Fecha_EstimadaFin { get; set; }
        }
    }