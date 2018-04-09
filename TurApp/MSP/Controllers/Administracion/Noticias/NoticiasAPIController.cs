using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TurApp.Filters;
using TurApp.Models;

namespace TurApp.Controllers
{
    public class NoticiasAPIController : ApiController
    {
        private TurAppEntities db = new TurAppEntities();


        //public async Task<IHttpActionResult> GetSenderoGZip()
        [Route("api/Noticias")]
        [AllowAnonymous]
        //[CompressFilter]
        public async Task<IHttpActionResult> GetNoticias()
        {
                                    
            var client = new HttpClient();
            
                        
            var response = await client.GetAsync("https://sisanjuan.gob.ar/secciones/ministerio-de-turismo-y-cultura?format=json");

            var value = await response.Content.ReadAsStringAsync();

           
            //convert to json instance
            JObject obj = JObject.Parse(value);
            
            //return event array
            var token = (JArray)obj.SelectToken("items");

           
            var listaNoticias = new List<noticia>();

            foreach (var item in token.Take(10))
            {
                var json = JsonConvert.SerializeObject(item);
                listaNoticias.Add(JsonConvert.DeserializeObject<noticia>(json));
            }


            listaNoticias.ForEach(r=>r.image= "https://sisanjuan.gob.ar" + r.image);
            
            return Json(new { Noticias = listaNoticias });
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


    public class noticia
    {
        public string title { get; set; }

        public string fulltext { get; set; }

        public string image { get; set; }

        public string created { get; set; }
    }
}