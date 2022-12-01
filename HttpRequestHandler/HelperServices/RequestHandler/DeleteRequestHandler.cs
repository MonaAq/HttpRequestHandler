using HttpRequestHandler.CustomException;
using Polly.CircuitBreaker;
using RestSharp;
using System.Net.Http.Headers;
using System.Text;

namespace HttpRequestHandler.HelperServices.RequestHandler
{
    class DeleteRequestHandler : RequestHandler
    {
        public DeleteRequestHandler(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public override async Task<TResult> CallApiAsync<TResult>(string url, object? body = null, DataFormat format = DataFormat.Json,
                                                                  Dictionary<string, string>? header = null)
        {
            HttpResponseMessage response;

            if (CircuitBreakerPolicy.CircuitState == CircuitState.Open)
            {
                throw new ServiceUnavailableException();
            }

            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            if (body == null)
            {
                response = await DeleteRequestAsync(url);
            }
            else
            {
                response = await SendRequestAsync(url, body);
            }

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

        private async Task<HttpResponseMessage> DeleteRequestAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var response = await CircuitBreakerPolicy
               .ExecuteAsync(() => TransientErrorRetryPolicy
               .ExecuteAsync(() => HttpClient.DeleteAsync(url)));

            return response;
        }

        private async Task<HttpResponseMessage> SendRequestAsync(string url, object body)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            string content = CreateStringContent(body);

            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            var response = await CircuitBreakerPolicy
                .ExecuteAsync(() => TransientErrorRetryPolicy
                .ExecuteAsync(() => HttpClient.SendAsync(request)));

            return response;

        }
    }
}
