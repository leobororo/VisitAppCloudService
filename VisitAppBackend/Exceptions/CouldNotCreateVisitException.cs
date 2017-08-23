using System;

namespace VisitAppBackend.Exceptions
{
    [Serializable]
    public class CouldNotCreateVisitException : SystemException
    {
        public CouldNotCreateVisitException(string message) : base(message)
        {
        }
    }
}
