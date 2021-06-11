using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceControlinator.Common.Exceptions
{
    public class NotFoundException : BusinessException
    {
        public NotFoundException() { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
