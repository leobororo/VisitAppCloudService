using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VisitAppBackend.Models;
using VisitAppBackend.Services;

namespace VisitAppBackend.Controllers
{
	public class VisitsController : ApiController
	{
		private IVisitsService visitsService = new VisitsService();

		// GET api/visits/5
		public HttpResponseMessage Get(string idFacebook = "", string accessToken = "")
		{
			HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

			if (visitsService.ValidateFacebookAccessToken(idFacebook, accessToken))
			{
				var visits = visitsService.GetUserVisits(idFacebook);

				if (visits == null)
				{
					httpResponseMessage.StatusCode = HttpStatusCode.BadRequest;
				}
				else
				{
					httpResponseMessage.Content = new ObjectContent<ICollection<Visit>>(visits, Configuration.Formatters.JsonFormatter);
					httpResponseMessage.StatusCode = HttpStatusCode.OK;
				}
			}
			else
			{
				httpResponseMessage.StatusCode = HttpStatusCode.Unauthorized;
			}


			return httpResponseMessage;
		}

		// POST api/visits
		public HttpResponseMessage Post(Visit visit, string idFacebook = "", string accessToken = "")
		{
			HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

			if (visitsService.ValidateFacebookAccessToken(idFacebook, accessToken))
			{
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
				}
				else
				{
					httpResponseMessage.StatusCode = HttpStatusCode.BadRequest;
				}
			}
			else
			{
				httpResponseMessage.StatusCode = HttpStatusCode.Unauthorized;
			}

			return httpResponseMessage;
		}

		// DELETE api/visits
		public HttpResponseMessage Delete(string id, string idFacebook = "", string accessToken = "")
		{
			HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

			if (visitsService.ValidateFacebookAccessToken(idFacebook, accessToken))
			{
				if (visitsService.DeleteVisit(id))
				{
					httpResponseMessage.StatusCode = HttpStatusCode.NoContent;
				}
				else
				{
					httpResponseMessage.StatusCode = HttpStatusCode.NotFound;
				}
			}
			else
			{
				httpResponseMessage.StatusCode = HttpStatusCode.Unauthorized;
			}

			return httpResponseMessage;
		}
	}
}
