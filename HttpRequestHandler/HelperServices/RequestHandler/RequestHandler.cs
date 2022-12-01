using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using RestSharp;
using System.Net;
using System.Text;

namespace HttpRequestHandler.HelperServices.RequestHandler
{
    public abstract class RequestHandler
    {
        private static readonly Random random = new();

        private readonly IHttpClientFactory _httpClientFactory;

        public RequestHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        protected HttpClient HttpClient => _httpClientFactory.CreateClient();

        protected readonly AsyncRetryPolicy<HttpResponseMessage> TransientErrorRetryPolicy = Policy
           .HandleResult<HttpResponseMessage>(message => (int)message.StatusCode == (int)HttpStatusCode.TooManyRequests || (int)message.StatusCode >= (int)HttpStatusCode.InternalServerError)
           .WaitAndRetryAsync(2, retryAttemp =>
           {
               return TimeSpan.FromSeconds(Math.Pow(1, retryAttemp)) + TimeSpan.FromMilliseconds(random.Next(0, 50));
           });

        protected static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> CircuitBreakerPolicy = Policy
           .HandleResult<HttpResponseMessage>(message => (int)message.StatusCode >= 500 || (int)message.StatusCode == (int)HttpStatusCode.TooManyRequests)
           .CircuitBreakerAsync(2, TimeSpan.FromMinutes(5));

        protected TResult ConvertToResult<TResult>(DataFormat format, string content)
        {
            return format switch
            {
                DataFormat.Json => JsonConvert.DeserializeObject<TResult>(content),
                DataFormat.None => (TResult)Convert.ChangeType(content, typeof(TResult)),
                _ => throw new NotSupportedException(nameof(DataFormat))
            };
        }

        public abstract Task<TResult> CallApiAsync<TResult>(string url, object body = null, DataFormat format = DataFormat.Json, Dictionary<string, string>? header = null);

        public abstract Task<TResult> CallApiAsync<TResult, TErrorResult>(string url, object body = null, DataFormat format = DataFormat.Json);

        protected HttpContent CreateContent(object body)
        {
            return new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        }

        protected string CreateStringContent(object body)
        {
            return JsonConvert.SerializeObject(body);
        }

    }
}
