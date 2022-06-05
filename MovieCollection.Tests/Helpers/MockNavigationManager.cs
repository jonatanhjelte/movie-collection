using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Tests.Helpers
{
    public class MockNavigationManager : NavigationManager
    {
        public string LastCalledUri { get; private set; } = string.Empty;

        public MockNavigationManager()
        {
            Initialize("https://test.com/", "https://test.com/");
        }


        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            LastCalledUri = uri;
        }

        protected override void NavigateToCore(string uri, NavigationOptions options)
        {
            LastCalledUri = uri;
        }
    }
}
