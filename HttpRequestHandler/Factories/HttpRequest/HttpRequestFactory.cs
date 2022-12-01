using HttpRequestHandler.HelperServices.RequestHandler;

namespace HttpRequestHandler.Factories.HttpRequest
{
    public class HttpRequestFactory : IHttpRequestFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpRequestFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public RequestHandler Create(RequestType type)
        {
            return type switch
            {
                RequestType.Get => new GetRequestHandler(_httpClientFactory),
                RequestType.Put => new PutRequestHandler(_httpClientFactory),
                RequestType.Post => new PostRequestHandler(_httpClientFactory),
                RequestType.Delete => new DeleteRequestHandler(_httpClientFactory),
                _ => throw new NotSupportedException()
            };
        }
    }
}
