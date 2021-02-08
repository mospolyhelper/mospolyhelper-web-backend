namespace Mospolyhelper.Domain.Map.Repository.V0_1
{
    using System.Threading.Tasks;
    using Utils;

    public interface IMapRepository
    {
        public Task<Result<string>> GetMap();
    }
}
