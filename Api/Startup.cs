using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.IO;
using System.Security.Authentication;
using System.Text.Json;
using System.Xml.Xsl;

[assembly: FunctionsStartup(typeof(Api.Startup))]

namespace Api;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

        builder.Services
            .AddSingleton<JsonSerializerOptions>(new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            })
            .AddSingleton<Auth>()
            .AddSingleton<IConfiguration>(config);

        MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(config["COSMOS_DB_CONNECTION_STRING"]));
        settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

        builder.Services
            .AddSingleton((s) => new MongoClient(settings))
            .AddSingleton<IProductData, ProductDataDB>();
    }
}
