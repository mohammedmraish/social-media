using Microsoft.EntityFrameworkCore;
using social_media.Data;
using soicalMedia.Data;
using soicalMedia.Helpers;
using soicalMedia.Interfaces;
using soicalMedia.Services;

namespace soicalMedia.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILikesRepository, LikesRepository>();
            services.AddScoped<IMessageRepository ,MassageRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            services.AddDbContext<DataContext>(opions =>
            {
                opions.UseSqlServer(config.GetConnectionString("Default"));
            });

            return services;
        }
    }
}
