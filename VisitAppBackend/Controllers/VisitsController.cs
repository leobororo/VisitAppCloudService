using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VisitAppBackend.Models;
using VisitAppBackend.Services;

namespace VisitAppBackend.Controllers
{
    public class VisitsController : ApiController
    {
        private IVisitsService visitsService = new VisitsService();

        // GET api/visits?5
        public HttpResponseMessage Get(string idFacebook = "")
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            var visits = visitsService.GetUserVisits(idFacebook);

            if (visits == null)
            {
                httpResponseMessage.StatusCode = HttpStatusCode.BadRequest;
            } else
            {
                httpResponseMessage.Content = new ObjectContent<ICollection<Visit>>(visits, Configuration.Formatters.JsonFormatter);
                httpResponseMessage.StatusCode = HttpStatusCode.OK;
            }

            return httpResponseMessage;
        }

        // POST api/visits
        public HttpResponseMessage Post(Visit visit)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            Visit newVisit = visitsService.PostVisit(visit);

            if (newVisit != null)
            {
                if (String.IsNullOrEmpty(newVisit.ObjectId))
                {
                    httpResponseMessage.StatusCode = HttpStatusCode.Conflict;
                }
                else
                {
                    httpResponseMessage.Content = new ObjectContent<Visit>(newVisit, Configuration.Formatters.JsonFormatter);
                    httpResponseMessage.StatusCode = HttpStatusCode.Created;
                }
            } else
            {
                httpResponseMessage.StatusCode = HttpStatusCode.BadRequest;
            }

            return httpResponseMessage;
        }

        // DELETE api/visits/{id}
        public HttpResponseMessage Delete(string id)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            if (visitsService.DeleteVisit(id))
            {
                httpResponseMessage.StatusCode = HttpStatusCode.NoContent;
            }
            else
            {
                httpResponseMessage.StatusCode = HttpStatusCode.NotFound;
            }

            return httpResponseMessage;
        }
    }
}
