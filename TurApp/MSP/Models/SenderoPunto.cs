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
    
    public partial class SenderoPunto
    {
        public int ID { get; set; }
        public int SenderoID { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
    
        public virtual Sendero Sendero { get; set; }
    }
}
