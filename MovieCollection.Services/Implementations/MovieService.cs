using Microsoft.Extensions.Configuration;
using MovieCollection.Domain;
using MovieCollection.Domain.Exceptions;
using MovieCollection.Services.TmdbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieCollection.Services.Implementations
{
    public class MovieService : IMovieService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;

        public MovieService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _baseUrl = config["TmdbApi:BaseUrl"].TrimEnd('/');
            _apiKey = config["TmdbApi:ApiKey"];
        }

        public async Task<IEnumerable<Movie>> FindMoviesByNameAsync(string name)
        {
            if (name.Length < 3)
            {
                return new List<Movie>();
            }

            var response = await _httpClient.GetAsync(@$"{_baseUrl}/search/movie?api_key={_apiKey}&language=en-US&query={name}&page=1&include_adult=false");

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiCommunicationException();
            }

            var responseText = await response.Content.ReadAsStringAsync();
            
            try
            {
                var jsonResult = JsonSerializer.Deserialize<TmdbMovieResult>(responseText);

                if (jsonResult == null)
                {
                    return new List<Movie>();
                }

                return jsonResult.results.Select(m => m.ToMovie());
            }
            catch (JsonException)
            {
            }

            return new List<Movie>();
        }
    }
}
