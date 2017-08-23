using System;

namespace VisitAppBackend.Exceptions
{
    [Serializable]
    public class InvalidMatchingVisitsParametersException : SystemException
    {
        public InvalidMatchingVisitsParametersException(string message) : base(message)
        {
        }
    }
}
