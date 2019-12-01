using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeLuisesScrumDEV.viewModel
{
    public partial class GetKnowledgesSP_Result_Mapped
    {
        public string Conocimiento { get; set; }
        public Nullable<int> Total { get; set; }
        public Nullable<int> Promedio_Antiguedad { get; set; }
    }
}