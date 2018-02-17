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
    
    public partial class Sendero
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sendero()
        {
            this.SenderoPunto = new HashSet<SenderoPunto>();
            this.SenderoPuntoElevacion = new HashSet<SenderoPuntoElevacion>();
            this.SenderoPuntoInteres = new HashSet<SenderoPuntoInteres>();
        }
    
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string LugarInicio { get; set; }
        public string LugarFin { get; set; }
        public int TipoDificultadTecnicaID { get; set; }
        public int TipoDificultadFisicaID { get; set; }
        public string Desnivel { get; set; }
        public Nullable<double> Distancia { get; set; }
        public Nullable<double> AlturaMaxima { get; set; }
        public string DuracionTotal { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string RutaImagen { get; set; }
        public string RutZipMapa { get; set; }
        public Nullable<bool> CalcularIdaVuelta { get; set; }
        public string ImgBase64 { get; set; }
        public string InfoInteres { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SenderoPunto> SenderoPunto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SenderoPuntoElevacion> SenderoPuntoElevacion { get; set; }
        public virtual TipoDificultadFisica TipoDificultadFisica { get; set; }
        public virtual TipoDificultadTecnica TipoDificultadTecnica { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SenderoPuntoInteres> SenderoPuntoInteres { get; set; }
    }
}
