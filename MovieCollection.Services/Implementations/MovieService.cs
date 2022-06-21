using MovieCollection.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Services.Implementations
{
    public class MovieService : IMovieService
    {
        public async Task<IEnumerable<Movie>> FindMoviesByNameAsync(string name)
        {
            return await Task.FromResult(new List<Movie>());
        }
    }
}
