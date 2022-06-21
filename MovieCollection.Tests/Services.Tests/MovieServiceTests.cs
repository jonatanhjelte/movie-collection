using MovieCollection.Services.Implementations;
using MovieCollection.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieCollection.Tests.Services.Tests
{
    public class MovieServiceTests
    {
        [Fact]
        public async Task FindMoviesByName_LessThan3Characters_ReturnsEmptyList()
        {
            var mockHttpClient = new HttpClient(new MockHttpMessageHandler());
            var service = new MovieService(mockHttpClient);

            var moviesOneChar = await service.FindMoviesByNameAsync("a");
            var moviesTwoChar = await service.FindMoviesByNameAsync("aa");

            Assert.Empty(moviesOneChar);
            Assert.Empty(moviesTwoChar);
        }
    }
}
