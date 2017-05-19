using System;
using System.Runtime.Serialization;

namespace CQRSlite.Extensions.SQLStreamStore
{
    public class TypeResolutionException : Exception
    {        
       public TypeResolutionException()
        {
        }

        public TypeResolutionException(string message) : base(message)
        {
        }

        public TypeResolutionException(string message, Exception inner) : base(message, inner)
        {
        }        
    }
}