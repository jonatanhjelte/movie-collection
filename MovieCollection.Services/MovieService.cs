using Microsoft.EntityFrameworkCore;
using MovieCollection.Domain;
using MovieCollection.Domain.Exceptions;
using MovieCollection.Repositories;
using MovieCollection.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Services
{
    public class MovieService
    {
        private List<Movie> _movies = new List<Movie>();
        private readonly IMovieRepository _repo;

        public MovieService(IMovieRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return _repo.Movies;
        }

        public async Task AddAsync(Movie movie)
        {
            _repo.Movies.Add(movie);
            await _repo.SaveChangesAsync();
        }

        public async Task RemoveAsync(Movie movie)
        {
            if (!_repo.Movies.Contains(movie))
            {
                throw new MovieDoesNotExistException();
            }

            _repo.Movies.Remove(movie);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateAsync(Movie movie)
        {
            var existingMovie = await _repo
                .Movies
                .FirstOrDefaultAsync(m => m.MovieDatabaseId == movie.MovieDatabaseId);

            _ = existingMovie ?? throw new MovieDoesNotExistException();

            existingMovie.Name = movie.Name;
            await _repo.SaveChangesAsync();
        }
    }
}
