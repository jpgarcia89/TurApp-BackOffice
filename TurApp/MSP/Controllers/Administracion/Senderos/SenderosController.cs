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

        // GET: Senderos
        public ActionResult Index()
        {
            return View(db.Senderos.ToList());
        }

        // GET: Senderos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Senderos senderos = db.Senderos.Find(id);
            if (senderos == null)
            {
                return HttpNotFound();
            }
            return View(senderos);
        }

        // GET: Senderos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Senderos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nombre,Descripcion,LugarInicio,LugarFin,TipoDificultadTecnicaID,TipoDificultadFisicaID,Desnivel,Distancia,AlturaMaxima,DuracionTotal")] Senderos senderos)
        {
            if (ModelState.IsValid)
            {
                db.Senderos.Add(senderos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(senderos);
        }

        // GET: Senderos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Senderos senderos = db.Senderos.Find(id);
            if (senderos == null)
            {
                return HttpNotFound();
            }
            return View(senderos);
        }

        // POST: Senderos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nombre,Descripcion,LugarInicio,LugarFin,TipoDificultadTecnicaID,TipoDificultadFisicaID,Desnivel,Distancia,AlturaMaxima,DuracionTotal")] Senderos senderos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(senderos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(senderos);
        }

        // GET: Senderos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Senderos senderos = db.Senderos.Find(id);
            if (senderos == null)
            {
                return HttpNotFound();
            }
            return View(senderos);
        }

        // POST: Senderos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Senderos senderos = db.Senderos.Find(id);
            db.Senderos.Remove(senderos);
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
