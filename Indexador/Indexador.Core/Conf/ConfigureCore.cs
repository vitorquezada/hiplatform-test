using Indexador.Core.Repository;
using Indexador.Core.Repository.Implementation;
using Indexador.Core.Service;
using Indexador.Core.Service.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Indexador.Core.Conf
{
    public static class ConfigureCore
    {
        public static IServiceCollection ConfigureCoreDI(this IServiceCollection services)
        {
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IAlbumRepository, AlbumRepository>();

            return services;
        }
    }
}
