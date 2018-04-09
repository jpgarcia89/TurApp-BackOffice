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
    public class GuiasController : Controller
    {
        private TurAppEntities db = new TurAppEntities();

        // GET: Guias
        public ActionResult Index()
        {
            return View(db.Guia.ToList());
        }

        // GET: Guias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guia guia = db.Guia.Find(id);
            if (guia == null)
            {
                return HttpNotFound();
            }
            return View(guia);
        }

        // GET: Guias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Guias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nombre,Telefono")] Guia guia)
        {
            if (ModelState.IsValid)
            {
                db.Guia.Add(guia);
                db.SaveChanges();
                return Json(new
                {
                    ok = true
                });
            }

            return View(guia);
        }

        // GET: Guias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guia guia = db.Guia.Find(id);
            if (guia == null)
            {
                return HttpNotFound();
            }
            return View(guia);
        }

        // POST: Guias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nombre,Telefono")] Guia guia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guia).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new
                {
                    ok = true
                });
            }
            return View(guia);
        }

        // GET: Guias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guia guia = db.Guia.Find(id);
            if (guia == null)
            {
                return HttpNotFound();
            }
            return View(guia);
        }

        // POST: Guias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Guia guia = db.Guia.Find(id);
            db.Guia.Remove(guia);
            db.SaveChanges();
            return Json(new
            {
                ok = true
            });
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
