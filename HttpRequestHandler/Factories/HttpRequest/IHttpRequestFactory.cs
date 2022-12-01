using HttpRequestHandler.HelperServices.RequestHandler;

namespace HttpRequestHandler.Factories.HttpRequest
{
    public interface IHttpRequestFactory
    {
        RequestHandler Create(RequestType type);
    }
}
