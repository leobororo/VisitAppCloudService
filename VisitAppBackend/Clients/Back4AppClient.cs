using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using VisitAppBackend.Exceptions;
using VisitAppBackend.Models;

namespace VisitAppBackend.Clients
{
    public class Back4AppClient : IBack4AppClient
    {
        private const String BASE_URL = "https://parseapi.back4app.com/classes/visita";
        private const String PARSE_APPLICATION_ID = "32FhpZF70irk91ykDXOmAL6at1aqcBq1GppYDMuU";
        private const String PARSE_REST_API_KEY = "l1WTcbgboH6edRdUuu0ldW2H1yIdUGVg2F48Yykd";

        public ICollection<Visit> GetMatchingVisits(string idFacebook, string idPlace, string date)
        {
            return Task.Run(() => GetMatchingVisitsTask(idFacebook, idPlace, date)).Result;
        }

        public ICollection<Visit> GetUserVisits(string idFacebook)
        {
            return Task.Run(() => GetUserVisitsTask(idFacebook)).Result;
        }

        public Visit CreateVisit(Visit visit)
        {
            Visit visitCreated = Task.Run(() => PostVisitTask(visit)).Result;

			if (visitCreated == null)
			{
                throw new CouldNotCreateVisitException(String.Format("Could not create visit"));
			}

            return visitCreated;
        }

        public void DeleteVisit(string id)
        {
            if (!Task.Run(() => DeleteVisitTask(id)).Result) {
                throw new VisitNotFoundException(String.Format("Could not find visit with id {0}", id));
            }
        }

        private async Task<ICollection<Visit>> GetUserVisitsTask(string idFacebook)
        {
            HttpClient httpClient = GetBack4AppHttpClient();

            var serializer = new DataContractJsonSerializer(typeof(Visits));
            var streamTask = httpClient.GetStreamAsync(BASE_URL + "?where={\"id_facebook\":\"" + idFacebook + "\"}");
            var repositories = serializer.ReadObject(await streamTask) as Visits;

            List<Visit> visits = repositories.results;
            foreach (var visit in visits)
                Console.WriteLine(visit.NomePlace);

            return visits;
        }

        private async Task<ICollection<Visit>> GetMatchingVisitsTask(string idFacebook, string idPlace, string date)
        {
            var httpClient = GetBack4AppHttpClient();


            var serializer = new DataContractJsonSerializer(typeof(Visits));
            var streamTask = httpClient.GetStreamAsync(BASE_URL + "?where={\"id_facebook\":\"" + idFacebook + "\", \"id_place\":\"" + idPlace + "\", \"data_visita\":\"" + date + "\"}");
            var repositories = serializer.ReadObject(await streamTask) as Visits;

            List<Visit> visits = repositories.results;
            foreach (var visit in visits)
                Console.WriteLine(visit.NomePlace);

            return visits;
        }

        private async Task<Visit> PostVisitTask(Visit visit)
        {
            var httpClient = GetBack4AppHttpClient();

            var serializer = new DataContractJsonSerializer(typeof(Visit));
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, visit);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            string str = Encoding.UTF8.GetString(dataBytes);

            HttpResponseMessage response = await httpClient.PostAsync(BASE_URL, new StringContent(str,  Encoding.UTF8,  "application/json"));

            Visit newVisit = null;
            if (response.StatusCode.Equals(HttpStatusCode.Created))
            {
                newVisit = serializer.ReadObject(await response.Content.ReadAsStreamAsync()) as Visit;
            }

            return newVisit;
        }

        private async Task<bool> DeleteVisitTask(string id)
        {
            var httpClient = GetBack4AppHttpClient();

            HttpResponseMessage response = await httpClient.DeleteAsync(BASE_URL + "/" + id);

            return response.StatusCode.Equals(HttpStatusCode.OK);
        }

        /// <summary>
        /// Instancia um HttpClient para acessar o BaaS
        /// </summary>
        /// <returns>HttpClient</returns>
        private static HttpClient GetBack4AppHttpClient()
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("X-Parse-Application-Id", PARSE_APPLICATION_ID);
            httpClient.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", PARSE_REST_API_KEY);

            return httpClient;
        }
    }
}