using System;

namespace BLL
{
    public class StudentValidationException : Exception
    {
        public StudentValidationException() { }
        public StudentValidationException(string message) : base(message) { }
        public StudentValidationException(string message, Exception inner) : base(message, inner) { }
    }

    public class DataServiceException : Exception
    {
        public DataServiceException() { }
        public DataServiceException(string message) : base(message) { }
        public DataServiceException(string message, Exception inner) : base(message, inner) { }
    }
}
