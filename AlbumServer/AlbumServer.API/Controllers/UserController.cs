using AlbumServer.Core.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlbumServer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public UserController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet("{id}/albums")]
        public async Task<IActionResult> Get(ulong id)
        {
            var albums = await _albumService.ObterAlbuns(id);
            return Ok(albums);
        }
    }
}
