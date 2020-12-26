namespace Mospolyhelper.DI.Common
{
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class DiExt
    {
        public static IServiceCollection RegisterModule(this IServiceCollection services, IModule module)
        {
            module.Load(services);
            return services;
        }
    }
}
