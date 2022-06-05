﻿using Microsoft.AspNetCore.Components;
using MovieCollection.WebApp.Shared.Requests;
using System.Net.Http.Json;

namespace MovieCollection.WebApp.Client.PageModels
{
    public class LoginPageModel : BasePageModel
    {
        [Inject]
        public HttpClient HttpClient { get; set; } = new HttpClient();
        public string ErrorMessage { get; set; } = string.Empty;

        public async Task Login(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) 
                || string.IsNullOrWhiteSpace(request.Password))
            {
                return;
            }

            var response = await HttpClient.PostAsJsonAsync("login", request);

            if (response.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo("home", true);
            }
            else
            {
                ErrorMessage = "Invalid username or password";
            }
        }

        public void ClearErrorMessage()
        {
            ErrorMessage = string.Empty;
        }
    }
}