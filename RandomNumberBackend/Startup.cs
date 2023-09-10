using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using RandomNumberBackend.Database;
using RandomNumberBackend.Game;

[assembly: FunctionsStartup(typeof(RandomNumberBackend.Startup))]
namespace RandomNumberBackend
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IDatabase>(new DatabaseLocal());
            builder.Services.AddSingleton<INumberGenerator>(new NumberGenerator(1, 100));
        }
    }
}
