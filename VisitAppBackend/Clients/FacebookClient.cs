using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Teste.Exceptions;
using VisitAppBackend.Models;

namespace VisitAppBackend.Clients
{
    public class FacebookClient : IFacebookClient
    {
		public void ValidateFacebookAccessToken(string idFacebook, string accessToken)
		{
			if (!Task.Run(() => ValidateFacebookAccessTokenTask(idFacebook, accessToken)).Result)
			{
				throw new InvalidTokenException(String.Format("Invalid token for facebook id {0}", idFacebook));
			}
		}

		private async Task<bool> ValidateFacebookAccessTokenTask(string idFacebook, String accessToken)
		{
			HttpClient httpClient = new HttpClient();

			httpClient.DefaultRequestHeaders.Accept.Clear();
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var serializer = new DataContractJsonSerializer(typeof(Visits));
			HttpResponseMessage response = await httpClient.GetAsync("https://graph.facebook.com/" + idFacebook + "/permissions?access_token=" + accessToken);

			return response.IsSuccessStatusCode;
		}
    }
}
