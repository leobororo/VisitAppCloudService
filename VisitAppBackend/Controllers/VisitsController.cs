using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VisitAppBackend.Exceptions;
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

		    try
		    {		                
                var visits = visitsService.GetUserVisits(idFacebook, accessToken);

	            httpResponseMessage.Content = new ObjectContent<ICollection<Visit>>(visits, Configuration.Formatters.JsonFormatter);
	            httpResponseMessage.StatusCode = HttpStatusCode.OK;   
		    }
		    catch (InvalidTokenException e)
		    {
		        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e.Message));
		    }
			catch (VisitNotFoundException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, e.Message));
			}

			return httpResponseMessage;
		}

		// POST api/visits
		public HttpResponseMessage Post(Visit visit, string idFacebook = "", string accessToken = "")
		{
			HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

			try
			{
				Visit newVisit = visitsService.CreateVisit(idFacebook, accessToken, visit);

	            httpResponseMessage.Content = new ObjectContent<Visit>(newVisit, Configuration.Formatters.JsonFormatter);
	            httpResponseMessage.StatusCode = HttpStatusCode.Created;
			}
		    catch (InvalidTokenException e)
		    {
		        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e.Message));
			}
            catch (CouldNotCreateVisitException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message));
			}
            catch (MatchingVisitException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, e.Message));
			}

			return httpResponseMessage;
		}

		// DELETE api/visits
		public HttpResponseMessage Delete(string id, string idFacebook = "", string accessToken = "")
		{
			HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
		    
			try
			{
                visitsService.DeleteVisit(idFacebook, accessToken, id);
				httpResponseMessage.StatusCode = HttpStatusCode.NoContent;
		    }
		    catch (InvalidTokenException e)
		    {
		        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e.Message));
		    }
		    catch (VisitNotFoundException e)
		    {
		        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, e.Message));
		    }

			return httpResponseMessage;
		}
	}
}
