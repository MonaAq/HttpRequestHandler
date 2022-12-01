using System.Net;
using System.Web.Mvc;

namespace HttpRequestHandler.CustomException
{
    public class ServiceUnavailableException : SchedulerException
    {
        public ServiceUnavailableException() : base("Service Unavailable")
        {

        }

        public ServiceUnavailableException(string message) : base(message)
        {

        }

        public ServiceUnavailableException(ModelStateDictionary modelState) : base(modelState)
        {

        }

        public override HttpStatusCode StatusCode => HttpStatusCode.ServiceUnavailable;
    }
}
