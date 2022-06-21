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
        private HttpClient _httpClient;

        public MovieService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Movie>> FindMoviesByNameAsync(string name)
        {
            return await Task.FromResult(new List<Movie>());
        }
    }
}
