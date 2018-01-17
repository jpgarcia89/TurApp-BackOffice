using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSP_TurApp.Models;
using MSP_TurApp.Helpers;



namespace MSP_TurApp.Controllers.Exportaciones.ExportarProfesionalesSISA
{
    

    public class ExportarProfesionalesSISAController : Controller
    {

        private MSPEntities db = new TurApp.Models.MSPEntities();

        // GET: ExportarProfesionalesSISA
        public ActionResult Index()
        {
            List<Persona> personas = new List<Persona>();                        
            personas.AddRange(db.Persona.Where(r => r.Matricula.Count > 0).Take(100).ToList());

            //personas.AddRange(db.Profesional.Where(r=>r.SisaId!=null).Select(r=>r.Persona).ToList());
            
            return View("Index", personas.ToList());
        }
    }
}