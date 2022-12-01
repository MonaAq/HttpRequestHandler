using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace HttpRequestHandler.CustomException
{
    public class CustomArgumentOutOfRangeException : SchedulerException
    {
        public CustomArgumentOutOfRangeException(string parameterName) : base($"Specified argument was out of the range of valid values. (Parameter '{parameterName}')")
        {

        }

        public CustomArgumentOutOfRangeException(ModelStateDictionary modelState) : base(modelState)
        {

        }

        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    }
}
