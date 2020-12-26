namespace Mospolyhelper.DI
{
    using Microsoft.Extensions.DependencyInjection;
    using Mospolyhelper.DI.Common;
    using System.Net.Http;

    public class CoreModule : IModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddSingleton<HttpClient>(it => 
                new HttpClient(new HttpClientHandler { UseCookies = false })
            );
        }
    }
}
