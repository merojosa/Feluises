using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeLuisesScrumDEV.viewModel
{
    using System;
    public class requirmentStatus_Result_Mapped
    {
        public string Nombre_Requerimiento { get; set;}
        public System.DateTime Fecha_de_inicio { get; set; }
        public System.DateTime Fecha_de_Finalizacion { get; set; }
        public int Duracion_Estimada { get; set; }
    }
}