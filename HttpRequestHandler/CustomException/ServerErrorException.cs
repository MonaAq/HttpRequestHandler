using System.Net;
using System.Web.Mvc;

namespace HttpRequestHandler.CustomException
{
    public class ServerErrorException : SchedulerException
    {
        public ServerErrorException(string message) : base(message)
        {

        }

        public ServerErrorException(ModelStateDictionary modelState) : base(modelState)
        {

        }

        public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
    }
}
