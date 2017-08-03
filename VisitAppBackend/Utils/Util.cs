using System;
using VisitAppBackend.Models;

namespace VisitAppBackend.Utils
{
    public class Util
    {
        private Util()
        {
        }

        /// <summary>
        /// Copia os dados de uma visita para outra
        /// </summary>
        /// <param name="fromVisit">Visit de onde os dados serão copiados</param>
        /// <param name="toVisit">Visit para onde os dados serão copiados</param>
        public static void copyVisitData(Visit fromVisit, Visit toVisit)
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

        public static bool isTimeBoxValid(Visit visit)
        {
            bool finalHourGreaterInitialHour = visit.HoraFimVisita > visit.HoraInicioVisita;
            bool initialHourEqualsFinalHourAndInitialMinuteLessFinalMinute = visit.HoraInicioVisita == visit.HoraFimVisita && visit.MinutoInicioVisita < visit.MinutoFimVisita;
            return isTimeParametersValid(visit) && (finalHourGreaterInitialHour || initialHourEqualsFinalHourAndInitialMinuteLessFinalMinute);
        }

        public static bool isQueryParametersForMatchingValid(String idFacebook, string idPlace, string date)
        {
            return !(String.IsNullOrEmpty(idFacebook) || String.IsNullOrEmpty(idPlace) || String.IsNullOrEmpty(date));
        }

        private static bool isTimeParametersValid(Visit visit)
        {
            return isValidHour(visit.HoraInicioVisita) && isValidHour(visit.HoraFimVisita) && isValidMinute(visit.MinutoFimVisita) && isValidMinute(visit.MinutoInicioVisita);
        }

        private static bool isValidHour(int hour)
        {
            return hour > -1 && hour < 23;
        }

        private static bool isValidMinute(int minute)
        {
            return minute > -1 && minute < 59;
        }
    }
}