//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
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
