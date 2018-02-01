using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Sendero Sendero)//[Bind(Include = "ID,Nombre,Descripcion,LugarInicio,LugarFin,TipoDificultadTecnicaID,TipoDificultadFisicaID,Desnivel,Distancia,AlturaMaxima,DuracionTotal")] 
        {

            try
            {
                if (ModelState.IsValid)
                {
                    db.Sendero.Add(Sendero);
                    db.SaveChanges();
                    return Json(new {
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
            return View(Sendero);
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

            ViewBag.TipoDificultadFisicaID = new SelectList(db.TipoDificultadFisica, "ID", "Descripcion",Sendero.TipoDificultadFisicaID);
            ViewBag.TipoDificultadTecnicaID = new SelectList(db.TipoDificultadTecnica, "ID", "Descripcion", Sendero.TipoDificultadTecnicaID);

            return View(Sendero);
        }

        // POST: Sendero/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Sendero Sendero)//[Bind(Include = "ID,Nombre,Descripcion,LugarInicio,LugarFin,TipoDificultadTecnicaID,TipoDificultadFisicaID,Desnivel,Distancia,AlturaMaxima,DuracionTotal")] 
        {
            if (ModelState.IsValid)
            {
                //var SenderoEntidad = db.Sendero.Find(Sendero.ID);

                if (db.SenderoPunto.Any(r => r.SenderoID == Sendero.ID))
                {
                    //Delete old objects
                    db.SenderoPunto.RemoveRange(db.SenderoPunto.Where(r => r.SenderoID == Sendero.ID));

                    //Add new objects
                    Sendero.SenderoPunto.ToList().ForEach(r => r.SenderoID = Sendero.ID);
                    db.SenderoPunto.AddRange(Sendero.SenderoPunto);
                }
                else
                {
                    //Add new objects
                    Sendero.SenderoPunto.ToList().ForEach(r => r.SenderoID = Sendero.ID);
                    db.SenderoPunto.AddRange(Sendero.SenderoPunto);
                }

                if (db.SenderoPuntoElevacion.Any(r => r.SenderoID == Sendero.ID))
                {
                    //Delete old objects
                    db.SenderoPuntoElevacion.RemoveRange(db.SenderoPuntoElevacion.Where(r => r.SenderoID == Sendero.ID));

                    //Add new objects
                    Sendero.SenderoPuntoElevacion.ToList().ForEach(r => r.SenderoID = Sendero.ID);
                    db.SenderoPuntoElevacion.AddRange(Sendero.SenderoPuntoElevacion);
                }
                else
                {
                    //Add new objects
                    Sendero.SenderoPuntoElevacion.ToList().ForEach(r => r.SenderoID = Sendero.ID);
                    db.SenderoPuntoElevacion.AddRange(Sendero.SenderoPuntoElevacion);
                }




                db.Entry(Sendero).State = EntityState.Modified;
                db.SaveChanges();
                //return 
                return Json(new
                {
                    ok = true
                });
                //return RedirectToAction("Index");
            }


            ViewBag.TipoDificultadFisicaID = new SelectList(db.TipoDificultadFisica, "ID", "Descripcion");
            ViewBag.TipoDificultadTecnicaID = new SelectList(db.TipoDificultadTecnica, "ID", "Descripcion");
            return View(Sendero);
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
