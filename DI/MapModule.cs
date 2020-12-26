namespace Mospolyhelper.DI
{
    using Microsoft.Extensions.DependencyInjection;
    using Mospolyhelper.Data.Map.Api;
    using Mospolyhelper.Data.Map.Remote;
    using Mospolyhelper.Data.Map.Repository;
    using Mospolyhelper.DI.Common;
    using Mospolyhelper.Domain.Map.Repository;
    using Mospolyhelper.Domain.Map.UseCase;

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