using System;
namespace Teste.Exceptions
{
    [Serializable]
    public class CouldNotCreateVisitException : SystemException
    {
        public CouldNotCreateVisitException(string message) : base(message)
        {
        }
    }
}
