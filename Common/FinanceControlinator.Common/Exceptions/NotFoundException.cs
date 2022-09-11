using CleanHandling;
using System;
using System.Net;

namespace FinanceControlinator.Common.Exceptions
{
    public class NotFoundException : BusinessException
    {
        public NotFoundException(string message) : base(HttpStatusCode.NotFound, message) { }

        public NotFoundException(string? message, Exception? innerException) : base(HttpStatusCode.NotFound, message, innerException) { }
    }
}
