using System.Collections.Generic;

namespace AlbumServer.Core.Model.UserAlbum
{
    public class UserAlbum
    {
        public ulong UserId { get; set; }

        public IList<Album> Albums { get; set; } = new List<Album>();
    }
}
