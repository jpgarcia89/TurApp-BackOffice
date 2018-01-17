using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSP_RegProf.Models
{
    public class TituloProfVM
    {
        public int titId { get; set; }

        public int profId { get; set; }

        public string titDenominacion { get; set; }

        public string titMatricula { get; set; }

        public bool existeDoc { get; set; }
    }
}