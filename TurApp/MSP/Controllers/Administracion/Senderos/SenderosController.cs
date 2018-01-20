using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TurApp.Models;

namespace TurApp.Controllers
{
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
            
            if (ModelState.IsValid)
            {
                db.Sendero.Add(Sendero);
                //db.SaveChanges();
                return RedirectToAction("Index");
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

            ViewBag.TipoDificultadFisicaID = new SelectList(db.TipoDificultadFisica, "ID", "Descripcion");
            ViewBag.TipoDificultadTecnicaID = new SelectList(db.TipoDificultadTecnica, "ID", "Descripcion");

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
                db.Entry(Sendero).State = EntityState.Modified;
                db.SaveChanges();
                //return 
                RedirectToAction("Index");
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
