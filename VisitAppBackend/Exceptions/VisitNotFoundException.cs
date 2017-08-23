using System;
namespace Teste.Exceptions
{
    [Serializable]
    public class VisitNotFoundException : SystemException
    {
        public VisitNotFoundException(string message) : base(message)
        {
        }
    }
}
