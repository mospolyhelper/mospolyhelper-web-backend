using System.Net.Http;
using Autofac;

namespace Mospolyhelper.DI
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(c => new HttpClient())
                .As<HttpClient>()
                .SingleInstance();
        }
    }
}
