using Xunit;
using MovieCollection.Services;
using System.Threading.Tasks;
using MovieCollection.Domain;
using MovieCollection.Domain.Exceptions;
using System.Linq;
using System;
using MovieCollection.Repositories.Abstractions;

namespace MovieCollection.Tests
{
    public class MovieServiceTests
    {
        private readonly MovieService _service = new MovieService(TestHelpers.MakeTestMovieRepository());
        private readonly Movie _testMovie = MakeTestMovie();

        [Fact]
        public async Task CreateService_IsEmpty()
        {
            var movies = await _service.GetAllAsync();

            Assert.Empty(movies);
        }

        [Fact]
        public async Task AddMovieThenRemoveMovie_IsEmpty()
        {
            await _service.AddAsync(_testMovie);
            await _service.RemoveAsync(_testMovie);
            var movies = await _service.GetAllAsync();

            Assert.Empty(movies);
        }

        [Fact]
        public async Task RemoveMovieThatDoesNotExist_ThrowsMovieDoesNotExistException()
        {
            await Assert.ThrowsAsync<MovieDoesNotExistException>(async () => await _service.RemoveAsync(_testMovie));
        }

        [Fact]
        public async Task Add2MoviesThenRemove1_GetAllOnlyReturns1()
        {
            var testMovie2 = MakeTestMovie();

            await _service.AddAsync(_testMovie);
            await _service.AddAsync(testMovie2);
            await _service.RemoveAsync(_testMovie);
            var movies = await _service.GetAllAsync();

            Assert.Single(movies);
            Assert.Equal(testMovie2, movies.ElementAt(0));
        } 

        [Fact]
        public async Task Add2Movies_GetAllReturnsBoth()
        {
            var testMovie2 = MakeTestMovie();

            await _service.AddAsync(_testMovie);
            await _service.AddAsync(testMovie2);
            var movies = await _service.GetAllAsync();

            Assert.Equal(2, movies.Count());
            Assert.Contains(testMovie2, movies);
            Assert.Contains(_testMovie, movies);
        }

        [Fact]
        public async Task AddMovieThenUpdateMovie_GetAllReturnsUpdatedMovie()
        {
            var updatedMovie = _testMovie with { Name = $"{_testMovie.Name}UPDATED" };

            await _service.AddAsync(_testMovie);
            await _service.UpdateAsync(updatedMovie);
            var movies = await _service.GetAllAsync();

            Assert.Equal(updatedMovie, movies.First(m => m.MovieDatabaseId == _testMovie.MovieDatabaseId));
        }

        [Fact]
        public async Task UpdateMovieThatDoesNotExist_ThrowsMovieDoesNotExistException()
        {
            var testMovie2 = MakeTestMovie();

            await _service.AddAsync(_testMovie);
            
            await Assert.ThrowsAsync<MovieDoesNotExistException>(async () => await _service.UpdateAsync(testMovie2));
        }

        private static Movie MakeTestMovie()
        {
            var rnd = new Random();

            return new Movie()
            {
                MovieDatabaseId = rnd.Next().ToString(),
                Name = Guid.NewGuid().ToString(),
            };
        }
    }
}