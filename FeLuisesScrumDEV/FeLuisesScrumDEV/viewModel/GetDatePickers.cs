using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FeLuisesScrumDEV.viewModel
{
    using System;

    public partial class GetDatePickers
    {
        [Required(ErrorMessage = "Campo obligatorio, debe de ingresar la fecha de inicio del proyecto*")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid format, valid format is yyyy-MM-dd")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<System.DateTime> Start_Range { get; set; }

        [Required(ErrorMessage = "Campo obligatorio, debe de ingresar la fecha de inicio del proyecto*")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid format, valid format is yyyy-MM-dd")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<System.DateTime> End_Range { get; set; }
    }
}