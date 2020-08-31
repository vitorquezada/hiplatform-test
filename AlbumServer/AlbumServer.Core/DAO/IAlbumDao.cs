using AlbumServer.Core.Model.UserAlbum;
using System.Threading.Tasks;

namespace AlbumServer.Core.DAO
{
    public interface IAlbumDao
    {
        Task<UserAlbum> ObterAlbuns(ulong userId);
    }
}
