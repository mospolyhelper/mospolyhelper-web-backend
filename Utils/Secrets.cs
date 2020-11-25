using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mospolyhelper.Utils
{
    public static partial class Secrets
    {
        public static readonly string URL_SCHEDULE_SESSION_ALL = 
            Environment.GetEnvironmentVariable(URL_SCHEDULE_SESSION_ALL) ?? string.Empty;
        public static readonly string URL_SCHEDULE_ALL =
            Environment.GetEnvironmentVariable(URL_SCHEDULE_ALL) ?? string.Empty;
    }
}
