using System.Collections.Generic;
using Xunit;
using Moq;
using PlacesApp.ViewModels;
using PlacesApp.Services.Interfaces;
using PlacesApp.Model;
using System.Threading.Tasks;

namespace PlacesApp.Tests
{
    public class SearchViewModelTests
    {
        [Fact]
        public async Task PerformSearch_ShouldUpdateSearchResults()
        {
            // Arrange
            var mockPlacesService = new Mock<IPlacesService>();
            var testPlaces = new List<Place>
            {
                new Place { PlaceId = "ChIJj8aPVtsUlR4RD1SVrzliOz0", MainText = "Kempton Park", SecondaryText = "South Africa" },
                new Place { PlaceId = "ChIJs48Hw5d0dkgRfWxqrH2RQu0", MainText = "Kempton Park Racecourse", SecondaryText = "Staines Road East, Sunbury-on-Thames, UK" }
            };
            mockPlacesService.Setup(s => s.SearchPlaces(It.IsAny<string>())).ReturnsAsync(testPlaces);

            var viewModel = new SearchViewModel(mockPlacesService.Object);

            // Act
            viewModel.SearchQuery = "Test Query";
            viewModel.SearchCommand.Execute(null);

            // Assert
            Assert.Equal(2, viewModel.SearchResults.Count);
            Assert.Equal("Kempton Park", viewModel.SearchResults[0].MainText);
            Assert.Equal("Kempton Park Racecourse", viewModel.SearchResults[1].MainText);
        }

        [Fact]
        public async Task PerformSearch_WithEmptyQuery_ShouldNotCallService()
        {
            // Arrange
            var mockPlacesService = new Mock<IPlacesService>();
            var viewModel = new SearchViewModel(mockPlacesService.Object);

            // Act
            viewModel.SearchQuery = "";
            viewModel.SearchCommand.Execute(null);

            // Assert
            mockPlacesService.Verify(s => s.SearchPlaces(It.IsAny<string>()), Times.Never);
        }
    }
}