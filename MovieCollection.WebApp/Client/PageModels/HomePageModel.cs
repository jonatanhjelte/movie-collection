using MovieCollection.Domain;
using MovieCollection.WebApp.Shared.Routes;
using System.Text.Json;

namespace MovieCollection.WebApp.Client.PageModels
{
    public class HomePageModel : BasePageModel
    {
        public async Task<IEnumerable<Movie>> FindMoviesAsync(string name)
        {
            if (name.Length < 4)
            {
                return Enumerable.Empty<Movie>();
            }
            
            var response = await HttpClient.GetAsync(MovieRoute.FindMovie + $"?name={name}");
            var text = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode
                || string.IsNullOrEmpty(text))
            {
                ErrorMessage = "API Communication error";
                return Enumerable.Empty<Movie>();
            }

            var movies = JsonSerializer.Deserialize<IEnumerable<Movie>>(text);

            return movies ?? Enumerable.Empty<Movie>();
        }
    }
}
