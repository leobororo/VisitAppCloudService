using System;

namespace VisitAppBackend.Clients
{
    interface IFacebookClient
    {
        /// <summary>
        /// Validates the facebook access token.
        /// </summary>
        /// <returns><c>true</c>, if facebook access token was validated, <c>false</c> otherwise.</returns>
        /// <param name="idFacebook">Identifier facebook.</param>
        /// <param name="accessToken">Access token.</param>
        void ValidateFacebookAccessToken(string idFacebook, string accessToken);
    }
}
