using MovieCollection.Repositories;
using MovieCollection.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Tests
{
    internal static class TestHelpers
    {
        public static IMovieRepository MakeTestMovieRepository()
        {
            var context = new MovieContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }
    }
}
