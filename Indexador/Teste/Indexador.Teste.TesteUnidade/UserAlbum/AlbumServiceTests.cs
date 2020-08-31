using Indexador.Core.Model;
using Indexador.Core.Repository;
using Indexador.Core.Service.Implementation;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Indexador.Teste.TesteUnidade.UserAlbum
{
    public class AlbumServiceTests
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumServiceTests()
        {
            var mock = new Mock<IAlbumRepository>();


            _albumRepository = mock.Object;
        }

        #region " DATA ObterUsuariosDesindexarTest "

        public static IEnumerable<object[]> UsuariosDesindexarParametrosTest()
        {
            yield return new object[]
            {
                new Dictionary<ulong, string>(),
                new List<Core.Entity.UserAlbum.UserAlbum>(){ UtilUserAlbum.ObterUserAlbum() },
                0
            };

            yield return new object[]
            {
                null,
                new List<Core.Entity.UserAlbum.UserAlbum>(){ UtilUserAlbum.ObterUserAlbum() },
                0
            };

            yield return new object[]
            {
                new Dictionary<ulong, string>
                {
                    { 90, "hashTeste" },
                    { 91, "hashTeste" },
                    { 92, "hashTeste" },
                },
                new List<Core.Entity.UserAlbum.UserAlbum>(){ UtilUserAlbum.ObterUserAlbum() },
                3
            };

            yield return new object[]
            {
                new Dictionary<ulong, string>
                {
                    { 1, "hashTeste" },
                    { 2, "hashTeste" },
                    { 3, "hashTeste" },
                },
                null,
                3
            };
        }

        #endregion

        [Theory]
        [MemberData(nameof(UsuariosDesindexarParametrosTest))]
        public void ObterUsuariosDesindexarTest(Dictionary<ulong, string> dicionarioUsuariosHash, List<Core.Entity.UserAlbum.UserAlbum> listaUsuarios, int resultadoEsperado)
        {
            AlbumService albumService = new AlbumService(_albumRepository);

            if (listaUsuarios != null)
            {
                for (var i = 0; i < listaUsuarios.Count; i++)
                    listaUsuarios[i].CalcularHash();
            }

            var resultado = albumService.ObterUsuariosDesindexar(dicionarioUsuariosHash, listaUsuarios);

            Assert.NotNull(resultado);
            Assert.Equal(resultadoEsperado, resultado.Count);
        }

        #region " DATA ObterUsuariosIndexarTest "

        public static IEnumerable<object[]> UsuariosIndexarParametrosTest()
        {
            var listaUser = new List<Core.Entity.UserAlbum.UserAlbum>() { UtilUserAlbum.ObterUserAlbum() };

            yield return new object[]
            {
                new Dictionary<ulong, string>(),
                listaUser,
                1
            };

            yield return new object[]
            {
                null,
                listaUser,
                1
            };

            yield return new object[]
            {
                new Dictionary<ulong, string>
                {
                    { 1, "hashTeste" },
                },
                listaUser,
                1
            };

            yield return new object[]
            {
                new Dictionary<ulong, string>
                {
                    { 1, "hashTeste" },
                    { 2, "hashTeste" },
                    { 3, "hashTeste" },
                },
                null,
                0
            };

            yield return new object[]
            {
                null,
                new List<Core.Entity.UserAlbum.UserAlbum>(),
                0
            };
        }

        #endregion

        [Theory]
        [MemberData(nameof(UsuariosIndexarParametrosTest))]
        public void ObterUsuariosIndexarTest(Dictionary<ulong, string> dicionarioUsuariosHash, List<Core.Entity.UserAlbum.UserAlbum> listaUsuarios, int resultadoEsperado)
        {
            AlbumService albumService = new AlbumService(_albumRepository);

            if (listaUsuarios != null)
            {
                for (var i = 0; i < listaUsuarios.Count; i++)
                    listaUsuarios[i].CalcularHash();
            }

            var resultado = albumService.ObterUsuariosIndexar(dicionarioUsuariosHash, listaUsuarios);

            Assert.NotNull(resultado);
            Assert.Equal(resultadoEsperado, resultado.Count);
        }

        #region " DATA ObterListaUsuariosTest "

        public static IEnumerable<object[]> ObterListaUsuariosParametrosTest()
        {
            yield return new object[]
            {
                new List<AlbumApiModel>
                {
                    new AlbumApiModel { Id = 1, UserId = 1 },
                    new AlbumApiModel { Id = 2, UserId = 1 },
                    new AlbumApiModel { Id = 3, UserId = 1 },
                    new AlbumApiModel { Id = 4, UserId = 2 },
                },
                new List<FotoApiModel>
                {
                    new FotoApiModel { Id = 1, AlbumId = 1 },
                    new FotoApiModel { Id = 2, AlbumId = 1 },
                    new FotoApiModel { Id = 3, AlbumId = 2 },
                    new FotoApiModel { Id = 4, AlbumId = 2 },
                    new FotoApiModel { Id = 5, AlbumId = 3 },
                    new FotoApiModel { Id = 6, AlbumId = 4 },
                    new FotoApiModel { Id = 7, AlbumId = 4 },
                },
                2
            };

            yield return new object[]
            {
                new List<AlbumApiModel> 
                {
                    new AlbumApiModel { Id = 1, UserId = 1 },
                },
                new List<FotoApiModel>
                {
                    new FotoApiModel { Id = 1, AlbumId = 1 },
                },
                1
            };

            yield return new object[]
            {
                new List<AlbumApiModel>
                {
                    new AlbumApiModel { Id = 1, UserId = 1 },
                },
                null,
                1
            };

            yield return new object[]
            {
                null,
                new List<FotoApiModel>
                {
                    new FotoApiModel { Id = 1, AlbumId = 1 },
                },
                0
            };
        }

        #endregion

        [Theory]
        [MemberData(nameof(ObterListaUsuariosParametrosTest))]
        public void ObterListaUsuariosTest(List<AlbumApiModel> listaAlbuns, List<FotoApiModel> listaFotos, int resultadoEsperado)
        {
            AlbumService albumService = new AlbumService(_albumRepository);
            var resultado = albumService.ObterListaUsuarios(listaAlbuns, listaFotos);

            Assert.NotNull(resultado);
            Assert.Equal(resultadoEsperado, resultado.Count);
        }
    }
}
