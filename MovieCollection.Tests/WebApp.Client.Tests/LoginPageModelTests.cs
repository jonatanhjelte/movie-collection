using Moq;

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
    public class LoginPageModelTests
    {
        private readonly LoginPageModel _model;
        private readonly MockHttpMessageHandler _mockHandler;

        public LoginPageModelTests()
        {
            _model = new LoginPageModel();
            _mockHandler = new MockHttpMessageHandler();
            _model.HttpClient = new HttpClient(_mockHandler) { BaseAddress = new Uri(@"http:\\test.com")};
        }

        [Fact]
        public async Task Login_EmptyUserName_DoesNothing()
        {
            await _model.Login(new LoginRequest() { UserName = string.Empty, Password = "123" });

            Assert.Empty(_mockHandler.RequestMessages);
        }

        [Fact]
        public async Task Login_EmptyPassword_DoesNothing()
        {
            await _model.Login(new LoginRequest() { UserName = "123", Password = string.Empty });

            Assert.Empty(_mockHandler.RequestMessages);
        }

        [Fact]
        public async Task Login_UserNameAndPassword_SendsProperRequestAndRedirects()
        {            
            var mockNavMan = new MockNavigationManager();
            _model.NavigationManager = mockNavMan;

            await _model.Login(new LoginRequest() { UserName = "user123", Password = "password123" });

            Assert.Single(_mockHandler.RequestMessages);
            var content = _mockHandler.RequestMessages.First().Content;
            if (content == null)
            {
                throw new ArgumentNullException("No content in request.");
            }
            var text = await content.ReadAsStringAsync();
            Assert.Contains("user123", text);
            Assert.Contains("password123", text);
            Assert.Contains("HOME", mockNavMan.LastCalledUri.ToUpper());
        }

        [Fact]
        public async Task Login_UserNameAndPasswordGivesUnauthorized_AddsErrorToModel()
        {            
            _mockHandler.DefaultResponse = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);

            await _model.Login(new LoginRequest() { UserName = "user123", Password = "password123" });

            Assert.Contains("INVALID USERNAME OR PASSWORD", _model.ErrorMessage.ToUpper());
        }

        [Fact]
        public void ClearErrorMessage_WhenCalled_ClearsErrorMessage()
        {
            var pageModel = new LoginPageModel();
            pageModel.ErrorMessage = "TEST";

            pageModel.ClearErrorMessage();

            Assert.Equal(string.Empty, pageModel.ErrorMessage);
        }
    }
}
