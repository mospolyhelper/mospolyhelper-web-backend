namespace Mospolyhelper.Domain.Account.Model.V0_2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AuthResponse
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }
    }
}
