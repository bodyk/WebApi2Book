using System;

namespace WebApi2Book.Common
{
    public class DateTimeAdapter : IDateTime
    {
        public DateTime UtcNow => DateTime.Now;
    }
}
