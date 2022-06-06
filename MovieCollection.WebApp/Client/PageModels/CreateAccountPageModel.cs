using Microsoft.AspNetCore.Components;
using MovieCollection.WebApp.Shared.Requests;
using MovieCollection.WebApp.Shared.Routes;
using System.Net;
using System.Net.Http.Json;

namespace MovieCollection.WebApp.Client.PageModels
{
    public class CreateAccountPageModel : BasePageModel
    {
        [Inject]
        public HttpClient HttpClient { get; set; } = new HttpClient();
        public string ErrorMessage { get; set; } = string.Empty;

        public async Task Create(CreateUserRequest request)
        {
            if (string.IsNullOrEmpty(request.UserName)
                || string.IsNullOrEmpty(request.Password)
                || string.IsNullOrEmpty(request.Email))
            {
                return;
            }

            var response = await HttpClient.PostAsJsonAsync(MovieRoute.CreateUser, request);

            if (response.IsSuccessStatusCode)
            {
                await HttpClient.PostAsJsonAsync(MovieRoute.LoginUser, new LoginRequest() { UserName = request.UserName, Password = request.Password});
                NavigationManager.NavigateTo("Home");
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                ErrorMessage = "Username already exists";
            }
        }
    }
}
