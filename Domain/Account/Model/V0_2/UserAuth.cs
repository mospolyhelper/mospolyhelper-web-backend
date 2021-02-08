namespace Mospolyhelper.Domain.Account.Model.V0_2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserAuth
    {
        public UserAuth(string sessionId, string name, string avatarUrl, IList<string> permissions)
        {
            SessionId = sessionId;
            Name = name;
            AvatarUrl = avatarUrl;
            Permissions = permissions;
        }

        public string SessionId { get; }
        public string Name { get; }
        public string AvatarUrl { get; }
        public IList<string> Permissions { get; }
    }
}
