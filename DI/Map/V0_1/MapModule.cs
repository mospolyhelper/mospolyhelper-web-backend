namespace Mospolyhelper.DI.Map.V0_1
{
    using Common;
    using Data.Map.Api.V0_1;
    using Data.Map.Remote.V0_1;
    using Data.Map.Repository.V0_1;
    using Domain.Map.Repository;
    using Domain.Map.Repository.V0_1;
    using Domain.Map.UseCase;
    using Domain.Map.UseCase.V0_1;
    using Microsoft.Extensions.DependencyInjection;

    public class MapModule : IModule
    {
        public void Load(IServiceCollection services)
        {
            // Apis
            services.AddSingleton<MapClient>();


            // DataSources
            services.AddSingleton<MapRemoteDataSource>();


            // Repositories
            services.AddSingleton<IMapRepository, MapRepository>();


            // UseCases
            services.AddSingleton<MapUseCase>();
        }
    }
}