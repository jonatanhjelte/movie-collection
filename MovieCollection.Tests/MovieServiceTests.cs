using Xunit;
using MovieCollection.Services;
using System.Threading.Tasks;
using MovieCollection.Domain;
using MovieCollection.Domain.Exceptions;
using System.Linq;
using System;

namespace MovieCollection.Tests
{
    public class MovieServiceTests
    {
        private readonly MovieService service = new MovieService();
        private readonly Movie testMovie = MakeTestMovie();

        [Fact]
        public async Task CreateService_IsEmpty()
        {
            var movies = await service.GetAllAsync();

            Assert.Empty(movies);
        }

        [Fact]
        public async Task AddMovieThenRemoveMovie_IsEmpty()
        {
            await service.AddAsync(testMovie);
            await service.RemoveAsync(testMovie);
            var movies = await service.GetAllAsync();

            Assert.Empty(movies);
        }

        [Fact]
        public async Task RemoveMovieThatDoesNotExist_ThrowsMovieDoesNotExistException()
        {
            await Assert.ThrowsAsync<MovieDoesNotExistException>(async () => await service.RemoveAsync(testMovie));
        }

        [Fact]
        public async Task Add2MoviesThenRemove1_GetAllOnlyReturns1()
        {
            var testMovie2 = MakeTestMovie();

            await service.AddAsync(testMovie);
            await service.AddAsync(testMovie2);
            await service.RemoveAsync(testMovie);
            var movies = await service.GetAllAsync();

            Assert.Single(movies);
            Assert.Equal(testMovie2, movies.ElementAt(0));
        } 

        [Fact]
        public async Task AddMovieThenUpdateMovie_GetAllReturnsUpdatedMovie()
        {
            var updatedMovie = testMovie with { Name = $"{testMovie.Name}UPDATED" };

            await service.AddAsync(testMovie);
            await service.UpdateAsync(updatedMovie);
            var movies = await service.GetAllAsync();

            Assert.Equal(updatedMovie, movies.First(m => m.Id == testMovie.Id));
        }

        [Fact]
        public async Task UpdateMovieThatDoesNotExist_ThrowsMovieDoesNotExistException()
        {
            var testMovie2 = MakeTestMovie();

            await service.AddAsync(testMovie);
            
            await Assert.ThrowsAsync<MovieDoesNotExistException>(async () => await service.UpdateAsync(testMovie2));
        }

        private static Movie MakeTestMovie()
        {
            var rnd = new Random();

            return new Movie()
            {
                Id = rnd.Next(),
                Name = Guid.NewGuid().ToString(),
            };
        }
    }
}