using MovieCollection.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var service = new MovieService();

            var moviesOneChar = await service.FindMoviesByNameAsync("a");
            var moviesTwoChar = await service.FindMoviesByNameAsync("aa");

            Assert.Empty(moviesOneChar);
            Assert.Empty(moviesTwoChar);
        }
    }
}
