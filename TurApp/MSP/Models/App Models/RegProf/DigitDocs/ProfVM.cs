using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSP_RegProf.Models
{
    public class ProfVM
    {
        public int profId { get; set; }
        public string profDni { get; set; }

        public string profNombre { get; set; }

        public string profApellido { get; set; }

        public string profPais { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime profFechaNac { get; set; }


        public List<TituloProfVM> ListaTitulos { get; set; }



        public static ProfVM GetProfDummy()
        {
            ProfVM Profesional = new ProfVM()
            {
                profId=1,
                profDni = "28558963",
                profNombre = "Pepe",
                profApellido = "Panzas",
                profFechaNac = DateTime.Today
            };

            List<TituloProfVM> ListaTitulos = new List<TituloProfVM> {
               new TituloProfVM() {titId=1,titDenominacion="Medico Clinico",titMatricula="15847"},
               new TituloProfVM() {titId=2,titDenominacion="Traumatologo",titMatricula="92584"},
               new TituloProfVM() {titId=3,titDenominacion="Cirujano",titMatricula="14785"}
            };

            Profesional.ListaTitulos=ListaTitulos;

            return Profesional;
        }

        public static List<ProfVM> GetListaProfDummy()
        {
            List<ProfVM> ListaProfesionales = new List<ProfVM>();

            //Prof nº 1
            ProfVM Profesional = new ProfVM()
            {
                profId = 1,
                profDni = "123456",
                profNombre = "Jose",
                profApellido = "Peralta",
                profFechaNac = DateTime.Today,
                profPais = "Argentina"
            };
            List<TituloProfVM> ListaTitulos = new List<TituloProfVM> {
               new TituloProfVM() {profId = 1,titId=1,titDenominacion="Medico Clinico",titMatricula="15747"},
               new TituloProfVM() {profId = 1,titId=2,titDenominacion="Traumatologo",titMatricula="93284"},
               new TituloProfVM() {profId = 1,titId=3,titDenominacion="Cirujano",titMatricula="17235"}
            };
            Profesional.ListaTitulos = ListaTitulos;
            ListaProfesionales.Add(Profesional);


            //Prof nº 2
            Profesional = new ProfVM()
            {
                profId = 2,
                profDni = "34329291",
                profNombre = "Paulo",
                profApellido = "Garcia",
                profFechaNac = DateTime.Today,
                profPais = "Argentina"
            };
            ListaTitulos = new List<TituloProfVM> {
               new TituloProfVM() {profId = 2,titId=4,titDenominacion="Medico Clinico",titMatricula="87947"},
               new TituloProfVM() {profId = 2,titId=5,titDenominacion="Incologia",titMatricula="92584"},
               new TituloProfVM() {profId = 2,titId=6,titDenominacion="Neurocirujano",titMatricula="112345"}
            };
            Profesional.ListaTitulos = ListaTitulos;
            ListaProfesionales.Add(Profesional);

            //Prof nº 3
            Profesional = new ProfVM()
            {
                profId = 3,
                profDni = "987654",
                profNombre = "Luis",
                profApellido = "Manzano",
                profFechaNac = DateTime.Today,
                profPais = "Chile"
            };
            ListaTitulos = new List<TituloProfVM> {
               new TituloProfVM() {profId = 3,titId=7,titDenominacion="Medico Clinico",titMatricula="15847"},
               new TituloProfVM() {profId = 3,titId=8,titDenominacion="Traumatologo",titMatricula="92584"},
               new TituloProfVM() {profId = 3,titId=9,titDenominacion="Cirujano",titMatricula="14785"}
            };
            Profesional.ListaTitulos = ListaTitulos;
            ListaProfesionales.Add(Profesional);

            //Prof nº 4
            Profesional = new ProfVM()
            {
                profId = 4,
                profDni = "147258",
                profNombre = "Pepe",
                profApellido = "Panzas",
                profFechaNac = DateTime.Today,
                profPais = "Peru"
            };
            ListaTitulos = new List<TituloProfVM> {
               new TituloProfVM() {profId = 4,titId=10,titDenominacion="Medico Clinico",titMatricula="15847"},
               new TituloProfVM() {profId = 4,titId=11,titDenominacion="Traumatologo",titMatricula="92584"},
               new TituloProfVM() {profId = 4,titId=12,titDenominacion="Cirujano",titMatricula="14785"}
            };
            Profesional.ListaTitulos = ListaTitulos;
            ListaProfesionales.Add(Profesional);

            //Prof nº 5
            Profesional = new ProfVM()
            {
                profId = 5,
                profDni = "369258",
                profNombre = "Carlos",
                profApellido = "Mas",
                profFechaNac = DateTime.Today,
                profPais = "Argentina"
            };
            ListaTitulos = new List<TituloProfVM> {
               new TituloProfVM() {profId = 5,titId=13,titDenominacion="Medico Clinico",titMatricula="15847"},
               new TituloProfVM() {profId = 5,titId=14,titDenominacion="Traumatologo",titMatricula="92584"},
               new TituloProfVM() {profId = 5,titId=15,titDenominacion="Cirujano",titMatricula="14785"}
            };
            Profesional.ListaTitulos = ListaTitulos;
            ListaProfesionales.Add(Profesional);

            //Prof nº 6
            Profesional = new ProfVM()
            {
                profId = 6,
                profDni = "741852",
                profNombre = "Pepe",
                profApellido = "Panzas",
                profFechaNac = DateTime.Today,
                profPais = "Argentina"
            };
            ListaTitulos = new List<TituloProfVM> {
               new TituloProfVM() {profId = 6,titId=16,titDenominacion="Medico Clinico",titMatricula="15847"},
               new TituloProfVM() {profId = 6,titId=17,titDenominacion="Traumatologo",titMatricula="92584"},
               new TituloProfVM() {profId = 6,titId=18,titDenominacion="Cirujano",titMatricula="14785"}
            };
            Profesional.ListaTitulos = ListaTitulos;
            ListaProfesionales.Add(Profesional);


            return ListaProfesionales;
        }

    }
}