namespace Mospolyhelper.DI.Core.V0_1
{
    using System.Net.Http;
    using Common;
    using Microsoft.Extensions.DependencyInjection;

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
