using Indexador.Core.Entity.UserAlbum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Indexador.Core.Repository
{
    public interface IAlbumRepository
    {
        Task AdicionarOuAlterar(List<UserAlbum> user);

        Task Remover(List<ulong> ids);

        Task<Dictionary<ulong, string>> ObterHashs();
    }
}
