//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TurApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SenderoSector
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SenderoSector()
        {
            this.Sendero = new HashSet<Sendero>();
        }
    
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string RutaZipMapa { get; set; }
        public string PesoZipMapa { get; set; }
        public string NombreDepartamento { get; set; }
        public Nullable<int> Version { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sendero> Sendero { get; set; }
    }
}
