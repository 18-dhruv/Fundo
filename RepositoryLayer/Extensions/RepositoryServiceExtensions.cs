using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;

namespace RepositoryLayer.Extensions
{
    public static class RepositoryServiceExtensions
    {
      
        public static IServiceCollection AddRepositoryLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<FundooContext>(options => options.UseNpgsql(connectionString));

            services.AddScoped<IUserRL, UserRL>();

            return services;
        }
    }
}
