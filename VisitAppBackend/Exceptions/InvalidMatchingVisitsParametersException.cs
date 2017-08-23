using System;
namespace Teste.Exceptions
{
    [Serializable]
    public class InvalidMatchingVisitsParametersException : SystemException
    {
        public InvalidMatchingVisitsParametersException(string message) : base(message)
        {
        }
    }
}
