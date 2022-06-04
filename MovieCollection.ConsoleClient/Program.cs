using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieCollection.Services;
using MovieCollection.Services.Implementations;


using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        RegisterServices(services))
    .Build();




void RegisterServices(IServiceCollection services)
{
    services.AddSingleton<IUserService, UserService>();
}