using AlbumServer.Core.DAO;
using AlbumServer.Core.DAO.Implementation;
using AlbumServer.Core.Service;
using AlbumServer.Core.Service.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace AlbumServer.Core.Conf
{
    public static class ConfigureCore
    {
        public static IServiceCollection ConfigureCoreDI(this IServiceCollection services)
        {
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IAlbumDao, AlbumDao>();

            return services;
        }
    }
}
