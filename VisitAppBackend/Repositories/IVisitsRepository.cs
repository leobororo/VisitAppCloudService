﻿using System.Collections.Generic;
using VisitAppBackend.Models;

namespace VisitAppBackend.Repositories
{
    interface IVisitsRepository
    {
        /// <summary>
        /// Validates the facebook access token.
        /// </summary>
        /// <returns><c>true</c>, if facebook access token was validated, <c>false</c> otherwise.</returns>
        /// <param name="idFacebook">Identifier facebook.</param>
        /// <param name="accessToken">Access token.</param>
		bool ValidateFacebookAccessToken(string idFacebook, string accessToken);

        /// <summary>
        /// Devolve uma lista de visitas de um determinado usuário do facebook em uma determinado local em um determinado dia
        /// </summary>
        /// <param name="idsFacebook">Um string contendo uma lista de ids do facebook separados por vírgula</param>
        /// <param name="idPlace">Um string contendo o id de um Google Place</param>
        /// <param name="date">Uma string representando uma data no formato dd/MM/yyyy</param>
        /// <returns>ICollection<Visit></returns>
        ICollection<Visit> GetMatchingVisits(string idFacebook, string idPlace, string date);

        /// <summary>
        /// Devolve todas as visitas de um determinado usuário do facebook
        /// </summary>
        /// <param name="idFacebook">Uma string representando o id do facebook do usuário</param>
        /// <returns>ICollection<Visit></returns>
        ICollection<Visit> GetUserVisits(string idFacebook);

        /// <summary>
        /// Salva a visita especificada, devolve uma nova visita caso esta tenha sido salva com sucesso, 
        /// null caso não tenha sido possível salvar a visita
        /// </summary>
        /// <param name="visit">Visita a ser salva</param>
        /// <returns>Visit salva ou null</returns>
        Visit PostVisit(Visit visit);

        /// <summary>
        /// Remove a visita cujo id é igual ao especificado
        /// </summary>
        /// <param name="id">Uma string representando o id da visita</param>
        /// <returns>true caso a visita tenha sido removida, false caso contrário</returns>
        bool DeleteVisit(string id);
    }
}
