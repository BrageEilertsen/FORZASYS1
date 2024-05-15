using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using FORZASYS1.Controllers;
using FORZASYS1.Models;
using FluentAssertions;
using FORZASYS1.Interfaces;

namespace TestProject1
{
    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<IElasticsearchService> _elasticsearchServiceMock;
        private HomeController _homeController;

        [SetUp]
        public void Setup()
        {
            _elasticsearchServiceMock = new Mock<IElasticsearchService>();
            _homeController = new HomeController(_elasticsearchServiceMock.Object);
        }

        [Test]
        public async Task Index_ShouldReturnVideos_WhenSearchTermIsProvided()
        {
            // Arrange
            var searchTerm = "goal";
            var expectedVideos = new List<Video>
            {
                new Video { Title = "Goal by Player A", Url = "https://example.com/video1.mp4", ThumbnailUrl = "/images/default-thumbnail.jpg" }
            };

            _elasticsearchServiceMock
                .Setup(service => service.SearchHighlights(searchTerm))
                .ReturnsAsync(expectedVideos);

            // Act
            var result = await _homeController.Index(searchTerm) as ViewResult;
            Assert.IsNotNull(result, "Expected result to be a ViewResult, but it was null.");

            var model = result.Model as VideoListViewModel;
            Assert.IsNotNull(model, "Expected model to be of type VideoListViewModel, but it was null.");

            // Assert
            model.Videos.Should().BeEquivalentTo(expectedVideos);
        }

        [Test]
        public async Task Index_ShouldReturnEmptyList_WhenSearchTermIsNotProvided()
        {
            // Act
            var result = await _homeController.Index(null) as ViewResult;
            var model = result.Model as VideoListViewModel;

            // Assert
            model.Videos.Should().BeEmpty();
        }

        [Test]
        public async Task Index_ShouldReturnEmptyList_WhenSearchTermIsEmpty()
        {
            // Act
            var result = await _homeController.Index(string.Empty) as ViewResult;
            var model = result.Model as VideoListViewModel;

            // Assert
            model.Videos.Should().BeEmpty();
        }
    }
}
