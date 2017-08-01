using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitAppBackend.Models;

namespace VisitAppBackend.Repositories
{
    interface IVisitsRepository
    {
        ICollection<Visit> GetMatchingVisits(string idFacebook, string idPlace, string date);

        ICollection<Visit> GetUserVisits(string idFacebook);

        Visit PostVisit(Visit visit);

        bool DeleteVisit(string id);
    }
}
