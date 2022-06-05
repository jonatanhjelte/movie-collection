using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MovieCollection.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Tests.WebApp.Client.Tests
{
    public class BaseTestPage : TestContext
    {
        protected readonly TestAuthorizationContext _authContext;
        protected readonly MockNavigationManager _mockNavMan;

        public BaseTestPage()
        {
            _authContext = this.AddTestAuthorization();
            _mockNavMan = new MockNavigationManager();

            Services.AddSingleton<NavigationManager>(_mockNavMan);
        }
    }
}
