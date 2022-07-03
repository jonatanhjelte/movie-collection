using Microsoft.Extensions.Configuration;
using MovieCollection.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Tests.Helpers
{
    internal class MovieServiceTestHelper : BaseTestHelper
    {
        public readonly MovieService MovieService;
        public readonly MockHttpMessageHandler MockHttpMessageHandler;

        public MovieServiceTestHelper(string baseUrl, string apiKey)
            : base()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"TmdApi:BaseUrl", baseUrl},
                {"TmdApi:ApiKey", apiKey},
                };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            MockHttpMessageHandler = new MockHttpMessageHandler();
            MovieService = new MovieService(
                new HttpClient(MockHttpMessageHandler),
                configuration);
        }
    }
}
