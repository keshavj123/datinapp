using API.Interfaces;
using API.Services;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration; 

namespace API.Extensions 
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
                services.AddScoped<ITokenService, TokenService>();

                services.AddDbContext<DataContext>(options =>
            
                options.UseSqlite("Data source=datingapp.db")
            );
 
            return services;

        }
    }
}