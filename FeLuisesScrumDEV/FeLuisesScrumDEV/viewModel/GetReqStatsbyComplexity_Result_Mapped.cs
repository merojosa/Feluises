using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeLuisesScrumDEV.viewModel
{
    using System;

    public partial class GetReqStatsbyComplexity_Result_Mapped
    {
        public Nullable<short> Dificultad { get; set; }
        public Nullable<int> Total { get; set; }
        public Nullable<int> Minima_Diff { get; set; }
        public Nullable<int> Max_Diff { get; set; }
        public Nullable<int> Avg_Diff { get; set; }
        public Nullable<int> Avg_Real { get; set; }
        public Nullable<int> Avg_Est { get; set; }
    }
}