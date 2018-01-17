using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MSP_TurApp.Models;
using System.Transactions;

namespace MSP_TurApp.Controllers
{
    [Authorize]
    public class CentroDeSaludController : Controller
    {
        private MSPEntities db = new MSPEntities();

        // GET: CentroDeSalud
        public ActionResult Index()
        {
            var centroDeSalud = db.CentroDeSalud.Include(c => c.Localidad);
            return View(centroDeSalud.ToList());
        }

        // GET: CentroDeSalud/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CentroDeSalud centroDeSalud = db.CentroDeSalud.Find(id);
            if (centroDeSalud == null)
            {
                return HttpNotFound();
            }
            return View(centroDeSalud);
        }

        // GET: CentroDeSalud/Create
        public ActionResult Create()
        {
            ViewBag.LocalidadID = new SelectList(db.Localidad, "ID", "Nombre");
            return View();
        }

        // POST: CentroDeSalud/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nombre,Latitud,Longitud,Direccion,Telefono,EMail,URLImagenDelCentroDeSalud,LocalidadID,Activo,FechaUltimaActualizacion")] CentroDeSalud centroDeSalud)
        {
            if (ModelState.IsValid)
            {
                db.CentroDeSalud.Add(centroDeSalud);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocalidadID = new SelectList(db.Localidad, "ID", "Nombre", centroDeSalud.LocalidadID);
            return View(centroDeSalud);
        }

        // GET: CentroDeSalud/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CentroDeSalud centroDeSalud = db.CentroDeSalud.Find(id);
            if (centroDeSalud == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocalidadID = new SelectList(db.Localidad, "ID", "Nombre", centroDeSalud.LocalidadID);
            ViewBag.DepartamentoID = new SelectList(db.Departamento, "ID", "Nombre", centroDeSalud.Localidad.DepartamentoID);
            return View(centroDeSalud);
        }

        // POST: CentroDeSalud/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nombre,Latitud,Longitud,Direccion,Telefono,EMail,URLImagenDelCentroDeSalud,LocalidadID,Activo,FechaUltimaActualizacion")] CentroDeSalud centroDeSalud)
        {
            if (ModelState.IsValid)
            {
                db.Entry(centroDeSalud).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocalidadID = new SelectList(db.Localidad, "ID", "Nombre", centroDeSalud.LocalidadID);
            return View(centroDeSalud);
        }

        // GET: CentroDeSalud/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CentroDeSalud centroDeSalud = db.CentroDeSalud.Find(id);
            if (centroDeSalud == null)
            {
                return HttpNotFound();
            }
            return View(centroDeSalud);
        }

        // POST: CentroDeSalud/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            CentroDeSalud centroDeSalud = db.CentroDeSalud.Find(id);
            db.CentroDeSalud.Remove(centroDeSalud);
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




        // GET: CentroDeSalud/CreateEspecialidad/5
        [Route("CentroDeSalud/CreateEspecialidad/{csId}")]
        [HttpGet]
        public ActionResult CreateEspecialidad(short csId)
        {


            ViewBag.EspecialidadID = new SelectList(db.Especialidad, "ID", "Nombre");
            ViewBag.HorariosID = new SelectList(db.Horarios, "ID", "Hora");

            //EspecialidadPorCentroDeSalud
            //HorariosPorEspecialidadPorCentroDeSalud
            ViewBag.csId = csId;

            return PartialView();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("CentroDeSalud/CreateEspecialidad")]
        public ActionResult CreateEspecialidad(EspecialidadPorCentroDeSalud data)
        {

            //using (TransactionScope tran = new TransactionScope())
            //{

            //}
            try
            {
                var existeEspecialidad = db.CentroDeSalud.Find(data.CentroDeSaludID).EspecialidadPorCentroDeSalud.Any(r => r.EspecialidadID == data.EspecialidadID);

                if (existeEspecialidad)
                {
                    return Json(new {
                        ok = false,
                        msj = "La especialidad seleccionada ya esta asociada al Centro de Salud",
                    });
                }
                

                db.EspecialidadPorCentroDeSalud.Add(data);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e )
            {

                ViewBag.EspecialidadID = new SelectList(db.Especialidad, "ID", "Nombre");
                ViewBag.HorariosID = new SelectList(db.Horarios, "ID", "Hora");

                //EspecialidadPorCentroDeSalud
                //HorariosPorEspecialidadPorCentroDeSalud

                return PartialView();
            }



            //if (ModelState.IsValid)
            //{
            //    db.EspecialidadPorCentroDeSalud.Add(data);
            //    //db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            
        }



        // GET: CentroDeSalud/EditEspecialidad/5
        [Route("CentroDeSalud/EditEspecialidad/{EspecialidadPorCentroDeSaludID}")]
        [HttpGet]
        public ActionResult EditEspecialidad(int EspecialidadPorCentroDeSaludID)
        {
            var model = db.EspecialidadPorCentroDeSalud.Where(r => r.ID == EspecialidadPorCentroDeSaludID).FirstOrDefault();

            ViewBag.EspecialidadID = new SelectList(db.Especialidad, "ID", "Nombre");
            ViewBag.HorariosID = new SelectList(db.Horarios, "ID", "Hora");

            //EspecialidadPorCentroDeSalud
            //HorariosPorEspecialidadPorCentroDeSalud
            ViewBag.csId = model.CentroDeSaludID;

            return PartialView(model);
        }















        //Return Partial View
        [Route("CentroDeSalud/EspecialidadesPartial/{csId}")]
        public ActionResult EspecialidadesPartial(short csId)
        {
            var model = db.EspecialidadPorCentroDeSalud.Where(r => r.CentroDeSaludID == csId).ToList();
            return PartialView("_EspecialidadesPartial", model);

        }


    }
}
