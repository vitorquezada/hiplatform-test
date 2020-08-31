using System;
using Xunit;


namespace Indexador.Teste.TesteUnidade.UserAlbum
{
    public class UserAlbumEntityTests
    {
        public UserAlbumEntityTests()
        {

        }

        [Fact]
        public void CalcularHash_AlterandoOrdenacao_Test()
        {
            var user = UtilUserAlbum.ObterUserAlbum();

            user.CalcularHash();
            var hash1 = user.Hash;

            UtilUserAlbum.EmbaralharLista(user.Albums);
            for (var i = 0; i < user.Albums.Count; i++) UtilUserAlbum.EmbaralharLista(user.Albums[i].Photos);

            user.CalcularHash();
            var hash2 = user.Hash;

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void CalcularHash_RemovendoFoto_Test()
        {
            var rng = new Random();

            var user = UtilUserAlbum.ObterUserAlbum();

            user.CalcularHash();
            var hash1 = user.Hash;

            var indexAlbum = rng.Next(user.Albums.Count);
            var indexFoto = rng.Next(user.Albums[indexAlbum].Photos.Count);

            user.Albums[indexAlbum].Photos.RemoveAt(indexFoto);

            user.CalcularHash();
            var hash2 = user.Hash;

            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void CalcularHash_AlterandoUrlFoto_Test()
        {
            var rng = new Random();

            var user = UtilUserAlbum.ObterUserAlbum();

            user.CalcularHash();
            var hash1 = user.Hash;

            var indexAlbum = rng.Next(user.Albums.Count);
            var indexFoto = rng.Next(user.Albums[indexAlbum].Photos.Count);
            user.Albums[indexAlbum].Photos[indexFoto].Url = "http://urlalterada";

            user.CalcularHash();
            var hash2 = user.Hash;

            Assert.NotEqual(hash1, hash2);
        }
    }
}
