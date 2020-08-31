using Indexador.Core.Entity.UserAlbum;
using System;
using System.Collections.Generic;
using UserAlbumEntity = Indexador.Core.Entity.UserAlbum.UserAlbum;

namespace Indexador.Teste.TesteUnidade.UserAlbum
{
    public static class UtilUserAlbum
    {
        private static Random rng = new Random();

        public static UserAlbumEntity ObterUserAlbum()
        {
            var albuns = new List<Album>();

            for (var i = 0; i < 10; i++)
            {
                var fotos = new List<AlbumFoto>();
                for (var j = 0; j < 5; j++)
                {
                    fotos.Add(new AlbumFoto
                    {
                        PhotoId = Convert.ToUInt64(j + 1),
                        Title = $"Titulo Foto {j}",
                        ThumbnailUrl = $"http://thumbnailurl/{j}",
                        Url = $"http://url/{j}",
                    });
                }

                albuns.Add(new Album
                {
                    AlbumId = Convert.ToUInt64(i + 1),
                    AlbumTitle = $"Titulo {i + 1}",
                    Photos = fotos
                });
            }

            return new UserAlbumEntity
            {
                UserId = 1,
                Albums = albuns
            };
        }

        public static void EmbaralharLista<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
