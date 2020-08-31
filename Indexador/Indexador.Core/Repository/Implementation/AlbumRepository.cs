using Indexador.Core.Entity.UserAlbum;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indexador.Core.Repository.Implementation
{
    public class AlbumRepository : IAlbumRepository
    {
        private const string INDEX_NAME = "requests_photos_vitorandrade";

        private string UrlElasticSearch { get => _configuration.GetSection("UrlElasticSearch").Value; }

        private readonly object lockClient = new object();
        private ElasticClient _client;
        private readonly IConfiguration _configuration;

        public AlbumRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task AdicionarOuAlterar(List<UserAlbum> users)
        {
            var client = ObterCliente();
            var response = await client.IndexManyAsync(users);
            if (!response.IsValid)
                throw new Exception("Erro ao indexar Albuns.");
        }

        public async Task Remover(List<ulong> ids)
        {
            var client = ObterCliente();
            var response = await client.DeleteManyAsync(ids.Select(i => new UserAlbum { UserId = i }));
            if (!response.IsValid)
                throw new Exception("Erro ao desindexar Albuns.");
        }

        public async Task<Dictionary<ulong, string>> ObterHashs()
        {
            var client = ObterCliente();
            var response = await client.SearchAsync<UserAlbum>(s =>
                s.Query(q => q.MatchAll())
                .Source(sf =>
                    sf.Includes(i =>
                        i.Fields(
                            x => x.UserId,
                            x => x.Hash
                        )
                    )
                )
            );

            return response.Documents?.ToDictionary(x => x.UserId, x => x.Hash) ?? new Dictionary<ulong, string>();
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
