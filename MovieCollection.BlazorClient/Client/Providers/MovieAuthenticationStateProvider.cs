using Microsoft.AspNetCore.Components.Authorization;

namespace MovieCollection.WebApp.Client.Providers
{
    public class MovieAuthenticationStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
