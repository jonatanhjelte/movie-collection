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
            var tm = new TmdbMovie() { imdb_id = "123test", title = "123title" };

            var movie = tm.ToMovie();

            Assert.Equal(tm.imdb_id, movie.MovieDatabaseId);
            Assert.Equal(tm.title, movie.Name);
        }

        [Fact]
        public void ToMovie_HasNoImdbId_SetsMovieDatabaseIdProperly()
        {
            var tm = new TmdbMovie() { imdb_id = null };

            var movie = tm.ToMovie();

            Assert.Equal("tt0000000", movie.MovieDatabaseId);
        }
    }
}
