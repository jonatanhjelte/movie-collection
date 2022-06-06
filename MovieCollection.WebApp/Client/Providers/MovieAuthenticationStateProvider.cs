using Microsoft.AspNetCore.Components.Authorization;
using MovieCollection.Domain;
using MovieCollection.WebApp.Shared.Routes;
using System.Net.Http.Json;
using System.Security.Claims;

namespace MovieCollection.WebApp.Client.Providers
{
    public class MovieAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;

        public MovieAuthenticationStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            User? currentUser = null;
            try
            {
                currentUser = await _httpClient.GetFromJsonAsync<User>(MovieRoute.CurrentUser);

                if (currentUser != null && !string.IsNullOrEmpty(currentUser.UserName))
                {
                    var claim = new Claim(ClaimTypes.Name, currentUser.UserName);
                    var claimsIdentity = new ClaimsIdentity(new[] { claim }, "serverAuth");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    return new AuthenticationState(claimsPrincipal);
                }

                throw new UnauthorizedAccessException();
            }
            catch (HttpRequestException)
            {
                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
