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
    
    public partial class HorariosPorEspecialidadPorCentroDeSalud
    {
        public int ID { get; set; }
        public short HorarioIDEntrada { get; set; }
        public short HorarioIDSalida { get; set; }
        public int EspecialidadPorCentroDeSaludID { get; set; }
        public byte Dia { get; set; }
        public bool Activo { get; set; }
    
        public virtual EspecialidadPorCentroDeSalud EspecialidadPorCentroDeSalud { get; set; }
        public virtual Horarios Horarios { get; set; }
        public virtual Horarios Horarios1 { get; set; }
    }
}