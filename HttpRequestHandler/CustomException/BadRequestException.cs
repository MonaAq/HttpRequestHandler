using System.Net;


namespace HttpRequestHandler.CustomException
{
    public class BadRequestException : SchedulerException
    {
        public BadRequestException() : base("Bad Request")
        {

        }

        public BadRequestException(string message) : base(message)
        {

        }

        public BadRequestException(object errorObject) : base(errorObject, "Bad Request")
        {

        }

        public BadRequestException(string key, string value) : base(key, value, "Bad Request")
        {

        }

        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    }
}
