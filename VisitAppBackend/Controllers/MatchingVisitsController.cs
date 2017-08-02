﻿using System.Net;
using System.Net.Http;
using System.Web.Http;
using VisitAppBackend.Models;
using VisitAppBackend.Services;

namespace VisitAppBackend.Controllers
{
	public class MatchingVisitsController : ApiController
	{
		private IVisitsService visitsService = new VisitsService();

		// GET api/matchingvisits
		public HttpResponseMessage Get(string idFacebook = "", string accessToken = "", string idsFacebookFriend = "", string idPlace = "", string date = "", int startHour = -1, int endHour = -1)
		{
			HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

			if (visitsService.ValidateFacebookAccessToken(idFacebook, accessToken))
			{
				var matchingVisits = visitsService.GetMatchingVisits(idsFacebookFriend, idPlace, date, startHour, endHour);

				if (matchingVisits == null)
				{
					httpResponseMessage.StatusCode = HttpStatusCode.BadRequest;
				}
				else
				{
					httpResponseMessage.Content = new ObjectContent<MatchingVisits>(matchingVisits, Configuration.Formatters.JsonFormatter);
					httpResponseMessage.StatusCode = HttpStatusCode.OK;
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
