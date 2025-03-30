using GameOfLife.Application.Interfaces;
using GameOfLife.Infrastructure.Persistence;
using GameOfLife.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameOfLife.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDb"));
            services.AddSingleton<MongoDbContext>();
            services.AddScoped<IBoardRepository, BoardRepository>();

            return services;
        }
    }
}