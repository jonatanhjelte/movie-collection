using MovieCollection.Domain;
using MovieCollection.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Tests.Helpers
{
    internal class MovieServiceTestHelper : BaseTestHelper
    {
        public readonly MovieService MovieService;

        public MovieServiceTestHelper()
        {
            MovieService = new MovieService(MovieRepository);
        }
    }
}
