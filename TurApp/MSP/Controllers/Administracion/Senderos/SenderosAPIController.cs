using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TurApp.Filters;
using TurApp.Models;

namespace TurApp.Controllers
{
    public class SenderosAPIController : ApiController
    {
        private TurAppEntities db = new TurAppEntities();

        // GET: api/SenderosAPI
        public IHttpActionResult GetSendero()
        {
            var data = db.Sendero.Select(r => new
            {
                r.ID,
                r.Nombre,
                r.Descripcion,
                r.InfoInteres,
                r.LugarInicio,
                r.LugarFin,
                r.Distancia,
                r.Desnivel,
                r.DuracionTotal,
                r.AlturaMaxima,
                //SenderoPunto = r.SenderoPunto.Select(x => new { x.Latitud, x.Longitud }),
                SenderoPuntoElevacion = r.SenderoPuntoElevacion.Select(x => new { x.Latitud, x.Longitud, x.Altura }),
                SenderoPuntoInteres = r.SenderoPuntoInteres.Select(x => new { x.Descripcion, x.Latitud, x.Longitud, x.TipoPuntoInteresID }),
                SenderoSector = new
                {
                    r.SenderoSector.ID,
                    r.SenderoSector.Nombre,
                    r.SenderoSector.NombreDepartamento,
                    r.SenderoSector.PesoZipMapa,
                    r.SenderoSector.RutaZipMapa,
                    r.SenderoSector.Version,
                },
                r.RutaImagen,
                r.RutZipMapa,
                TipoDificultadFisica = r.TipoDificultadFisica.Descripcion,
                TipoDificultadTecnica = r.TipoDificultadTecnica.Descripcion,
                r.ImgBase64
            });


            return Json(new { Senderos = data });
        }


        //public async Task<IHttpActionResult> GetSenderoGZip()
        [Route("api/SenderosGZip")]
        //[AllowAnonymous]
        [CompressFilter]
        public IHttpActionResult GetSenderoGZip()
        {

            var data = db.Sendero.Select(r => new
            {
                r.ID,
                r.Nombre,
                r.Descripcion,
                r.InfoInteres,
                r.LugarInicio,
                r.LugarFin,
                r.Distancia,
                r.Desnivel,
                r.DuracionTotal,
                r.AlturaMaxima,
                SenderoPuntoElevacion = r.SenderoPuntoElevacion.Select(x => new { x.Latitud, x.Longitud, x.Altura }),
                SenderoPuntoInteres = r.SenderoPuntoInteres.Select(x => new { x.Descripcion, x.Latitud, x.Longitud, x.TipoPuntoInteresID }),
                SenderoSector = new
                {
                    r.SenderoSector.ID,
                    r.SenderoSector.Nombre,
                    r.SenderoSector.NombreDepartamento,
                    r.SenderoSector.PesoZipMapa,
                    r.SenderoSector.RutaZipMapa,
                    r.SenderoSector.Version,
                },
                r.RutaImagen,
                r.RutZipMapa,
                TipoDificultadFisica = r.TipoDificultadFisica.Descripcion,
                TipoDificultadTecnica = r.TipoDificultadTecnica.Descripcion,
                r.ImgBase64
            }).OrderBy(r => r.SenderoSector.NombreDepartamento).ThenBy(n => n.SenderoSector.Nombre);

            //sendero.SenderoPunto.ToList().ForEach(r => r.SenderoID = sendero.ID);
            //data.ToList().ForEach(r => r.SenderoSector.FechaActualizacionTicks = r.SenderoSector.FechaActualizacion.Ticks);

            return Json(new { Senderos = data });
            //return Ok(data);
        }





        // GET: api/SenderosAPI/5
        [ResponseType(typeof(Sendero))]
        [CompressFilter]
        public IHttpActionResult GetSendero(int id)
        {
            //Sendero sendero = db.Sendero.Find(id);
            var data = db.Sendero.Where(x => x.ID == id).Select(r => new
            {
                r.ID,
                r.Nombre,
                r.Descripcion,
                r.InfoInteres,
                r.LugarInicio,
                r.LugarFin,
                r.Distancia,
                r.Desnivel,
                r.DuracionTotal,
                r.AlturaMaxima,
                //SenderoPunto = r.SenderoPunto.Select(x => new { x.Latitud, x.Longitud }),
                SenderoPuntoElevacion = r.SenderoPuntoElevacion.Select(x => new { x.Latitud, x.Longitud, x.Altura }),
                SenderoPuntoInteres = r.SenderoPuntoInteres.Select(x => new { x.Descripcion, x.Latitud, x.Longitud, x.TipoPuntoInteresID }),
                SenderoSector = new
                {
                    r.SenderoSector.ID,
                    r.SenderoSector.Nombre,
                    r.SenderoSector.NombreDepartamento,
                    r.SenderoSector.PesoZipMapa,
                    r.SenderoSector.RutaZipMapa,
                    r.SenderoSector.Version,
                },
                r.RutaImagen,
                r.RutZipMapa,
                TipoDificultadFisica = r.TipoDificultadFisica.Descripcion,
                TipoDificultadTecnica = r.TipoDificultadTecnica.Descripcion,
                r.ImgBase64

            });

            if (data == null)
            {
                return NotFound();
            }


            return Json(data);
        }



        // GET: /UpdateSenderosAPI
        //[HttpGet, ActionName("GetFechaActualizacion")]
        [Route("UpdateSenderosAPI")]
        public IHttpActionResult GetFechaActualizacion()
        {
            var data = db.RegistroActualizacion.Select(r => new
            {
                r.FechaActualizacion,
                r.Version,
            }).FirstOrDefault();


            return Json(new { FechaActualizacion = data.FechaActualizacion.Ticks, Version = data.Version });
        }



        // GET: /VersionSectorAPI/2
        [Route("VersionSectorAPI/{id}")]
        public IHttpActionResult GetFechaActualizacionSector(int id)
        {
            var Sector = db.SenderoSector.Find(id);

            if (Sector == null)
            {
                return NotFound();
            }


            return Json(new { Version = Sector.Version });
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