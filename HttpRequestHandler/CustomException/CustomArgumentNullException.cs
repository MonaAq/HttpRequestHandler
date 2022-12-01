using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace HttpRequestHandler.CustomException
{
    public class CustomArgumentNullException : SchedulerException
    {
        public CustomArgumentNullException(string parameterName) : base($"Value cannot be null. (Parameter '{parameterName}')")
        {

        }

        public CustomArgumentNullException(ModelStateDictionary modelState) : base(modelState)
        {

        }

        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    }
}
