namespace Mospolyhelper.Domain.Account.Repository.V0_2
{
    using Model.V0_2;
    using Utils;
    using System.Threading.Tasks;

    public interface IAccountRepository
    {
        public Task<Result<UserAuth>> Auth(string login, string password, string? sessionId = null);
    }
}
