using System;

namespace VisitAppBackend.Exceptions
{
    [Serializable]
    public class VisitNotFoundException : SystemException
    {
        public VisitNotFoundException(string message) : base(message)
        {
        }
    }
}
