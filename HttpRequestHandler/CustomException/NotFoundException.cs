using System.Net;

namespace HttpRequestHandler.CustomException
{
    public class NotFoundException : SchedulerException
    {
        public NotFoundException() : base("Not Found")
        {
        }

        public NotFoundException(string parameterName) : base($"Not Found '{parameterName}'")
        {

        }

        public NotFoundException(object errorObject) : base(errorObject)
        {

        }

        public NotFoundException(string key, string value) : base(key, value)
        {

        }

        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    }
}
