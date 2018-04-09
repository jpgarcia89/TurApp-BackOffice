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
    public class GuiasAPIController : ApiController
    {
        private TurAppEntities db = new TurAppEntities();

   
        //public async Task<IHttpActionResult> GetSenderoGZip()
        [Route("api/Guias")]
        [AllowAnonymous]
        [CompressFilter]
        public IHttpActionResult GetGuia()
        {

            var data = db.Guia.Select(r => new
            {
                r.ID,
                r.Nombre,
                r.Telefono
                
            }).OrderBy(r => r.Nombre);

            //sendero.SenderoPunto.ToList().ForEach(r => r.SenderoID = sendero.ID);
            //data.ToList().ForEach(r => r.SenderoSector.FechaActualizacionTicks = r.SenderoSector.FechaActualizacion.Ticks);

            return Json(new { Guias = data });
            //return Ok(data);
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
            return db.Guia.Count(e => e.ID == id) > 0;
        }
    }
}