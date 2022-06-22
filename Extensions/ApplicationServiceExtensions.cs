using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using g2hotel_server.Data;
using g2hotel_server.Helper;
using g2hotel_server.Services.Implements;
using g2hotel_server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace g2hotel_server.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddMemoryCache();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            // builder.Services.AddDbContext<DataContext>(options =>
            //     options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(config.GetConnectionString("SqlServerConnection")));

            return services;
        }
    }
}