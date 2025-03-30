using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

public class CustomFactory<T> : WebApplicationFactory<T> where T : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Override configuration for tests.
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            var testConfig = new Dictionary<string, string?>
            {
                // Override the connection string to use localhost for the docker container.
                { "MongoDb:ConnectionString", "mongodb://localhost:27017" },
                { "MongoDb:DatabaseName", "GameOfLife" }
            };
            configBuilder.AddInMemoryCollection(testConfig);
        });
    }
}