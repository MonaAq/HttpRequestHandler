using System.Net;

namespace HttpRequestHandler.CustomException
{
    public class MemoryCacheException : SchedulerException
    {
        public MemoryCacheException() : base("Internal Server Error")
        {
        }

        public MemoryCacheException(string message) : base(message)
        {
        }

        public MemoryCacheException(object errorObject) : base(errorObject, "Internal Server Error")
        {
        }

        public MemoryCacheException(string key, string value) : base(key, value, "Internal Server Error")
        {
        }

        public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
    }
}
