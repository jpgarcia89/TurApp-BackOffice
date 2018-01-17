using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSP_TurApp.Models;
using System.IO;
using WebGrease.Activities;
using System.Data.Entity;

namespace MSP_TurApp.Controllers
{
    [Authorize]
    public class DigitDocsController : Controller
    {
        private MSPEntities db = new MSPEntities();

        // GET: DigitDocs
        public ActionResult Index()
        {
            var model = ProfVM.GetProfDummy();

            return View(model);
        }

        // POST: DigitDocs/BuscaProf/{dni}
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult BuscaProf(string profDni)
        {
            //Implementar Busqueda del Profecional
            //var Profesional = ProfVM.GetListaProfDummy().Where(r => r.profDni == profDni).FirstOrDefault();

            profDni = profDni.Trim();
            var Profesional = db.Persona.Include(r => r.Matricula).Where(r => r.NroDocumento == profDni).FirstOrDefault();

            if (Profesional == null)
            {
                return Json(new
                {
                    ok = "false",
                    mensaje = "DNI incorrecto o Profesional no registrado"
                });
            }

            return PartialView("_datosProf", Profesional);
        }


        // GET: DigitDocs/SubirDocs
        [Route("DigitDocs/SubirDocs/{perId}/{matId}")]
        [HttpGet]
        public ActionResult SubirDocs(int perId, int matId)
        {
            //var titulo = ProfVM.GetListaProfDummy().Where(r => r.profId == profId).FirstOrDefault().ListaTitulos.Where(r => r.titId == titId).FirstOrDefault();

            //var titulo = db.Persona.FirstOrDefault(r => r.ID == perId).Matricula.FirstOrDefault(r => r.NroMatricula == matId);
            var Matricula = db.Matricula.FirstOrDefault(r => r.ID == matId);
            return PartialView(Matricula);
        }

        // POST: DigitDocs/Create
        [Route("DigitDocs/SubirDocs/{perId}/{matId}")]
        [HttpPost]
        public ActionResult SubirDocs(HttpPostedFileBase docTitulo, HttpPostedFileBase docAnalitico, int perId, int matId)//
        {
            try
            {
                //var profesional = ProfVM.GetListaProfDummy().Where(r => r.profId == perId).FirstOrDefault();
                var Matricula = db.Matricula.FirstOrDefault(r => r.ID == matId);
                Matricula.FechaActualizacion = DateTime.Now;

                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    //string _IdMatricula = profesional.profId.ToString()+"_"+ profesional.ListaTitulos.Where(r=>r.titId== matId).FirstOrDefault().titMatricula.ToString();
                    string _IdMatricula = Matricula.PersonaID.ToString() + "_" + Matricula.NroMatricula.ToString();

                    //Titulo
                    if (files["docTitulo"] != null)
                    {
                        docTitulo = files["docTitulo"];

                        if (docTitulo.ContentLength > 0)
                        {
                            string _FileName = _IdMatricula + "_Titulo" + Path.GetExtension(docTitulo.FileName);
                            System.IO.Directory.CreateDirectory(Server.MapPath("~/UploadedFiles/Profesionales/" + Matricula.PersonaID.ToString() + "/"+ _IdMatricula));
                            string _path = Path.Combine(Server.MapPath("~/UploadedFiles/Profesionales/" + Matricula.PersonaID.ToString() + "/" + _IdMatricula), _FileName);
                            docTitulo.SaveAs(_path);
                            Matricula.TieneTitulo = true;
                        }
                        ViewBag.Message1 = "Titulo subido correctamente!!";
                    }

                    //Analitico
                    if (files["docAnalitico"] != null)
                    {
                        docAnalitico = files["docAnalitico"];

                        if (docAnalitico.ContentLength > 0)
                        {
                            string _FileName = _IdMatricula + "_Analitico" + Path.GetExtension(docAnalitico.FileName);
                            System.IO.Directory.CreateDirectory(Server.MapPath("~/UploadedFiles/Profesionales/" + Matricula.PersonaID.ToString() + "/" + _IdMatricula));
                            string _path = Path.Combine(Server.MapPath("~/UploadedFiles/Profesionales/" + Matricula.PersonaID.ToString() + "/" + _IdMatricula), _FileName);
                            docAnalitico.SaveAs(_path);
                            Matricula.TieneAnalitico = true;
                        }
                        ViewBag.Message2 = "Analitico subido correctamente!!";
                    }

                    db.SaveChanges();
                    return PartialView();
                }
                else
                {
                    ViewBag.Message = "No se subio ningun archivo.";
                    return PartialView();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message1 = null;
                ViewBag.Message2 = null;
                ViewBag.Message = "Ocurrio un error! Intente nuevamente.\n Mensaje: "+ ex.Message??ex.InnerException.Message;
                return PartialView();
            }
        }


        // GET: DigitDocs/SubirDocs
        [Route("DigitDocs/VerDocs/{perId}/{matId}")]
        [HttpGet]
        public ActionResult VerDocs(int perId, int matId)
        {
            //var titulo = ProfVM.GetListaProfDummy().Where(r => r.profId == perId).FirstOrDefault().ListaTitulos.Where(r => r.titId == matId).FirstOrDefault();
            var Matricula = db.Matricula.FirstOrDefault(r => r.ID == matId);
            return PartialView(Matricula);
        }

        // GET: DigitDocs/SubirDocs
        [Route("DigitDocs/VerDocs/{tipoDoc}/{perId}/{matId}")]
        [HttpGet]
        public FileContentResult VerDocs(string tipoDoc, int perId, int matId)
        {
            var Matricula = db.Matricula.FirstOrDefault(r => r.ID == matId);
            string _IdMatricula = Matricula.PersonaID.ToString() + "_" + Matricula.NroMatricula.ToString();

            //var profesional = ProfVM.GetListaProfDummy().Where(r => r.profId == profId).FirstOrDefault();
            //string _IdMatricula = profesional.profId.ToString() + "_" + profesional.ListaTitulos.Where(r => r.titId == matId).FirstOrDefault().titMatricula.ToString();

            switch (tipoDoc)
            {
                case "docTitulo":
                    {
                        var fullPathToFile = Server.MapPath("~/UploadedFiles/Profesionales/" + Matricula.PersonaID.ToString() + "/" + _IdMatricula + "/" + _IdMatricula + "_Titulo.pdf");
                        var mimeType = "application/pdf";
                        var fileContents = System.IO.File.ReadAllBytes(fullPathToFile);

                        return new FileContentResult(fileContents, mimeType);
                    }
                    //break;

                case "docAnalitico":
                    {
                        var fullPathToFile = Server.MapPath("~/UploadedFiles/Profesionales/" + Matricula.PersonaID.ToString() + "/" + _IdMatricula + "/" + _IdMatricula + "_Analitico.pdf");
                        var mimeType = "application/pdf";
                        var fileContents = System.IO.File.ReadAllBytes(fullPathToFile);

                        return new FileContentResult(fileContents, mimeType);
                    }
                    //break

                default:
                    return null;
                    //break;
            }



        }

        // GET: DigitDocs/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DigitDocs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DigitDocs/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DigitDocs/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DigitDocs/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DigitDocs/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DigitDocs/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
