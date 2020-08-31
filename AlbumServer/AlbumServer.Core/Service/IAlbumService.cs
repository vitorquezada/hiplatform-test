using AlbumServer.Core.Model.UserAlbum;
using System.Threading.Tasks;

namespace AlbumServer.Core.Service
{
    public interface IAlbumService
    {
        Task<UserAlbum> ObterAlbuns(ulong userId);
    }
}
