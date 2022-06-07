using Microsoft.AspNetCore.Components;
using MovieCollection.WebApp.Shared.Requests;
using MovieCollection.WebApp.Shared.Routes;
using System.Net.Http.Json;

namespace MovieCollection.WebApp.Client.PageModels
{
    public class LoginPageModel : BasePageModel
    {
        public string ErrorMessage { get; set; } = string.Empty;

        public async Task Login(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) 
                || string.IsNullOrWhiteSpace(request.Password))
            {
                return;
            }

            var response = await HttpClient.PostAsJsonAsync(MovieRoute.LoginUser, request);

            if (response.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo("home", true);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ErrorMessage = "Invalid username or password";
            }
            else
            {
                ErrorMessage = "Something went wrong, please try again later";
            }
        }

        public void ClearErrorMessage()
        {
            ErrorMessage = string.Empty;
        }
    }
}
