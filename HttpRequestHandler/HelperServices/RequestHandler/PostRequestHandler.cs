using HttpRequestHandler.CustomException;
using Polly.CircuitBreaker;
using RestSharp;
using System.Net.Http.Headers;

namespace HttpRequestHandler.HelperServices.RequestHandler
{
    class PostRequestHandler : RequestHandler
    {
        public PostRequestHandler(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async override Task<TResult> CallApiAsync<TResult>(string url, object? body = null, DataFormat format = DataFormat.Json,
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
            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                if (header != null)
                    foreach (var headerItem in header)
                    {
                        request.Headers.Add(headerItem.Key, headerItem.Value);
                    }
                request.Content = CreateContent(body);
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var response = await CircuitBreakerPolicy
                      .ExecuteAsync(() => TransientErrorRetryPolicy
                      .ExecuteAsync(() => HttpClient.SendAsync(request)));


                var stringResult = await response?.Content?.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = ConvertToResult<TResult>(format, stringResult);

                    return result;
                }
                else if ((int)response.StatusCode >= 400 && (int)response.StatusCode < 500)
                {
                    if (!string.IsNullOrEmpty(stringResult))
                    {
                        var result = ConvertToResult<TResult>(format, stringResult);

                        throw new BadRequestException(result);
                    }
                    else
                    {
                        throw new BadRequestException(response.StatusCode.ToString());
                    }
                }
                else
                {
                    throw new ServerErrorException(response.StatusCode.ToString());
                }
            }
        }

        public override Task<TResult> CallApiAsync<TResult, TErrorResult>(string url, object body = null, DataFormat format = DataFormat.Json)
        {
            throw new NotImplementedException();
        }
    }
}
