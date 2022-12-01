using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HttpRequestHandler.CustomException
{
    public abstract class SchedulerException : Exception
    {

        public abstract HttpStatusCode StatusCode { get; }

        public SchedulerException(string message) : base(message)
        {
            Data.Add("StatusCode", StatusCode);
        }

        public SchedulerException(object errorObject, string message = "Unknown Exception") : base(message)
        {
            Data.Add("StatusCode", StatusCode);

            if (errorObject is Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
            {
                Data.Add("ErrorObject", new SerializableError(modelState));
            }
            else
            {
                Data.Add("ErrorObject", errorObject);
            }
        }

        public SchedulerException(string key, string value, string message = "Unknown Exception") : base(message)
        {
            Data.Add("StatusCode", StatusCode);

            Data.Add("ErrorObject", new Dictionary<string, string> { { key, value } });
        }
    }
}
