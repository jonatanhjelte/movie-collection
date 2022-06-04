using MovieCollection.Domain;
using MovieCollection.Domain.Exceptions;
using MovieCollection.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Services
{
    public class MovieService
    {
        List<Movie> movies = new List<Movie>();

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return movies;
        }

        public async Task AddAsync(Movie movie)
        {
            movies.Add(movie);
        }

        public async Task RemoveAsync(Movie movie)
        {
            if (!movies.Contains(movie))
            {
                throw new MovieDoesNotExistException();
            }

            movies.Remove(movie);
        }

        public async Task UpdateAsync(Movie movie)
        {
            var existingMovie = movies.FirstOrDefault(m => m.MovieDatabaseId == movie.MovieDatabaseId);

            _ = existingMovie ?? throw new MovieDoesNotExistException();

            movies.Remove(existingMovie);
            movies.Add(movie);
        }
    }
}
