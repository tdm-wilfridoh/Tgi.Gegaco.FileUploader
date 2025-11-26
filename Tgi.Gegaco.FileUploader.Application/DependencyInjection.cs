using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MediatR;

namespace Tgi.Gegaco.FileUploader.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAMediatrApplication(this IServiceCollection services)
        {
            // Registrar servicios, handlers y otras dependencias de la aplicación aquí
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }

        public static IServiceCollection AddAutoMapperService(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
