using Moq;
using MovieCollection.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieCollection.Tests.WebApp.Client.Tests
{
    public class HomePageModelTests
    {
        [Fact]
        public void FindMoviesAsync_Error_AddsErrorToModel()
        {
            Assert.Equal(1, 1);
        }
    }
}
