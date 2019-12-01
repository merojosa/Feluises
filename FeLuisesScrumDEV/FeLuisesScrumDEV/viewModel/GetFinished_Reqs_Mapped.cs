using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FeLuisesScrumDEV.viewModel
{
    public class GetFinished_Reqs_Mapped
    {
        public String Nombre_Cliente { get; set; }
        public String Apellido_Cliente { get; set; }
        public String Nombre_Proyecto { get; set; }
        public Nullable<int> Requerimientos_Terminados { get; set; }
        public Nullable<int> Requerimientos_En_Proceso { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true , DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<DateTime> Fecha_Finalizacion { get; set; }
    }
}