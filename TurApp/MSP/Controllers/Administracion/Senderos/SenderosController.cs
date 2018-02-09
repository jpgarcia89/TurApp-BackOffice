using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TurApp.Models;

namespace TurApp.Controllers
{
    [Authorize]
    public class SenderosController : Controller
    {
        private TurAppEntities db = new TurAppEntities();

        // GET: Sendero
        public ActionResult Index()
        {
            return View(db.Sendero.ToList());
        }

        // GET: Sendero/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sendero Sendero = db.Sendero.Find(id);
            if (Sendero == null)
            {
                return HttpNotFound();
            }
            return View(Sendero);
        }

        // GET: Sendero/Create
        public ActionResult Create()
        {
            ViewBag.TipoDificultadFisicaID = new SelectList(db.TipoDificultadFisica, "ID", "Descripcion");
            ViewBag.TipoDificultadTecnicaID = new SelectList(db.TipoDificultadTecnica, "ID", "Descripcion");


            return View();
        }

        // POST: Sendero/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Sendero sendero, HttpPostedFileBase senderoImg, HttpPostedFileBase zipMapa)//[Bind(Include = "ID,Nombre,Descripcion,LugarInicio,LugarFin,TipoDificultadTecnicaID,TipoDificultadFisicaID,Desnivel,Distancia,AlturaMaxima,DuracionTotal")] 
        {
            sendero = JsonConvert.DeserializeObject<Sendero>(Request.Form["Sendero"]);
            //var x =Request.Files.Count;
            //var ssendero = Request.Form["Sendero"];
            //sendero.ID = 2;//dato de prueba

            ModelState["sendero"].Errors.Clear();
            UpdateModel<Sendero>(sendero);

            try
            {
                if (ModelState.IsValid)
                {
                    db.Sendero.Add(sendero);
                    db.SaveChanges();


                    if (Request.Files.Count > 0)
                    {
                        //Ej. ruta de recursos(imagen) de senderos: "~/Content/Senderos/2/Img/senderoImg_2.jpg"                                                
                        //Ej. ruta de recursos(mapa offline) de senderos: "~/Content/Senderos/2/Mapa/senderoMapa_2.zip"                          

                        //SenderoImg
                        #region process SenderoImg
                        if (senderoImg != null)
                        {
                            if (senderoImg.ContentLength > 0)
                            {
                                //Validate file extension
                                string fileExtension = Path.GetExtension(senderoImg.FileName);
                                if (new[] { ".jpg", ".png" }.Any(c => fileExtension == c))
                                {
                                    //Create name and paths
                                    string fileName = "senderoImg_" + sendero.ID + Path.GetExtension(senderoImg.FileName);  //Ej: "senderoiImg_2.jpg"
                                    string virtualDirectoryPath = "~/Content/Senderos/" + sendero.ID + "/Img";              //Ej: "~/Content/Senderos/2/Img/"
                                    string phisicaldirectoryPath = Server.MapPath(virtualDirectoryPath);                    //Ej: "F:/Sistemas/GitHub-Repositorios/TurApp-BackOffice/TurApp/MSP/Content/Senderos/2/Img"
                                    string fullVirtualPath = virtualDirectoryPath + "/" + fileName;
                                    string fullPhisicalPath = Path.Combine(phisicaldirectoryPath, fileName);                //Ej: "~/Content/Senderos/2/Img/senderoiImg_2.jpg"

                                    //CreateDirectory
                                    System.IO.Directory.CreateDirectory(phisicaldirectoryPath);

                                    //Save file in a phisicaldirectoryPath
                                    senderoImg.SaveAs(fullPhisicalPath);

                                    //Update in DB Sendero file path        
                                    sendero.RutaImagen = fullVirtualPath.Substring(1, fullVirtualPath.Length-1);
                                    db.SaveChanges();
                                }
                            }
                        }
                        #endregion

                        //ZipMapa
                        #region process ZipMapa
                        if (zipMapa != null)
                        {
                            if (zipMapa.ContentLength > 0)
                            {
                                //Validate file extension
                                string fileExtension = Path.GetExtension(senderoImg.FileName);
                                if (new[] { ".zip" }.Any(c => fileExtension == c))
                                {
                                    //Create name and paths
                                    string fileName = "senderoMapa_" + sendero.ID + Path.GetExtension(zipMapa.FileName);        //Ej: "senderoiMapa_2.jpg"
                                    string virtualDirectoryPath = "~/Content/Senderos/" + sendero.ID + "/Mapa";                 //Ej: "~/Content/Senderos/2/Mapa/"
                                    string phisicaldirectoryPath = Server.MapPath(virtualDirectoryPath);
                                    string fullVirtualPath = Path.Combine(virtualDirectoryPath, fileName);
                                    string fullPhisicalPath = Path.Combine(phisicaldirectoryPath, fileName);                    //Ej: "~/Content/Senderos/2/Img/senderoiImg_2.jpg"

                                    //CreateDirectory
                                    System.IO.Directory.CreateDirectory(phisicaldirectoryPath);

                                    //Save file in a phisicaldirectoryPath
                                    senderoImg.SaveAs(fullPhisicalPath);

                                    //Update in DB Sendero file path            
                                    sendero.RutZipMapa = fullVirtualPath;
                                    db.SaveChanges();       
                                }
                            }
                        }
                        #endregion
                                                
                    }

                    return Json(new
                    {
                        ok = true
                    });
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }

                //return Json(new
                //{
                //    ok = false,
                //    msj= ex.InnerException.Message
                //});
            }




            ViewBag.TipoDificultadFisicaID = new SelectList(db.TipoDificultadFisica, "ID", "Descripcion");
            ViewBag.TipoDificultadTecnicaID = new SelectList(db.TipoDificultadTecnica, "ID", "Descripcion");
            return View(sendero);
        }

        // GET: Sendero/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sendero Sendero = db.Sendero.Find(id);
            if (Sendero == null)
            {
                return HttpNotFound();
            }

            ViewBag.TipoDificultadFisicaID = new SelectList(db.TipoDificultadFisica, "ID", "Descripcion", Sendero.TipoDificultadFisicaID);
            ViewBag.TipoDificultadTecnicaID = new SelectList(db.TipoDificultadTecnica, "ID", "Descripcion", Sendero.TipoDificultadTecnicaID);

            return View(Sendero);
        }

        // POST: Sendero/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Sendero sendero, HttpPostedFileBase senderoImg, HttpPostedFileBase zipMapa)//[Bind(Include = "ID,Nombre,Descripcion,LugarInicio,LugarFin,TipoDificultadTecnicaID,TipoDificultadFisicaID,Desnivel,Distancia,AlturaMaxima,DuracionTotal")] 
        {
            sendero = JsonConvert.DeserializeObject<Sendero>(Request.Form["Sendero"]);
            //var x =Request.Files.Count;
            //var ssendero = Request.Form["Sendero"];
            //sendero.ID = 2;//dato de prueba

            ModelState["sendero"].Errors.Clear();
            UpdateModel<Sendero>(sendero);


            if (ModelState.IsValid)
            {

                #region Delete and Add "SenderoPunto"                
                if (db.SenderoPunto.Any(r => r.SenderoID == sendero.ID))
                {
                    //Delete old objects
                    db.SenderoPunto.RemoveRange(db.SenderoPunto.Where(r => r.SenderoID == sendero.ID));

                    //Add new objects
                    sendero.SenderoPunto.ToList().ForEach(r => r.SenderoID = sendero.ID);
                    db.SenderoPunto.AddRange(sendero.SenderoPunto);
                }
                else
                {
                    //Add new objects
                    sendero.SenderoPunto.ToList().ForEach(r => r.SenderoID = sendero.ID);
                    db.SenderoPunto.AddRange(sendero.SenderoPunto);
                }
                #endregion


                #region Delete and Add "SenderoPunto"  
                if (db.SenderoPuntoElevacion.Any(r => r.SenderoID == sendero.ID))
                {
                    //Delete old objects
                    db.SenderoPuntoElevacion.RemoveRange(db.SenderoPuntoElevacion.Where(r => r.SenderoID == sendero.ID));

                    //Add new objects
                    sendero.SenderoPuntoElevacion.ToList().ForEach(r => r.SenderoID = sendero.ID);
                    db.SenderoPuntoElevacion.AddRange(sendero.SenderoPuntoElevacion);
                }
                else
                {
                    //Add new objects
                    sendero.SenderoPuntoElevacion.ToList().ForEach(r => r.SenderoID = sendero.ID);
                    db.SenderoPuntoElevacion.AddRange(sendero.SenderoPuntoElevacion);
                }
                #endregion

                
                db.Entry(sendero).State = EntityState.Modified;
                db.SaveChanges();


                if (Request.Files.Count > 0)
                {
                    //Ej. ruta de recursos(imagen) de senderos: "~/Content/Senderos/2/Img/senderoImg_2.jpg"                                                
                    //Ej. ruta de recursos(mapa offline) de senderos: "~/Content/Senderos/2/Mapa/senderoMapa_2.zip"                          

                    //SenderoImg
                    #region process SenderoImg
                    if (senderoImg != null)
                    {
                        if (senderoImg.ContentLength > 0)
                        {
                            //Validate file extension
                            string fileExtension = Path.GetExtension(senderoImg.FileName);
                            if (new[] { ".jpg", ".png" }.Any(c => fileExtension == c))
                            {
                                //Create name and paths
                                string fileName = "senderoImg_" + sendero.ID + Path.GetExtension(senderoImg.FileName);  //Ej: "senderoiImg_2.jpg"
                                string virtualDirectoryPath = "~/Content/Senderos/" + sendero.ID + "/Img";              //Ej: "~/Content/Senderos/2/Img/"
                                string phisicaldirectoryPath = Server.MapPath(virtualDirectoryPath);                    //Ej: "F:/Sistemas/GitHub-Repositorios/TurApp-BackOffice/TurApp/MSP/Content/Senderos/2/Img"
                                string fullVirtualPath = virtualDirectoryPath + "/" + fileName;
                                string fullPhisicalPath = Path.Combine(phisicaldirectoryPath, fileName);                //Ej: "~/Content/Senderos/2/Img/senderoiImg_2.jpg"

                                //CreateDirectory
                                System.IO.Directory.CreateDirectory(phisicaldirectoryPath);

                                //Save file in a phisicaldirectoryPath
                                senderoImg.SaveAs(fullPhisicalPath);

                                //Update in DB Sendero file path        
                                sendero.RutaImagen = fullVirtualPath.Substring(1, fullVirtualPath.Length - 1);
                                db.SaveChanges();
                            }
                        }
                    }
                    #endregion

                    //ZipMapa
                    #region process ZipMapa
                    if (zipMapa != null)
                    {
                        if (zipMapa.ContentLength > 0)
                        {
                            //Validate file extension
                            string fileExtension = Path.GetExtension(senderoImg.FileName);
                            if (new[] { ".zip" }.Any(c => fileExtension == c))
                            {
                                //Create name and paths
                                string fileName = "senderoMapa_" + sendero.ID + Path.GetExtension(zipMapa.FileName);        //Ej: "senderoiMapa_2.jpg"
                                string virtualDirectoryPath = "~/Content/Senderos/" + sendero.ID + "/Mapa";                 //Ej: "~/Content/Senderos/2/Mapa/"
                                string phisicaldirectoryPath = Server.MapPath(virtualDirectoryPath);
                                string fullVirtualPath = Path.Combine(virtualDirectoryPath, fileName);
                                string fullPhisicalPath = Path.Combine(phisicaldirectoryPath, fileName);                    //Ej: "~/Content/Senderos/2/Img/senderoiImg_2.jpg"

                                //CreateDirectory
                                System.IO.Directory.CreateDirectory(phisicaldirectoryPath);

                                //Save file in a phisicaldirectoryPath
                                senderoImg.SaveAs(fullPhisicalPath);

                                //Update in DB Sendero file path            
                                sendero.RutZipMapa = fullVirtualPath;
                                db.SaveChanges();
                            }
                        }
                    }
                    #endregion

                }








                return Json(new
                {
                    ok = true
                });
            }


            ViewBag.TipoDificultadFisicaID = new SelectList(db.TipoDificultadFisica, "ID", "Descripcion", sendero.TipoDificultadFisicaID);
            ViewBag.TipoDificultadTecnicaID = new SelectList(db.TipoDificultadTecnica, "ID", "Descripcion", sendero.TipoDificultadTecnicaID);
            return View(sendero);
        }

        // GET: Sendero/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sendero Sendero = db.Sendero.Find(id);
            if (Sendero == null)
            {
                return HttpNotFound();
            }
            return View(Sendero);
        }

        // POST: Sendero/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sendero Sendero = db.Sendero.Find(id);
            db.Sendero.Remove(Sendero);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
