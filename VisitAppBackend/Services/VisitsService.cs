using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Web;
using VisitAppBackend.Models;
using VisitAppBackend.Repositories;

namespace VisitAppBackend.Services
{
    public class VisitsService : IVisitsService
    {
        private IVisitsRepository visitsRepository = new VisitsRepository();

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

            if (validateQueryParameters(idsFacebook, idPlace, date))
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

                        if (isTimeParametersValid(startHour, endHour))
                        {
                            sameTimeVisits = GenerateListOfSameTimeVisits(sameDayVisits, startHour, endHour);

                            foreach (var visit in sameTimeVisits)
                            {
                                matchingVisits.SameTimeVisits.Add(visit);
                            }
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
            } else
            {
                userVisits = visitsRepository.GetUserVisits(idFacebook);
            }

            return userVisits;
        }

        /// <summary>
        /// Remove a visita cujo id é igual ao especificado
        /// </summary>
        /// <param name="id">Uma string representando o id da visita</param>
        /// <returns></returns>
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
            Visit newVisit = visit;

            MatchingVisits matchingVisits = GetMatchingVisits(visit.IdFacebook, visit.PlaceId, visit.DataVisita, visit.HoraInicioVisita, visit.HoraFimVisita);

            if (matchingVisits.SameTimeVisits.Count == 0)
            {
                newVisit = visitsRepository.PostVisit(visit);

                if (newVisit != null)
                {
                    copyVisitData(visit, newVisit);
                }
            }

            return newVisit;
        }

        /// <summary>
        /// Copia os dados de uma visita para outra
        /// </summary>
        /// <param name="fromVisit">Visit de onde os dados serão copiados</param>
        /// <param name="toVisit">Visit para onde os dados serão copiados</param>
        private void copyVisitData(Visit fromVisit, Visit toVisit)
        {
            toVisit.AcompanharAmigos = fromVisit.AcompanharAmigos;
            toVisit.DataVisita = fromVisit.DataVisita;
            toVisit.EnderecoPlace = fromVisit.EnderecoPlace;
            toVisit.HoraFimVisita = fromVisit.HoraFimVisita;
            toVisit.HoraInicioVisita = fromVisit.HoraInicioVisita;
            toVisit.IdFacebook = fromVisit.IdFacebook;
            toVisit.Latitude = fromVisit.Latitude;
            toVisit.Longitude = fromVisit.Longitude;
            toVisit.MinutoFimVisita = fromVisit.MinutoFimVisita;
            toVisit.MinutoInicioVisita = fromVisit.MinutoInicioVisita;
            toVisit.NomePlace = fromVisit.NomePlace;
            toVisit.PlaceId = fromVisit.PlaceId;
            toVisit.UpdatedAt = toVisit.CreatedAt;
        }

        /// <summary>
        /// Verifica se todas as strings são não nulas e não vazias
        /// </summary>
        /// <param name="idFacebook">Uma string</param>
        /// <param name="idPlace">Uma string</param>
        /// <param name="date">Um string</param>
        /// <returns>true caso todas as strings sejam válidas, false caso contrário</returns>
        private bool validateQueryParameters(String idFacebook, string idPlace, string date)
        {
            return !(String.IsNullOrEmpty(idFacebook) || String.IsNullOrEmpty(idPlace) || String.IsNullOrEmpty(date));
        }

        /// <summary>
        /// Verifica se os números representam horas válidas
        /// </summary>
        /// <param name="startHour">Número representando uma hora</param>
        /// <param name="endHour">Número representando uma hora</param>
        /// <returns>true caso os dois números representem horas, false caso contrário</returns>
        private bool isTimeParametersValid(int startHour, int endHour)
        {
            return isValidTime(startHour) && isValidTime(endHour);
        }

        /// <summary>
        /// Verifica se um número representa um horário válido
        /// </summary>
        /// <param name="time">Número representando um horário</param>
        /// <returns>true caso o número represente um horário, false caso contrário</returns>
        private bool isValidTime(int time)
        {
            return time > -1 && time < 24;
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