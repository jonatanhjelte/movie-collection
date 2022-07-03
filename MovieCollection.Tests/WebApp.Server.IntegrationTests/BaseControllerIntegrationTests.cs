using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MovieCollection.Repositories;
using MovieCollection.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Tests.WebApp.Server.IntegrationTests
{
    public class BaseControllerIntegrationTests
    {
        protected HttpClient SetupTestAndGetClient(
            IUserService? userServiceMock = null,
            IAuthenticationService? authMock = null,
            IMovieService? movieServiceMock = null)
        {
            if (userServiceMock == null)
            {
                userServiceMock = new Mock<IUserService>().Object;
            }

            if (movieServiceMock == null)
            {
                movieServiceMock = new Mock<IMovieService>().Object;
            }

            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, configBuilder) =>
                    {
                        configBuilder.AddInMemoryCollection(
                            new Dictionary<string, string>
                            {
                                     {"ConnectionStrings:Database", $"Data Source=test.db"},
                                     {"TmdbApi:BaseUrl", @"http://testurl.com/"},
                                     {"TmdbApi:ApiKey", @"ApiKey123!"},
                            });
                    });

                    builder.ConfigureServices(services =>
                    {
                        var movieContextOptionsDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MovieContext>));
                        if (movieContextOptionsDescriptor != null)
                        {
                            services.Remove(movieContextOptionsDescriptor);
                        }

                        var movieContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(MovieContext));
                        if (movieContextDescriptor != null)
                        {
                            services.Remove(movieContextDescriptor);
                        }
                        
                        services.AddDbContext<MovieContext, FileMovieContext>();
                        services.AddScoped(p => userServiceMock);
                        services.AddScoped(p => movieServiceMock);

                        if (authMock != null)
                        {
                            services.AddSingleton(p => authMock);
                        }
                    });
                });

            var client = application.CreateClient();

            return client;
        }
    }
}
