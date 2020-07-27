using System;
using System.Collections.Generic;

namespace ComplaintService.BusinessDomain.ApplicationModels
{
    public class ApiError<T>
    {
        public ApiError(int statusCode, string statusDescription)
        {
            Id = Guid.NewGuid().ToString();
            StatusCode = statusCode;
            StatusDescription = statusDescription;
            ErrorTime = DateTime.UtcNow;
        }

        public ApiError(int statusCode, string statusDescription, string message, List<T> errors)
            : this(statusCode, statusDescription)
        {
            Message = message;
            Errors = errors;
        }

        public string Id { get; set; }
        public int StatusCode { get; }
        public string StatusDescription { get; }
        public DateTime ErrorTime { get; }

        public string Message { get; }
        public List<T> Errors { get; }
    }

    public class ValidationErrorModel
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }
}