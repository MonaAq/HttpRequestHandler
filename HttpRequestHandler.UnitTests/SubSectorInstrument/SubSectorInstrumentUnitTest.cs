using HttpRequestHandler.Factories.HttpRequest;
using HttpRequestHandler.UnitTests.SubSectorInstrument.Models;
using Microsoft.Extensions.Configuration;
using Moq;

namespace HttpRequestHandler.UnitTests.SubSectorInstrument
{
    public class SubSectorInstrumentUnitTest
    {
        public readonly IConfigurationRoot _configuration;
        public SubSectorInstrumentUnitTest()
        {
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.UnitTest.json", true).Build();
        }

        [Fact]
        public async void GetRequestTest()
        {
            var config = _configuration.GetSection("ApiConfiguration").Get<ApiConfiguration>();
            string url = config.BaseUrl + config.GetInstrumentsBySectorCodeUrl + "/60";
            var mockFactory = new Mock<IHttpClientFactory>();
            var client = new HttpClient();
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            IHttpClientFactory factory = mockFactory.Object;
            HttpRequestFactory httpRequestFactory = new HttpRequestFactory(factory);
            var response = await httpRequestFactory.Create(RequestType.Get).CallApiAsync<InstrumentsBySectorCodeResponse>(url);
            Assert.NotNull(response);
        }
    }
}