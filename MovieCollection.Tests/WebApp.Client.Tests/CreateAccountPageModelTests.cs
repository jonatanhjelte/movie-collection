using MovieCollection.Tests.Helpers;
using MovieCollection.WebApp.Client.PageModels;
using MovieCollection.WebApp.Shared.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieCollection.Tests.WebApp.Client.Tests
{
    public class CreateAccountPageModelTests
    {
        private CreateAccountPageModel _model;
        private MockHttpMessageHandler _mockHandler;

        public CreateAccountPageModelTests()
        {
            _model = new CreateAccountPageModel();
            _mockHandler = new MockHttpMessageHandler();
            _model.HttpClient = new HttpClient(_mockHandler) { BaseAddress = new Uri(@"http:\\test.com")};
        }

        [Fact]
        public async Task Create_EmptyUserName_DoesNothing()
        {
            var request = new CreateUserRequest() { Password = "123", Email = "123" };
            
            await _model.Create(request);

            Assert.Empty(_mockHandler.RequestMessages);
        }

        [Fact]
        public async Task Create_EmptyPassword_DoesNothing()
        {
            var request = new CreateUserRequest() { UserName = "123", Email = "123" };

            await _model.Create(request);

            Assert.Empty(_mockHandler.RequestMessages);
        }

        [Fact]
        public async Task Create_EmptyEmail_DoesNothing()
        {
            var request = new CreateUserRequest() { UserName = "123", Password = "123" };

            await _model.Create(request);

            Assert.Empty(_mockHandler.RequestMessages);
        }

        [Fact]
        public async Task Create_ServerRespondsConflict_AddsErrorToModel()
        {
            var request = new CreateUserRequest() { UserName = "123", Password = "123", Email = "123" };
            _mockHandler.DefaultResponse = new HttpResponseMessage(System.Net.HttpStatusCode.Conflict);

            await _model.Create(request);

            Assert.Contains("USERNAME ALREADY EXISTS", _model.ErrorMessage.ToUpper());
        }

        [Fact]
        public async Task Create_ServerRespondsOk_RedirectsAndLogsIn()
        {
            var request = new CreateUserRequest() { UserName = "user123", Password = "password123", Email = "email123" };
            var mockNavMan = new MockNavigationManager();
            _model.NavigationManager = mockNavMan;

            await _model.Create(request);

            Assert.Equal(2, _mockHandler.RequestMessages.Count());
            var content = _mockHandler.RequestMessages.First().Content;
            if (content == null)
            {
                throw new ArgumentNullException("No content in request.");
            }
            var text = await content.ReadAsStringAsync();
            Assert.Contains(request.UserName, text);
            Assert.Contains(request.Password, text);
            Assert.Contains(request.Email, text);
            Assert.Contains("HOME", mockNavMan.LastCalledUri.ToUpper());
        }
    }
}
