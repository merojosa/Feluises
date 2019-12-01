using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeLuisesScrumDEV.viewModel
{
    public class GetHistory_Results_Mapped
    {
        public String Nombre_Empleado { get; set; }
        public String Apellido_Empleado { get; set; }
        public String Nombre_Proyecto { get; set; }
        public Nullable<int> Rol { get; set; }
        public Nullable<int> Horas_trabajadas { get; set; }
    }
}