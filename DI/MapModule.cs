using System.Net.Http;
using Autofac;
using Mospolyhelper.Data.Map.Api;
using Mospolyhelper.Data.Map.Remote;
using Mospolyhelper.Data.Map.Repository;
using Mospolyhelper.Domain.Map.Repository;
using Mospolyhelper.Domain.Map.UseCase;

namespace Mospolyhelper.DI
{
    public class MapModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Apis
            builder
                .Register(c => new MapClient(c.Resolve<HttpClient>()))
                .As<MapClient>()
                .SingleInstance();


            // DataSources
            builder
                .Register(c => 
                    new MapRemoteDataSource(c.Resolve<MapClient>())
                )
                .As<MapRemoteDataSource>()
                .SingleInstance();


            // Repositories
            builder
                .Register(c => new MapRepository(c.Resolve<MapRemoteDataSource>()))
                .As<IMapRepository>()
                .SingleInstance();


            // UseCases
            builder
                .Register(c => new MapUseCase(c.Resolve<IMapRepository>()))
                .As<MapUseCase>()
                .SingleInstance();
        }
    }
}