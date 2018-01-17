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
    
    public partial class Matricula
    {
        public int ID { get; set; }
        public int PersonaID { get; set; }
        public int TituloID { get; set; }
        public int OrganismoID { get; set; }
        public Nullable<bool> Revalido { get; set; }
        public Nullable<System.DateTime> FechaDiploma { get; set; }
        public string ObservacionDiploma { get; set; }
        public Nullable<System.DateTime> FechaInscripcion { get; set; }
        public int NroMatricula { get; set; }
        public string Folio { get; set; }
        public string libro { get; set; }
        public Nullable<System.DateTime> FechaActualizacion { get; set; }
        public bool Habilitada { get; set; }
        public byte TipoEstadoMatriculaID { get; set; }
        public bool Retirado { get; set; }
        public Nullable<System.DateTime> FechaRetiro { get; set; }
        public string ObservacionMatricula { get; set; }
        public bool TieneAnalitico { get; set; }
        public bool TieneTitulo { get; set; }
    
        public virtual Organismo Organismo { get; set; }
        public virtual Persona Persona { get; set; }
        public virtual TipoEstadoMatricula TipoEstadoMatricula { get; set; }
        public virtual Titulo Titulo { get; set; }
    }
}