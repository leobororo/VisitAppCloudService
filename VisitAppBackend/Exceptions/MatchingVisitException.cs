using System;
namespace Teste.Exceptions
{
    [Serializable]
    public class MatchingVisitException : SystemException
    {
        public MatchingVisitException(string message) : base(message)
        {
        }
    }
}
