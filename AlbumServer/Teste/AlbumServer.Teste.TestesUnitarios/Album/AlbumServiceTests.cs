using AlbumServer.Core.DAO;
using AlbumServer.Core.Model.UserAlbum;
using AlbumServer.Core.Service;
using AlbumServer.Core.Service.Implementation;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace AlbumServer.Teste.TestesUnitarios.Album
{
    public class AlbumServiceTests
    {
        private readonly IAlbumDao _albumDao;

        public AlbumServiceTests()
        {
            var mock = new Mock<IAlbumDao>();
            mock.Setup(x => x.ObterAlbuns(It.IsAny<ulong>())).ReturnsAsync<ulong, IAlbumDao, UserAlbum>(x => new UserAlbum { UserId = x });

            _albumDao = mock.Object;
        }

        [Fact]
        public async Task ObterAlbuns_Id_Invalido_Test()
        {
            IAlbumService albumService = new AlbumService(_albumDao);

            var result = await albumService.ObterAlbuns(0);

            Assert.Null(result);
        }

        [Fact]
        public async Task ObterAlbuns_Id_Valido_Test()
        {
            IAlbumService albumService = new AlbumService(_albumDao);

            var result = await albumService.ObterAlbuns(1);

            Assert.NotNull(result);
            Assert.Equal(1UL, result.UserId);
        }
    }
}
