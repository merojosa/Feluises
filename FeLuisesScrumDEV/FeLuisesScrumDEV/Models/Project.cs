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
    
    public partial class Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Project()
        {
            this.Module = new HashSet<Module>();
            this.WorksIn = new HashSet<WorksIn>();
        }
    
        public int idProjectPK { get; set; }
        public string projectName { get; set; }
        public string objective { get; set; }
        public Nullable<decimal> estimatedCost { get; set; }
        public Nullable<decimal> realCost { get; set; }
        public Nullable<System.DateTime> startingDate { get; set; }
        public Nullable<System.DateTime> finishingDate { get; set; }
        public Nullable<decimal> budget { get; set; }
        public Nullable<int> estimatedDuration { get; set; }
        public string idClientFK { get; set; }
        public Nullable<System.DateTime> creationDate { get; set; }
        public Nullable<short> status { get; set; }
    
        public virtual Client Client { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Module> Module { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorksIn> WorksIn { get; set; }
    }
}
