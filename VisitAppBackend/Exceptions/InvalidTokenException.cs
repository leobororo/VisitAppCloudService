using System;
namespace Teste.Exceptions
{
    [Serializable]
    public class InvalidTokenException : System.Exception
    {
        public InvalidTokenException(String message) : base (message)
        {
        }
    }
}
