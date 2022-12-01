using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace HttpRequestHandler.CustomException
{
    public class NationalCodeNotFoundException : SchedulerException
    {
        public NationalCodeNotFoundException(string message) : base(message)
        {

        }

        public NationalCodeNotFoundException(ModelStateDictionary modelState) : base(modelState)
        {

        }

        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;


    }
}
