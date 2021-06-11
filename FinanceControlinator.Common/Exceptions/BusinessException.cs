using System;

namespace FinanceControlinator.Common.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException() { }

        public BusinessException(string message) : base(message) { }

        public BusinessException(string? message, Exception? innerException) : base(message, innerException) { }

        public virtual object Serialize() => Message;
    }
}
