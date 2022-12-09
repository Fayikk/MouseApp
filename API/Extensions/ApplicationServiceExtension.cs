using API.Data;
using API.Helper;
using API.Helpers;
using API.Interfaces;
using API.Repository;
using API.Services;
using API.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services , IConfiguration config){

            services.AddDbContext<DataContext>(opt=>{

                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
                    services.AddScoped<ITokenService , TokenService>();
                    services.AddScoped<IUserRepository,UserRepository>();
                    services.AddScoped<ILikeRepository,LikeRepository>();
                    services.AddScoped<IMessageRepository,MessageRepository>();
                    services.AddScoped<IUnitOfWork,UnitOfWork>();
                    services.AddScoped<LogUserActivity>();
                    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                    services.AddScoped<IImageService,ImageService>(); 
                    services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
                    services.AddSignalR();
                    services.AddSingleton<PresenceTracker>();
        
        return services;
        }
        
    }
}