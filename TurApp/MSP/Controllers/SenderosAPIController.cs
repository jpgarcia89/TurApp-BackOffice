using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TurApp.Models;

namespace TurApp.Controllers
{
    public class SenderosAPIController : ApiController
    {
        private TurAppEntities db = new TurAppEntities();

        // GET: api/SenderosAPI
        public IHttpActionResult GetSendero()
        {
            var data = db.Sendero.Select(r =>  new {
                r.ID,
                r.Nombre,
                r.Descripcion,
                r.LugarInicio,
                r.LugarFin,
                r.Distancia,
                r.Desnivel,
                r.DuracionTotal,
                r.AlturaMaxima,
                //SenderoPunto = r.SenderoPunto.Select(x => new { x.Latitud, x.Longitud }),
                SenderoPuntoElevacion = r.SenderoPuntoElevacion.Select(x => new { x.Latitud, x.Longitud, x.Altura }),
                r.RutaImagen,
                r.RutZipMapa,
                TipoDificultadFisicaID = r.TipoDificultadFisica.Descripcion,
                TipoDificultadTecnica = r.TipoDificultadTecnica.Descripcion
            });


            return Json(new {Senderos = data});
        }

        // GET: api/SenderosAPI/5
        [ResponseType(typeof(Sendero))]
        public IHttpActionResult GetSendero(int id)
        {
            //Sendero sendero = db.Sendero.Find(id);
            var data = db.Sendero.Where(x => x.ID == id).Select(r => new {
                r.ID,
                r.Nombre,
                r.Descripcion,
                r.LugarInicio,
                r.LugarFin,
                r.Distancia,
                r.Desnivel,
                r.DuracionTotal,
                r.AlturaMaxima,
                //SenderoPunto = r.SenderoPunto.Select(x => new { x.Latitud, x.Longitud }),
                SenderoPuntoElevacion = r.SenderoPuntoElevacion.Select(x => new { x.Latitud, x.Longitud, x.Altura }),
                r.RutaImagen,
                r.RutZipMapa,
                TipoDificultadFisicaID = r.TipoDificultadFisica.Descripcion,
                TipoDificultadTecnica = r.TipoDificultadTecnica.Descripcion
            });

            if (data == null)
            {
                return NotFound();
            }

            
            return Json(data);
        }

        // PUT: api/SenderosAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSendero(int id, Sendero sendero)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sendero.ID)
            {
                return BadRequest();
            }

            db.Entry(sendero).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SenderoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/SenderosAPI
        [ResponseType(typeof(Sendero))]
        public IHttpActionResult PostSendero(Sendero sendero)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sendero.Add(sendero);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sendero.ID }, sendero);
        }

        // DELETE: api/SenderosAPI/5
        [ResponseType(typeof(Sendero))]
        public IHttpActionResult DeleteSendero(int id)
        {
            Sendero sendero = db.Sendero.Find(id);
            if (sendero == null)
            {
                return NotFound();
            }

            db.Sendero.Remove(sendero);
            db.SaveChanges();

            return Ok(sendero);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SenderoExists(int id)
        {
            return db.Sendero.Count(e => e.ID == id) > 0;
        }
    }
}