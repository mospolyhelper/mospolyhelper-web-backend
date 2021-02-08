namespace Mospolyhelper.Domain.Account.Repository.V0_2
{
    using Mospolyhelper.Domain.Account.Model.V0_2;
    using Mospolyhelper.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IAccountRepository
    {
        public Task<Result<UserAuth>> Auth(string login, string password, string? sessionId = null);
    }
}
