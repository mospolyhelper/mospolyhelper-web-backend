using Mospolyhelper.Utils;
using System.Threading.Tasks;

namespace Mospolyhelper.Domain.Map.Repository
{
    public interface IMapRepository
    {
        public Task<Result<string>> GetMap();
    }
}
