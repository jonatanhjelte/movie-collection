using MovieCollection.Services.TmdbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieCollection.Tests.Services.Tests.TmdbModels
{
    public class TmdbMovieTests
    {
        [Fact]
        public void ToMovie_HasAllRelevantProperties_MapsPropertiesProperly()
        {
            var tm = new TmdbMovie() { id = 123, title = "123title" };

            var movie = tm.ToMovie();

            Assert.Equal(tm.id.ToString(), movie.MovieDatabaseId);
            Assert.Equal(tm.title, movie.Name);
        }
    }
}
