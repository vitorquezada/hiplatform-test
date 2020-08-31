using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Indexador.Core.Entity.UserAlbum
{
    public class UserAlbum
    {
        public ulong UserId { get; set; }
        public IList<Album> Albums { get; set; } = new List<Album>();
        public string Hash { get; set; }

        public void CalcularHash()
        {
            var stringHash = $"{UserId}";

            if (Albums != null && Albums.Any())
            {
                foreach (var album in Albums.OrderBy(x => x.AlbumId))
                {
                    stringHash += $"{album.AlbumId}{album.AlbumTitle}";

                    if (album.Photos != null && album.Photos.Any())
                    {
                        var fotosHash = album.Photos.OrderBy(x => x.PhotoId).Select(x => $"{x.PhotoId}{x.Title}{x.Url}{x.ThumbnailUrl}");
                        stringHash += string.Join(string.Empty, fotosHash);
                    }
                }
            }

            var bytes = Encoding.UTF8.GetBytes(stringHash);
            using var crypto = new SHA256CryptoServiceProvider();
            var hash = crypto.ComputeHash(bytes);
            var bytesString = hash.Select(x => x.ToString("x2"));

            Hash = string.Join(string.Empty, bytesString);
        }
    }
}
