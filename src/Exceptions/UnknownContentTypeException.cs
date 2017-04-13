using System;

namespace SeeSharp7.Patch4Net.Exceptions
{
    public class UnknownContentTypeException : Exception
    {
        public UnknownContentTypeException(string message) : base(message)
        { }
    }
}
