using AlbumServer.Core.DAO;
using AlbumServer.Core.Model.UserAlbum;
using System.Threading.Tasks;

namespace AlbumServer.Core.Service.Implementation
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumDao _albumDao;

        public AlbumService(IAlbumDao albumDao)
        {
            _albumDao = albumDao;
        }

        public async Task<UserAlbum> ObterAlbuns(ulong userId)
        {
            if (userId <= 0)
                return null;

            return await _albumDao.ObterAlbuns(userId);
        }
    }
}
