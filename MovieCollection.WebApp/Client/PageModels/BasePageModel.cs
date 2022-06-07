using Microsoft.AspNetCore.Components;
using MovieCollection.WebApp.Shared.Routes;

namespace MovieCollection.WebApp.Client.PageModels
{
    public class BasePageModel : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;
        [Inject]
        public HttpClient HttpClient { get; set; } = new HttpClient();

        public async Task Logout()
        {
            await HttpClient.PostAsync(MovieRoute.LogoutUser, null);
            NavigationManager.NavigateTo("Login", true);
        }
    }
}
