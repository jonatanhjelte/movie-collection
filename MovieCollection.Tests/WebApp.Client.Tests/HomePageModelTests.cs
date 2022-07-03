using Moq;
using MovieCollection.Domain;
using MovieCollection.Services;
using MovieCollection.Tests.Helpers;
using MovieCollection.WebApp.Client.PageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace MovieCollection.Tests.WebApp.Client.Tests
{
    public class HomePageModelTests
    {
        private readonly HomePageModel _model;
        private readonly MockHttpMessageHandler _mockHandler;
        private readonly List<Movie> testMovies = new List<Movie>()
        {
            new Movie()
            {
                Name = "1234",
                MovieDatabaseId = "t1234",
            },
            new Movie()
            {
                Name = "4444",
                MovieDatabaseId = "t5555",
            }
        };

        public HomePageModelTests()
        {
            _model = new HomePageModel();
            _mockHandler = new MockHttpMessageHandler();
            _model.HttpClient = new HttpClient(_mockHandler) { BaseAddress = new Uri(@"http:\\test.com") };
        }

        [Fact]
        public async Task FindMoviesAsync_NameLessThan4Characters_DoesNotQuery()
        {
            var movies = await _model.FindMoviesAsync("123");

            Assert.Empty(_mockHandler.RequestMessages);
            Assert.Empty(movies);
        }

        [Fact]
        public async Task FindMoviesAsync_HttpIsNotSuccess_AddsErrorToModel()
        {
            _mockHandler.DefaultResponse = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);

            var movies = await _model.FindMoviesAsync("1234");

            Assert.Contains("COMMUNICATION ERROR", _model.ErrorMessage.ToUpper());
        }

        [Fact]
        public async Task FindMoviesAsync_HttpGetsMovieList_ReturnsMovieList()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonSerializer.Serialize(testMovies), Encoding.UTF8, @"application/json");
            _mockHandler.DefaultResponse = response;

            var movies = await _model.FindMoviesAsync("1234");

            Assert.Equal(testMovies, movies);
        }

        [Fact]
        public async Task FindMoviesAsync_ValidInput_QueriesForName()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonSerializer.Serialize(testMovies), Encoding.UTF8, @"application/json");
            _mockHandler.DefaultResponse = response;

            var movies = await _model.FindMoviesAsync("queryparam");

            Assert.Contains("?NAME=QUERYPARAM", _mockHandler.RequestMessages.First().RequestUri?.ToString().ToUpper());
        }

        [Fact]
        public async Task FindMoviesAsync_HttpReturnsOkWithNoJson_ReturnsEmptyListAndAddsError()
        {
            _mockHandler.DefaultResponse = new HttpResponseMessage(HttpStatusCode.OK);

            var movies = await _model.FindMoviesAsync("1234");

            Assert.Empty(movies);
            Assert.Contains("COMMUNICATION ERROR", _model.ErrorMessage.ToUpper());
        }
    }
}
