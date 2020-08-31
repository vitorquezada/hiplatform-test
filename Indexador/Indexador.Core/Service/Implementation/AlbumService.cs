using Indexador.Core.Entity.UserAlbum;
using Indexador.Core.Model;
using Indexador.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Indexador.Core.Service.Implementation
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumService(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        public async Task IndexarAlbuns()
        {
            var albunsTask = ObterAlbuns();
            var fotosTask = ObterFotos();
            var usuarios = ObterListaUsuarios(await albunsTask, await fotosTask);

            var usuariosHashElasticsearchTask = ObterHashUsuarios();

            for (var i = 0; i < usuarios.Count; i++)
                usuarios[i].CalcularHash();

            var usuariosHashElasticsearch = await usuariosHashElasticsearchTask;

            var usuariosIndexar = ObterUsuariosIndexar(usuariosHashElasticsearch, usuarios);
            var usuariosDeletar = ObterUsuariosDesindexar(usuariosHashElasticsearch, usuarios);

            if (usuariosIndexar != null && usuariosIndexar.Any())
                await _albumRepository.AdicionarOuAlterar(usuariosIndexar);

            if (usuariosDeletar != null && usuariosDeletar.Any())
                await _albumRepository.Remover(usuariosDeletar);
        }

        private async Task<Dictionary<ulong, string>> ObterHashUsuarios()
        {
            return await _albumRepository.ObterHashs();
        }

        public List<ulong> ObterUsuariosDesindexar(Dictionary<ulong, string> dicionarioUsuariosHash, List<UserAlbum> listaUsuarios)
        {
            if (dicionarioUsuariosHash == null || !dicionarioUsuariosHash.Any())
                return new List<ulong>();

            if (listaUsuarios == null || !listaUsuarios.Any())
                return dicionarioUsuariosHash.Keys.ToList();

            return dicionarioUsuariosHash.Where(uh => !listaUsuarios.Any(u => u.UserId == uh.Key)).Select(u => u.Key).ToList();
        }

        public List<UserAlbum> ObterUsuariosIndexar(Dictionary<ulong, string> dicionarioUsuariosHash, List<UserAlbum> listaUsuarios)
        {
            if (listaUsuarios == null || !listaUsuarios.Any())
                return new List<UserAlbum>();

            if (dicionarioUsuariosHash == null || !dicionarioUsuariosHash.Any())
                return listaUsuarios;

            return listaUsuarios.Where(u => !dicionarioUsuariosHash.ContainsKey(u.UserId) || dicionarioUsuariosHash[u.UserId] != u.Hash).ToList();
        }

        public List<UserAlbum> ObterListaUsuarios(List<AlbumApiModel> albuns, List<FotoApiModel> fotos)
        {
            albuns ??= new List<AlbumApiModel>();
            fotos ??= new List<FotoApiModel>();

            var qtdParalelismo = 2 * Environment.ProcessorCount;

            var dicFotosPorAlbuns = fotos
                .AsParallel()
                .WithDegreeOfParallelism(qtdParalelismo)
                .Where(x => x != null)
                .GroupBy(x => x.AlbumId)
                .ToDictionary(x => x.Key, x => x.Select(f => new AlbumFoto
                {
                    PhotoId = f.Id,
                    Title = f.Title,
                    Url = f.Url,
                    ThumbnailUrl = f.ThumbnailUrl,
                }).ToList());

            var albunsNormalizadosParallel = albuns
                .AsParallel()
                .WithDegreeOfParallelism(qtdParalelismo)
                .Where(x => x != null)
                .GroupBy(x => x.Id)
                .Select(x => x.FirstOrDefault());

            var dicUsuarioAlbuns = albunsNormalizadosParallel
                .GroupBy(x => x.UserId)
                .ToDictionary(x => x.Key, x => x.Select(a => new Album
                {
                    AlbumId = a.Id,
                    AlbumTitle = a.Title,
                    Photos = dicFotosPorAlbuns.ContainsKey(a.Id) ? dicFotosPorAlbuns[a.Id] : new List<AlbumFoto>(),
                }).ToList());

            return dicUsuarioAlbuns
                .AsParallel()
                .WithDegreeOfParallelism(qtdParalelismo)
                .Select(x => new UserAlbum
                {
                    UserId = x.Key,
                    Albums = x.Value
                })
                .ToList();
        }

        private async Task<List<AlbumApiModel>> ObterAlbuns()
        {
            var albuns = await GetUrl<List<AlbumApiModel>>("https://jsonplaceholder.typicode.com/albums");
            return albuns;
        }

        private async Task<List<FotoApiModel>> ObterFotos()
        {
            var fotos = await GetUrl<List<FotoApiModel>>("https://jsonplaceholder.typicode.com/photos");
            return fotos;
        }

        private async Task<T> GetUrl<T>(string url)
        {
            using var httpClient = new HttpClient();
            var request = await httpClient.GetAsync(url);
            if (request.IsSuccessStatusCode)
            {
                var bodyStream = await request.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<T>(bodyStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return default;
        }
    }
}
