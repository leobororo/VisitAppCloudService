using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitAppBackend.Models
{
    public class MatchingVisits
    {
        public ICollection<Visit> SameDayVisits
        {
            get;
            set;
        } = new List<Visit>();

        public ICollection<Visit> SameTimeVisits
        {
            get;
            set;
        } = new List<Visit>();
    }
}