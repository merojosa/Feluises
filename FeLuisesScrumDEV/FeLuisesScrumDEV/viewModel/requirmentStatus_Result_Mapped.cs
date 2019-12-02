using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FeLuisesScrumDEV.viewModel
{
    using System;
    public class requirmentStatus_Result_Mapped
    {
        public string Nombre_Requerimiento { get; set;}
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString ="{0:yyyy-MM-dd}")]
        public System.DateTime Fecha_de_inicio { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<System.DateTime> Fecha_de_Finalizacion { get; set; }
        public Nullable<int> Duracion_Estimada { get; set; }
        public Nullable<int> Estado { get; set; }
        public string Nombre_Desarrollador { get; set;}
        public string Nombre_Lider { get; set; }
        public string Nombre_Proyecto { get; set; }
    }
}