using HttpRequestHandler.CustomException;
using Polly.CircuitBreaker;
using RestSharp;

namespace HttpRequestHandler.HelperServices.RequestHandler
{
    class GetRequestHandler : RequestHandler
    {
        public GetRequestHandler(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }
        public async override Task<TResult> CallApiAsync<TResult>(string url, object body = null, DataFormat format = DataFormat.Json, Dictionary<string, string>? header = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (CircuitBreakerPolicy.CircuitState == CircuitState.Open)
            {
                throw new ServiceUnavailableException();
            }
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                if (header != null)
                    foreach (var headerItem in header)
                    {
                        request.Headers.Add(headerItem.Key, headerItem.Value);
                    }

                var response = await CircuitBreakerPolicy
                .ExecuteAsync(() => TransientErrorRetryPolicy
                .ExecuteAsync(() => HttpClient.SendAsync(request)));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var result = ConvertToResult<TResult>(format, content);

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
        }

        public override async Task<TResult> CallApiAsync<TResult, TErrorResult>(string url, object body = null, DataFormat format = DataFormat.Json)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (CircuitBreakerPolicy.CircuitState == CircuitState.Open)
            {
                throw new ServiceUnavailableException();
            }

            var response = await CircuitBreakerPolicy
                .ExecuteAsync(() => TransientErrorRetryPolicy
                .ExecuteAsync(() => HttpClient.GetAsync(url)));

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(content))
                {
                    var result = ConvertToResult<TResult>(format, content);

                    return result;
                }

                return default;

            }
            else if ((int)response.StatusCode >= 400 && (int)response.StatusCode < 500)
            {
                if (!string.IsNullOrEmpty(content))
                {
                    var result = ConvertToResult<TErrorResult>(format, content);

                    throw new BadRequestException(result);
                }

                throw new BadRequestException(response.StatusCode.ToString());
            }
            else
            {
                throw new ServerErrorException(response.StatusCode.ToString());
            }
        }
    }
}
