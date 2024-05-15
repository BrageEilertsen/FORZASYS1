/*
using NUnit.Framework;
using System.Net.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using FluentAssertions;
using System.Linq;
using System.Text;
using System.Threading;
using FORZASYS1.Models;
using FORZASYS1.Interfaces;
using FORZASYS1.Services;
using Moq;
using Moq.Language.Flow;
using Moq.Protected;
using Hit = FORZASYS1.Models.Hit;

namespace TestProject1
{
    [TestFixture]
    public class ElasticsearchServiceTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private ElasticsearchService _elasticService;
        private Mock<IOptions<ElasticConfig>> _configMock;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _configMock = new Mock<IOptions<ElasticConfig>>();
            _configMock.Setup(config => config.Value).Returns(new ElasticConfig
            {
                Uri = "https://example.com",
                ApiKey = "test_api_key"
            });
            _elasticService = new ElasticsearchService(_httpClient, _configMock.Object);
        }

        [Test]
        public async Task SearchHighlights_ShouldReturnVideos_WhenSearchTermIsValid()
        {
            // Arrange
            var searchTerm = "goal";
            var expectedVideos = new List<Video>
            {
                new Video { Title = "video1", Url = "https://forzasysstorage.blob.core.windows.net/video1.mp4?sp=r&st=2024-04-30T20:57:00Z&se=2024-11-11T05:57:00Z&spr=https&sv=2022-11-02&sr=c&sig=al21O8KJOGxL%2F4FlSpBqkg3O9vpmVSyBsv%2Baxp6JNOI%3D", ThumbnailUrl = "/images/default-thumbnail.jpg" }
            };

            var jsonResponse = JsonConvert.SerializeObject(new SearchResultsResponse
            {
                Hits = new HitsWrapper
                {
                    Hits = new List<Hit>
                    {
                        new Hit
                        {
                            _id = "video1.mp4",
                            Source = new Source
                            {
                                thumbnailUrl = null
                            }
                        }
                    }
                }
            });

            _httpMessageHandlerMock.SetupRequest(HttpMethod.Post, "https://example.com/_application/search_application/forzasyssearchapp/_search")
                                   .ReturnsResponse(jsonResponse, "application/json");

            // Act
            var result = await _elasticService.SearchHighlights(searchTerm);

            // Assert
            result.Should().BeEquivalentTo(expectedVideos);
        }

        [Test]
        public async Task SearchHighlights_ShouldReturnEmptyList_WhenSearchTermIsInvalid()
        {
            // Arrange
            var searchTerm = "random";
            var jsonResponse = JsonConvert.SerializeObject(new SearchResultsResponse
            {
                Hits = new HitsWrapper
                {
                    Hits = new List<Hit>()
                }
            });

            _httpMessageHandlerMock.SetupRequest(HttpMethod.Post, "https://example.com/_application/search_application/forzasyssearchapp/_search")
                                   .ReturnsResponse(jsonResponse, "application/json");

            // Act
            var result = await _elasticService.SearchHighlights(searchTerm);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public async Task SearchHighlights_ShouldHandleNullOrEmptyIdGracefully()
        {
            // Arrange
            var searchTerm = "goal";
            var jsonResponse = JsonConvert.SerializeObject(new SearchResultsResponse
            {
                Hits = new HitsWrapper
                {
                    Hits = new List<Hit>
                    {
                        new Hit
                        {
                            _id = null,
                            Source = new Source
                            {
                                thumbnailUrl = null
                            }
                        }
                    }
                }
            });

            _httpMessageHandlerMock.SetupRequest(HttpMethod.Post, "https://example.com/_application/search_application/forzasyssearchapp/_search")
                                   .ReturnsResponse(jsonResponse, "application/json");

            // Act
            var result = await _elasticService.SearchHighlights(searchTerm);

            // Assert
            result.Should().BeEmpty();
        }
    }

    public static class HttpMessageHandlerExtensions
    {
        public static ISetup<HttpMessageHandler, Task<HttpResponseMessage>> SetupRequest(this Mock<HttpMessageHandler> mock, HttpMethod method, string requestUri)
        {
            return mock.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                           "SendAsync",
                           ItExpr.Is<HttpRequestMessage>(req =>
                               req.Method == method && req.RequestUri.ToString() == requestUri),
                           ItExpr.IsAny<CancellationToken>()
                       );
        }

        public static void ReturnsResponse(this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> setup, string content, string mediaType)
        {
            setup.ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(content, Encoding.UTF8, mediaType)
            });
        }
    }
}
*/
