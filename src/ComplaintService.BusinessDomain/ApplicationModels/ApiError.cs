using System;
using System.Collections.Generic;

namespace ComplaintService.BusinessDomain.ApplicationModels
{
    public class ApiError<T>
    {
        public string Id { get; set; }
        public int StatusCode { get; private set; }
        public string StatusDescription { get; private set; }
        public DateTime ErrorTime { get; private set; }

        public string Message { get; private set; }
        public List<T> Errors { get; private set; }

        public ApiError(int statusCode, string statusDescription)
        {
            this.Id = Guid.NewGuid().ToString();
            this.StatusCode = statusCode;
            this.StatusDescription = statusDescription;
            this.ErrorTime = DateTime.UtcNow;
        }

        public ApiError(int statusCode, string statusDescription, string message, List<T> errors)
            : this(statusCode, statusDescription)
        {
            this.Message = message;
            this.Errors = errors;
        }
    }

    public class ValidationErrorModel
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }
}