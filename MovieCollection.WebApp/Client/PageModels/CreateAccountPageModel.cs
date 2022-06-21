using Microsoft.AspNetCore.Components;
using MovieCollection.WebApp.Shared.Requests;
using MovieCollection.WebApp.Shared.Routes;
using MudBlazor;
using System.Net;
using System.Net.Http.Json;

namespace MovieCollection.WebApp.Client.PageModels
{
    public class CreateAccountPageModel : BasePageModel
    {
        public string ErrorMessage { get; set; } = string.Empty;

        protected MudForm form = new MudForm();
        protected string userName = string.Empty;
        protected string email = string.Empty;
        protected string password = string.Empty;
        protected string confirmPassword = string.Empty;
        protected bool isBusy = false;

        public async Task ClickCreateButton()
        {
            ErrorMessage = string.Empty;
            if (password != confirmPassword)
            {
                ErrorMessage = "Password and confirm password mismatch";
                return;
            }

            isBusy = true;
            StateHasChanged();
            await Create(new CreateUserRequest() { UserName = userName, Email = email, Password = password });
            isBusy = false;
            StateHasChanged();
        }

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
