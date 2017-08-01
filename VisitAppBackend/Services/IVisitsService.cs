using System.Collections.Generic;
using VisitAppBackend.Models;

namespace VisitAppBackend.Services
{
    interface IVisitsService
    {
        /// <summary>
        /// Devolve um objeto do tipo MatchingVisits construído com informações retornada pelo repository
        /// </summary>
        /// <param name="idsFacebook">Um string contendo uma lista de ids do facebook separados por vírgula</param>
        /// <param name="idPlace">Um string contendo o id de um Google Place</param>
        /// <param name="date">Uma string representando uma data no formato dd/MM/yyyy</param>
        /// <param name="startHour">Um número representando a hora início da visita</param>
        /// <param name="endHour">Um número representando a hora fim da visita</param>
        /// <returns>MatchingVisits</returns>
        MatchingVisits GetMatchingVisits(string idFacebook, string idPlace, string date, int horaInicio, int horaFim);

        /// <summary>
        /// Devolve todas as visitas de um determinado usuário do facebook
        /// </summary>
        /// <param name="idFacebook">Uma string representando o id do facebook do usuário</param>
        /// <returns>ICollection<Visit></returns>
        ICollection<Visit> GetUserVisits(string idFacebook);

        /// <summary>
        /// Salva a visita especificada, devolve uma nova visita caso esta tenha sido salva com sucesso, 
        /// uma visita sem identificador caso já exista uma visita salva para o mesmo local, data e horário
        /// e uma visita nula caso não tenha sido possível salvar a visita
        /// </summary>
        /// <param name="visit">Visit a ser salva</param>
        /// <returns>Visit</returns>
        Visit PostVisit(Visit visit);

        /// <summary>
        /// Remove a visita cujo id é igual ao especificado
        /// </summary>
        /// <param name="id">Uma string representando o id da visita</param>
        /// <returns>true caso a visita tenha sido removida, false caso contrário</returns>
        bool DeleteVisit(string id);
    }
}
