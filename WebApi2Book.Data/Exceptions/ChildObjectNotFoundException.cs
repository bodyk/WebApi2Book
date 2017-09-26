using System;

namespace WebApi2Book.Data.Exceptions
{
    public class ChildObjectNotFoundException : Exception
    {
        public ChildObjectNotFoundException(string message)
            : base(message)
        {
            
        }
    }
}
