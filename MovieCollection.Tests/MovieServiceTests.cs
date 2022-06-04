using Xunit;
using MovieCollection.Services;
using System.Threading.Tasks;
using MovieCollection.Domain;
using MovieCollection.Domain.Exceptions;
using System.Linq;
using System;
using MovieCollection.Repositories.Abstractions;
using MovieCollection.Tests.Helpers;

namespace MovieCollection.Tests
{
    public class MovieServiceTests
    {
        private readonly Movie _testMovie = MakeTestMovie();

        [Fact]
        public async Task CreateService_IsEmpty()
        {
            using var helper = new MovieServiceTestHelper();
            
            var movies = await helper.MovieService.GetAllAsync();

            Assert.Empty(movies);
        }

        [Fact]
        public async Task AddMovieThenRemoveMovie_IsEmpty()
        {
            using var helper = new MovieServiceTestHelper();

            await helper.MovieService.AddAsync(_testMovie);
            await helper.MovieService.RemoveAsync(_testMovie);
            var movies = await helper.MovieService.GetAllAsync();

            Assert.Empty(movies);
        }

        [Fact]
        public async Task RemoveMovieThatDoesNotExist_ThrowsMovieDoesNotExistException()
        {
            using var helper = new MovieServiceTestHelper();

            await Assert.ThrowsAsync<MovieDoesNotExistException>(async () => await helper.MovieService.RemoveAsync(_testMovie));
        }

        [Fact]
        public async Task Add2MoviesThenRemove1_GetAllOnlyReturns1()
        {
            using var helper = new MovieServiceTestHelper();
            var testMovie2 = MakeTestMovie();

            await helper.MovieService.AddAsync(_testMovie);
            await helper.MovieService.AddAsync(testMovie2);
            await helper.MovieService.RemoveAsync(_testMovie);
            var movies = await helper.MovieService.GetAllAsync();

            Assert.Single(movies);
            Assert.Equal(testMovie2, movies.ElementAt(0));
        }

        [Fact]
        public async Task Add2Movies_GetAllReturnsBoth()
        {
            using var helper = new MovieServiceTestHelper();
            var testMovie2 = MakeTestMovie();

            await helper.MovieService.AddAsync(_testMovie);
            await helper.MovieService.AddAsync(testMovie2);
            var movies = await helper.MovieService.GetAllAsync();

            Assert.Equal(2, movies.Count());
            Assert.Contains(testMovie2, movies);
            Assert.Contains(_testMovie, movies);
        }

        [Fact]
        public async Task AddMovieThenUpdateMovie_GetAllReturnsUpdatedMovie()
        {
            using var helper = new MovieServiceTestHelper();
            var updatedMovie = _testMovie with { Name = $"{_testMovie.Name}UPDATED" };

            await helper.MovieService.AddAsync(_testMovie);
            await helper.MovieService.UpdateAsync(updatedMovie);
            var movies = await helper.MovieService.GetAllAsync();

            Assert.Equal(updatedMovie, movies.First(m => m.MovieDatabaseId == _testMovie.MovieDatabaseId));
        }

        [Fact]
        public async Task UpdateMovieThatDoesNotExist_ThrowsMovieDoesNotExistException()
        {
            using var helper = new MovieServiceTestHelper();
            var testMovie2 = MakeTestMovie();

            await helper.MovieService.AddAsync(_testMovie);

            await Assert.ThrowsAsync<MovieDoesNotExistException>(async () => await helper.MovieService.UpdateAsync(testMovie2));
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