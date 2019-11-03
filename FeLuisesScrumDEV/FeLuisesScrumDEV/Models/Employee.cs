//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FeLuisesScrumDEV.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.DeveloperKnowledge = new HashSet<DeveloperKnowledge>();
            this.Requeriment = new HashSet<Requeriment>();
            this.WorksIn = new HashSet<WorksIn>();
        }


        [Key]
        [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "La cédula debe de contener 9 dígitos")]
        [MaxLength(9, ErrorMessage = "La cédula debe de contener 9 dígitos")]
        public string idEmployeePK { get; set; }
        [MaxLength(20, ErrorMessage = "El nombre de un empleado no debe ser de más de 20 caracteres")]
        [Required(ErrorMessage = "Campo obligatorio, debe de ingresar el nombre del empleado*")]
        public string employeeName { get; set; }
        [MaxLength(20, ErrorMessage = "El apellido de un empleado no debe ser de más de 20 caracteres")]
        [Required(ErrorMessage = "Campo obligatorio, debe de ingresar el apellido del empleado*")]
        public string employeeLastName { get; set; }
        [MaxLength(20, ErrorMessage = "El segundo apellido de un empleado no debe ser de más de 20 caracteres")]
        public string employeeSecondLastName { get; set; }
        [Required(ErrorMessage = "Campo obligatorio, debe de ingresar la fecha de nacimiento del empleado*")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<System.DateTime> employeeBirthDate { get; set; }
        [Required(ErrorMessage = "Campo obligatorio, debe de ingresar la fecha de contratación del empleado*")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public System.DateTime employeeHireDate { get; set; }
        public Nullable<short> developerFlag { get; set; }
        [RegularExpression(@"^[0-9-]{8,20}$", ErrorMessage = "Un número de teléfono solo permite números del 0 al 9 y '-'")]
        public string tel { get; set; }
        public string email { get; set; }
        //[MaxLength(20, ErrorMessage = "La provincia no debe de poseer más de 20 caracteres")]
        public string province { get; set; }
        //[MaxLength(20, ErrorMessage = "El nombre del canton no debe de poseer más de 20 caracteres")]
        public string canton { get; set; }
        //[MaxLength(20, ErrorMessage = "El nombre del distrito no debe de poseer más de 20 caracteres")]
        public string district { get; set; }
        [MaxLength(35, ErrorMessage = "La descripción de la dirección exacta no debe de poseer más de 35 caracteres")]
        public string exactDirection { get; set; }
        //Comentado para que se valide equipo
        //[RegularExpression(@"^\d{1,20}$", ErrorMessage = "El presupuesto no debe de contener más de 18 enteros y 2 decimales.")]
        [Range(0, 9999999999999999.99)]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public Nullable<decimal> pricePerHour { get; set; }
        public Nullable<short> availability { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeveloperKnowledge> DeveloperKnowledge { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requeriment> Requeriment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorksIn> WorksIn { get; set; }
    }
}
