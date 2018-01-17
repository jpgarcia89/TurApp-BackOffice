using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MSP_TurApp.Models
{
    [MetadataType(typeof(MatriculaMetadata))]
    public partial class Matricula
    {
    }

    public class MatriculaMetadata
    {
        /// <summary>
        /// Valida Datos Personales
        /// </summary>
                
        public Nullable<bool> Revalido
        {
            get { return Revalido ?? false; }
            set { Revalido = value; }
        }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int PersonaID { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int OrganismoID { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaDiploma { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaInscripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaActualizacion { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaRetiro { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int NroMatricula { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Folio { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string libro { get; set; }

        
        
        
        
        
        /// <summary>
        /// Sin Validar
        /// </summary>
        public int ID { get; set; }
        
        public int TituloID { get; set; }
        
        
        
        public string ObservacionDiploma { get; set; }
        
        
        public bool Habilitada { get; set; }
        public byte TipoEstadoMatriculaID { get; set; }
        public bool Retirado { get; set; }

        
        public string ObservacionMatricula { get; set; }
        public bool TieneAnalitico { get; set; }
        public bool TieneTitulo { get; set; }

        public virtual Organismo Organismo { get; set; }
        public virtual Persona Persona { get; set; }
        public virtual TipoEstadoMatricula TipoEstadoMatricula { get; set; }
        public virtual Titulo Titulo { get; set; }

    }
}