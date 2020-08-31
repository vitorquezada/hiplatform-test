using System.Collections.Generic;

namespace AlbumServer.Core.Model.UserAlbum
{
    public class Album
    {
        public ulong AlbumId { get; set; }
        public string AlbumTitle { get; set; }
        public IList<AlbumFoto> Photos { get; set; } = new List<AlbumFoto>();
    }
}
