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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            this.Project = new HashSet<Project>();
        }

        [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "La cédula debe de contener 9 dígitos")]
        [Key]
        public string idClientPK { get; set; }
        [Required(ErrorMessage ="Campo obligatorio, debe ingresar el nombre del cliente*")]
        [MaxLength(20, ErrorMessage = "El nombre del cliente no debe ser de más de 20 caracteres")]
        public string clientName { get; set; }
        [Required(ErrorMessage = "Campo obligatorio, debe de ingresar el apellido del cliente*")]
        [MaxLength(20, ErrorMessage = "El apellido del cliente no debe de ser de más de 20 caracteres")]
        public string clientLastName { get; set; }
        [MaxLength(20, ErrorMessage = "El segundo apellido del cliente no debe de ser de más de 20 caracteres")]
        public string clientSecondLastName { get; set; }
        [MaxLength(20, ErrorMessage = "El nombre de la compañía del cliente no debe de ser de más de 20 caracteres")]
        public string company { get; set; }
        [Required(ErrorMessage ="Campo obligatorio, debe de introducir un número de contacto*")]
        [RegularExpression(@"^[0-9-]{8,20}$", ErrorMessage = "Un número de teléfono solo permite números del 0 al 9 y '-'")]
        [MaxLength(20, ErrorMessage = "El número de teléfono no debe de ser de más de 20 caracteres")]
        public string tel { get; set; }
        [Required(ErrorMessage = "Campo obligatorio, debe de introducir un correo*")]
        [MaxLength(30, ErrorMessage = "El correo electrónico no debe de ser de más de 30 caracteres")]
        public string email { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Project> Project { get; set; }
    }
}
