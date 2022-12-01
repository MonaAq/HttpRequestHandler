using HttpRequestHandler.CustomException;
using Polly.CircuitBreaker;
using RestSharp;
using System.Net.Http.Headers;

namespace HttpRequestHandler.HelperServices.RequestHandler
{
    class PutRequestHandler : RequestHandler
    {
        public PutRequestHandler(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public override async Task<TResult> CallApiAsync<TResult>(string url, object? body = null, DataFormat format = DataFormat.Json,
                                                                  Dictionary<string, string>? header = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            if (CircuitBreakerPolicy.CircuitState == CircuitState.Open)
            {
                throw new ServiceUnavailableException();
            }

            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            HttpContent content = CreateContent(body);

            var response = await CircuitBreakerPolicy
            .ExecuteAsync(() => TransientErrorRetryPolicy
            .ExecuteAsync(() => HttpClient.PutAsync(url, content)));

            if (response.IsSuccessStatusCode)
            {
                var stringResult = await response.Content.ReadAsStringAsync();

                var result = ConvertToResult<TResult>(format, stringResult);

                return result;
            }
            else if ((int)response.StatusCode >= 400 && (int)response.StatusCode < 500)
            {
                throw new BadRequestException(response.StatusCode.ToString());
            }
            else
            {
                throw new ServerErrorException(response.StatusCode.ToString());
            }

        }

        public override Task<TResult> CallApiAsync<TResult, TErrorResult>(string url, object body = null, DataFormat format = DataFormat.Json)
        {
            throw new NotImplementedException();
        }
    }
}
