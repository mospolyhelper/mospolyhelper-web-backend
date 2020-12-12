using System.Net.Http;
using Autofac;

namespace Mospolyhelper.DI
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(c => new HttpClient(new HttpClientHandler { UseCookies = false }))
                .As<HttpClient>()
                .SingleInstance();
        }
    }
}
