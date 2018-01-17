using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MSP_TurApp.Models
{
    [MetadataType(typeof(PersonaMetadata))]
    public partial class Persona
    {
    }

    public class PersonaMetadata
    {
        /// <summary>
        /// Valida Datos Personales
        /// </summary>

        //[DataType(DataType.Date)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaNacimiento { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es requerido.")]
        public string Apellido { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es requerido.")]
        public string Nombre { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nro Documento")]
        public string NroDocumento { get; set; }



        /// <summary>
        /// Valida Datos Domicilio
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Calle")]
        public string DomicilioCalle { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nro")]
        public string DomicilioNumero { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "CP")]
        public string CodigoPostal { get; set; }


        /// <summary>
        /// Sin Validar
        /// </summary>
        public int ID { get; set; }
        
        public byte TipodniID { get; set; }
        
        public Nullable<byte> EstadoCivilID { get; set; }
        public Nullable<byte> SexoID { get; set; }
        public Nullable<int> LocalidadID { get; set; }
        
       
        public string DomicilioManzana { get; set; }
        public string DomicilioPiso { get; set; }
        public string DomicilioDepto { get; set; }
        public string CalleReferencia1 { get; set; }
        public string CalleReferencia2 { get; set; }
        public Nullable<int> BarrioID { get; set; }
        public string TelefonoFijo { get; set; }
        public string TelefonoCelular { get; set; }
        public string Email { get; set; }
        public Nullable<int> LocalidadNacimientoID { get; set; }
        public Nullable<int> NacionalidadID { get; set; }
        public string CUIL { get; set; }
        public bool Fallecido { get; set; }
        public System.DateTime FechaActualizacion { get; set; }
        public System.DateTime FechaAlta { get; set; }
        public Nullable<System.DateTime> FechaFallecido { get; set; }

        //public virtual Localidad Localidad { get; set; }
        //public virtual Localidad Localidad1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Matricula> Matricula { get; set; }
        public virtual TipoDNI TipoDNI { get; set; }
        public virtual TipoEstadoCivil TipoEstadoCivil { get; set; }
        public virtual TipoSexo TipoSexo { get; set; }
        public virtual Pais Pais { get; set; }




    }
}