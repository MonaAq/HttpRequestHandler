using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace HttpRequestHandler.CustomException
{
    public class NotPermisionToAccessException : SchedulerException
    {
        public NotPermisionToAccessException(string message) : base(message)
        {

        }

        public NotPermisionToAccessException(ModelStateDictionary modelState) : base(modelState)
        {

        }

        public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
    }
}
