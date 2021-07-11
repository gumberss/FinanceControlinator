using FinanceControlinator.Common.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace FinanceControlinator.Common.Exceptions
{
    public class BusinessException : Exception
    {
        private IEnumerable<ErrorData> _errorData;

        public HttpStatusCode Code { get; private set; }

        public BusinessException(HttpStatusCode code, ErrorData data) : base(data.Message)
        {
            _errorData = new[] { data };
            Code = code;

        }

        public BusinessException(HttpStatusCode code, IEnumerable<ErrorData> datas) : base(String.Join(", ", datas?.Select(x => x.Message)))
        {
            _errorData = datas;
            Code = code;
        }

        public BusinessException(HttpStatusCode code, ErrorData data, Exception? innerException) : base(data?.Message, innerException)
        {
            Code = code;
        }

        public BusinessException(HttpStatusCode code, Exception innerException) : base(innerException.Message, innerException)
        {
            Code = code;
        }

        public virtual String Log()
        {
            if (_errorData is null)
                return $"An erroru occurred: {InnerException}";

            return $"An error occurred: Code: {Code} - {String.Join(String.Empty, this._errorData.Select(x => $"{Environment.NewLine}\t\t{x.Log()}"))}, {InnerException}";
        }

        public virtual object Serialize()
        {
            if (_errorData == null) return this.Message;

            return new ReturnData(_errorData);
        }
    }

    public class ErrorData
    {
        public ErrorData(String message, String param, String value) : this(message, param)
        {
            Value = value;
        }

        public ErrorData(String message, String param) : this(message)
        {
            Param = param;
        }

        public ErrorData(String message)
        {
            Message = message;
        }

        public String Message { get; private set; }

        public String Param { get; private set; }

        public String Value { get; private set; }

        public static implicit operator String(ErrorData data)
        {
            return data.Message;
        }

        public static implicit operator ErrorData(String message)
        {
            return new ErrorData(message);
        }

        public String Log()
        {
            return $"Error Data: {Message} - Param/Value: {Param}/{Value ?? "No Value Reported to ErrorData"}";
        }
    }
}
