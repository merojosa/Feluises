namespace FeLuisesScrumDEV.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Module
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Module()
        {
            this.Requeriment = new HashSet<Requeriment>();
        }
    
        [Key]
        public int idProjectFKPK { get; set; }
        [Key]
        public int idModulePK { get; set; }
        [MaxLength(30,ErrorMessage ="Module's name cant be longer than 30 characters.")]
        [Required(ErrorMessage = "Name must be specified.")]
        public string name { get; set; }
    
        public virtual Project Project { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requeriment> Requeriment { get; set; }
    }
}
