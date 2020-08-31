using AlbumServer.Core.Model.UserAlbum;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Threading.Tasks;

namespace AlbumServer.Core.DAO.Implementation
{
    public class AlbumDao : IAlbumDao
    {
        private const string INDEX_NAME = "requests_photos_vitorandrade";

        private string UrlElasticSearch { get => _configuration.GetSection("UrlElasticSearch").Value; }

        private readonly object lockClient = new object();
        private ElasticClient _client;
        private readonly IConfiguration _configuration;

        public AlbumDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<UserAlbum> ObterAlbuns(ulong userId)
        {
            var client = ObterCliente();
            var response = await client.GetAsync<UserAlbum>(new DocumentPath<UserAlbum>(new UserAlbum() { UserId = userId }));
            if (!response.IsValid)
                throw new Exception("Erro ao obter documento.");

            return response.Source;
        }

        private ElasticClient ObterCliente()
        {
            lock (lockClient)
            {
                if (_client == null)
                {
                    var settings = new ConnectionSettings(new Uri(UrlElasticSearch))
                        .DefaultMappingFor<UserAlbum>(x =>
                            x.IndexName(INDEX_NAME)
                            .IdProperty(nameof(UserAlbum.UserId))
                        );
                    _client = new ElasticClient(settings);
                }
            }

            return _client;
        }
    }
}
