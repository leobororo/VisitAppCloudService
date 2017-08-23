﻿using System.Net;
using System.Net.Http;
using System.Web.Http;
using Teste.Exceptions;
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

			try
			{
				var matchingVisits = visitsService.GetMatchingVisits(idFacebook, accessToken, idsFacebookFriend, idPlace, date, startHour, endHour);

				httpResponseMessage.Content = new ObjectContent<MatchingVisits>(matchingVisits, Configuration.Formatters.JsonFormatter);
				httpResponseMessage.StatusCode = HttpStatusCode.OK;
			}
			catch (InvalidTokenException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e.Message));
			}
            catch (InvalidMatchingVisitsParametersException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message));
            }

			return httpResponseMessage;
		}
	}
}
