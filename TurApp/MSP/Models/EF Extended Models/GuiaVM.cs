using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TurApp.Models
{
    [MetadataType(typeof(GuiaMetadata))]
    public partial class Guia
    {
    }

    public class GuiaMetadata
    {
        /// <summary>
        /// Valida Datos Personales
        /// </summary>

        [Required(AllowEmptyStrings = false, ErrorMessage ="El campo {0} es requerido.")]
        [Display(Name= "ID")]
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nombre")]
        [StringLength(100, ErrorMessage = "Cantidad de caracteres invalido")]
        public string Nombre { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Telefono")]
        [StringLength(50, MinimumLength = 16, ErrorMessage = "Cantidad de caracteres invalido")]
        public string Telefono { get; set; }
        /// <summary>
        /// Sin Validar
        /// </summary>




    }
}