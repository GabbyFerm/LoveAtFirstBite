// Infrastructure/ServiceCollectionExtensions.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
             this IServiceCollection services,
             IConfiguration configuration)
        {
            // EF Core
            services.AddDbContext<LoveAtFirstBiteDbContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Register your repository
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();

            return services;
        }
    }
}
