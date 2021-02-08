using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mospolyhelper.Utils
{
    public static partial class Secrets
    {
        public static readonly string UrlScheduleSessionAll = 
            Environment.GetEnvironmentVariable("URL_SCHEDULE_SESSION_ALL") ?? string.Empty;
        public static readonly string UrlScheduleAll =
            Environment.GetEnvironmentVariable("URL_SCHEDULE_ALL") ?? string.Empty;
        public static readonly string AuthAesIv =
            Environment.GetEnvironmentVariable("AUTH_AES_IV") ?? string.Empty;
        public static readonly string AuthAesKey =
            Environment.GetEnvironmentVariable("AUTH_AES_KEY") ?? string.Empty;
        public static readonly string AuthJwtKey =
            Environment.GetEnvironmentVariable("AUTH_JWT_KEY") ?? string.Empty;
    }
}
