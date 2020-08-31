using Indexador.Core.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Indexador.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _albumService.IndexarAlbuns();
            return Ok();
        }
    }
}
