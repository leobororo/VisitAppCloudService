using System;

namespace VisitAppBackend.Exceptions
{
    [Serializable]
    public class MatchingVisitException : SystemException
    {
        public MatchingVisitException(string message) : base(message)
        {
        }
    }
}
