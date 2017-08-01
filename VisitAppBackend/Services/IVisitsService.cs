using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitAppBackend.Models;

namespace VisitAppBackend.Services
{
    interface IVisitsService
    {
        MatchingVisits GetMatchingVisits(string idFacebook, string idPlace, string date, int horaInicio, int horaFim);

        ICollection<Visit> GetUserVisits(string idFacebook);

        Visit PostVisit(Visit visit);

        bool DeleteVisit(string id);
    }
}
