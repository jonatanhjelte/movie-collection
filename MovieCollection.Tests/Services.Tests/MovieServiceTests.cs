using MovieCollection.Domain.Exceptions;
using MovieCollection.Services.Implementations;
using MovieCollection.Services.TmdbModels;
using MovieCollection.Tests.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieCollection.Tests.Services.Tests
{
    public class MovieServiceTests
    {
        private readonly string _baseUrl = @"http:\\test.com";
        private readonly string _apiKey = "123ApiKey123!";

        [Fact]
        public async Task FindMoviesByName_LessThan3Characters_ReturnsEmptyList()
        {
            using var helper = MakeHelper();

            var moviesOneChar = await helper.MovieService.FindMoviesByNameAsync("a");
            var moviesTwoChar = await helper.MovieService.FindMoviesByNameAsync("aa");

            Assert.Empty(moviesOneChar);
            Assert.Empty(moviesTwoChar);
        }

        [Theory]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.GatewayTimeout)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.TooManyRequests)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task FindMoviesByName_WebReturnsNotOk_ThrowsApiCommunicationException(HttpStatusCode code)
        {
            using var helper = MakeHelper();
            helper.MockHttpMessageHandler.DefaultResponse = new HttpResponseMessage(code);

            await Assert.ThrowsAsync<ApiCommunicationException>(
                async () => await helper.MovieService.FindMoviesByNameAsync("abcde"));
        }

        [Fact]
        public async Task FindMoviesByName_WebReturnsMovies_ServiceReturnsMovies()
        {
            using var helper = MakeHelper();
            var jsonMovie1 = new TmdbMovie() { imdb_id = "1234imdb", title = "1234titlenumber1" };
            var jsonMovie2 = new TmdbMovie() { imdb_id = "5678imdb", title = "1234titlenumber2" };
            var jsonResult = new TmdbMovieResult()
            {
                page = 1,
                total_pages = 1,
                total_results = 2,
                results = new List<TmdbMovie>()
                {
                    jsonMovie1,
                    jsonMovie2,
                }
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(jsonResult), Encoding.UTF8, @"application/json");
            helper.MockHttpMessageHandler.DefaultResponse = response;

            var movies = await helper.MovieService.FindMoviesByNameAsync("1234");

            Assert.Equal(movies.ElementAt(0).Name, jsonMovie1.title);
            Assert.Equal(movies.ElementAt(0).MovieDatabaseId, jsonMovie1.imdb_id);
            Assert.Equal(movies.ElementAt(1).Name, jsonMovie2.title);
            Assert.Equal(movies.ElementAt(1).MovieDatabaseId, jsonMovie2.imdb_id);
        }

        [Fact]
        public async Task FindMoviesByName_WebReturnsEmptyOk_ServiceReturnsEmptyList()
        {
            using var helper = MakeHelper();
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(string.Empty, Encoding.UTF8, @"application/json");
            helper.MockHttpMessageHandler.DefaultResponse = response;

            var movies = await helper.MovieService.FindMoviesByNameAsync("1234");

            Assert.Empty(movies);
        }

        [Fact]
        public async Task FindMoviesByName_SearchInput_QueriesCorrectUrl()
        {
            string movieSearchString = "9999";
            var helper = MakeHelper();
            helper.MockHttpMessageHandler.DefaultResponse = new HttpResponseMessage(HttpStatusCode.OK);

            var movies = await helper.MovieService.FindMoviesByNameAsync(movieSearchString);

            Assert.Contains(@"/search/movie", helper.MockHttpMessageHandler.RequestMessages.First().RequestUri?.ToString().ToLower());
            Assert.Contains(movieSearchString, helper.MockHttpMessageHandler.RequestMessages.First().RequestUri?.ToString().ToLower());
        }

        private MovieServiceTestHelper MakeHelper()
        {
            return new MovieServiceTestHelper(_baseUrl, _apiKey);
        }
    }
}
