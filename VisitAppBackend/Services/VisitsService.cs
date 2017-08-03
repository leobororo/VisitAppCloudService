using System;
using System.Collections.Generic;
using System.Globalization;
using VisitAppBackend.Models;
using VisitAppBackend.Repositories;
using VisitAppBackend.Utils;

namespace VisitAppBackend.Services
{
    public class VisitsService : IVisitsService
    {
        private IVisitsRepository visitsRepository = new VisitsRepository();

        /// <summary>
        /// Validates the facebook access token.
        /// </summary>
        /// <returns><c>true</c>, if facebook access token was validated, <c>false</c> otherwise.</returns>
        /// <param name="idFacebook">Identifier facebook.</param>
        /// <param name="accessToken">Access token.</param>
		public bool ValidateFacebookAccessToken(string idFacebook, string accessToken)
		{
			return visitsRepository.ValidateFacebookAccessToken(idFacebook, accessToken);
		}

        /// <summary>
        /// Devolve um objeto do tipo MatchingVisits construído com informações retornada pelo repository
        /// </summary>
        /// <param name="idsFacebook">Um string contendo uma lista de ids do facebook separados por vírgula</param>
        /// <param name="idPlace">Um string contendo o id de um Google Place</param>
        /// <param name="date">Uma string representando uma data no formato dd/MM/yyyy</param>
        /// <param name="startHour">Um número representando a hora início da visita</param>
        /// <param name="endHour">Um número representando a hora fim da visita</param>
        /// <returns>MatchingVisits</returns>
        public MatchingVisits GetMatchingVisits(string idsFacebook, string idPlace, string date, int startHour, int endHour)
        {
			MatchingVisits matchingVisits = null;

			if (Util.isQueryParametersForMatchingValid(idsFacebook, idPlace, date))
			{
				matchingVisits = new MatchingVisits();

				string[] ids = idsFacebook.Split(',');

				ICollection<Visit> sameDayVisits;
				ICollection<Visit> sameTimeVisits;
				foreach (var id in ids)
				{
					sameDayVisits = visitsRepository.GetMatchingVisits(id, idPlace, date);

					if (sameDayVisits.Count > 0)
					{
						foreach (var visit in sameDayVisits)
						{
							matchingVisits.SameDayVisits.Add(visit);
						}

						sameTimeVisits = GenerateListOfSameTimeVisits(sameDayVisits, startHour, endHour);

						foreach (var visit in sameTimeVisits)
						{
							matchingVisits.SameTimeVisits.Add(visit);
						}
					}
				}
			}

			return matchingVisits;
        }

        /// <summary>
        /// Devolve todas as visitas de um determinado usuário do facebook
        /// </summary>
        /// <param name="idFacebook">Uma string representando o id do facebook do usuário</param>
        /// <returns>ICollection<Visit></returns>
        public ICollection<Visit> GetUserVisits(string idFacebook)
        {
			ICollection<Visit> userVisits;

			if (String.IsNullOrEmpty(idFacebook))
			{
				userVisits = null;
			}
			else
			{
				userVisits = new List<Visit>();
				ICollection<Visit> allCurrentUserVisits = visitsRepository.GetUserVisits(idFacebook);
				DateTime today = DateTime.Today;

				foreach (var visit in allCurrentUserVisits)
				{
					DateTimeFormatInfo brDtfi = new CultureInfo("pt-BR", false).DateTimeFormat;
					DateTime visitDate = Convert.ToDateTime(visit.DataVisita, brDtfi);

					if (visitDate.CompareTo(today) < 0)
					{
						if (!DeleteVisit(visit.ObjectId))
						{
							userVisits = null;
							break;
						}
					}
					else
					{
						userVisits.Add(visit);
					}
				}
			}

			return userVisits;
        }

        /// <summary>
        /// Remove a visita cujo id é igual ao especificado
        /// </summary>
        /// <param name="id">Uma string representando o id da visita</param>
        /// <returns>true caso a visita tenha sido removida, false caso contrário</returns>
        public bool DeleteVisit(string id)
        {
            return visitsRepository.DeleteVisit(id);
        }

        /// <summary>
        /// Salva a visita especificada, devolve uma nova visita caso esta tenha sido salva com sucesso, 
        /// uma visita sem identificador caso já exista uma visita salva para o mesmo local, data e horário
        /// e uma visita nula caso não tenha sido possível salvar a visita
        /// </summary>
        /// <param name="visit">Visit a ser salva</param>
        /// <returns>Visit</returns>
        public Visit PostVisit(Visit visit)
        {
            Visit newVisit = null;
            if (Util.isTimeBoxValid(visit))
            {
                newVisit = visit;

                MatchingVisits matchingVisits = GetMatchingVisits(visit.IdFacebook, visit.PlaceId, visit.DataVisita, visit.HoraInicioVisita, visit.HoraFimVisita);
                if (matchingVisits.SameTimeVisits.Count == 0)
                {
                    newVisit = visitsRepository.PostVisit(visit);
                    if (newVisit != null)
                    {
                        Util.copyVisitData(visit, newVisit);
                    }
                }
            }

            return newVisit;
        }

        /// <summary>
        /// Recebe uma lista de visitas, um horário inicial e um horário final. Devolve uma lista de visitas contendo as visitas recebidas como parâmetro que ocorram em horários parecidos
        /// </summary>
        /// <param name="sameDayVisits">Lista de visitas que será filtrada de acordo com hora início e hora fim</param>
        /// <param name="startHour">Um número representando uma hora inicial</param>
        /// <param name="endHour">Um número representando uma hora final</param>
        /// <returns></returns>
        private ICollection<Visit> GenerateListOfSameTimeVisits(ICollection<Visit> sameDayVisits, int startHour, int endHour)
        {
            ICollection<Visit> sameTimeVisits = new List<Visit>();

            foreach (var visit in sameDayVisits)
            {
                if ((startHour.CompareTo(visit.HoraInicioVisita) >= 0 && startHour.CompareTo(visit.HoraFimVisita) <= 0)
                    || (endHour.CompareTo(visit.HoraInicioVisita) >= 0 && endHour.CompareTo(visit.HoraFimVisita) <= 0))
                {
                    sameTimeVisits.Add(visit);
                }
            }

            return sameTimeVisits;
        }
    }
}