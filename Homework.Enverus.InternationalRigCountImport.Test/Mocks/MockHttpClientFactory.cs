using System.Net;
using Moq;
using Homework.Enverus.InternationalRigCountImport.Core.Extensions;
using Moq.Protected;
using HttpClient = System.Net.Http.HttpClient;

namespace Homework.Enverus.InternationalRigCountImport.Test.Mocks
{
    public static class MockHttpClientFactory
    {
        public static Mock<IHttpClientFactory> GetHttpClientFactoryMock()
        {
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(a=>a.RequestUri.AbsoluteUri.EndsWith("static-files/7240366e-61cc-4acb-89bf-86dc1a0dffe8")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(result)
                .Verifiable();
            
            result = new HttpResponseMessage(HttpStatusCode.NotFound);

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(a => a.RequestUri.AbsoluteUri.EndsWith("qwertyui")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(result)
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://bakerhughesrigcount.gcs-web.com")
            };

            mockHttpClientFactory.Setup(r => r.CreateClient(Constants.HttpClientName)).Returns(httpClient);
          
            return mockHttpClientFactory;
        }

        public static Mock<IHttpClientFactory> GetHttpClientFactoryWithDataMock()
        {
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(a => a.RequestUri.AbsoluteUri.EndsWith("static-files/7240366e-61cc-4acb-89bf-86dc1a0dffe8")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    Content = new ByteArrayContent(File.ReadAllBytes("c:\\Users\\luka.radovanovic\\Practice\\BakerHughesRigCount\\Worldwide Rig Counts - Current & Historical DataSource.xlsx")),
                    StatusCode = HttpStatusCode.OK
                })
                .Verifiable();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(a => a.RequestUri.AbsoluteUri.EndsWith("qwertyui")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    Content = new ByteArrayContent(Array.Empty<byte>()),
                    StatusCode = HttpStatusCode.NotFound
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://bakerhughesrigcount.gcs-web.com")
            };

            mockHttpClientFactory.Setup(r => r.CreateClient(Constants.HttpClientName)).Returns(httpClient);

            return mockHttpClientFactory;
        }
    }
}
