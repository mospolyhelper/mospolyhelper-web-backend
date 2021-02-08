namespace Mospolyhelper.DI.Schedule.V0_1
{
    using Common;
    using Data.Schedule.Api.V0_1;
    using Data.Schedule.Converters.V0_1;
    using Data.Schedule.Local.V0_1;
    using Data.Schedule.Remote.V0_1;
    using Data.Schedule.Repository.V0_1;
    using Domain.Schedule.Repository;
    using Domain.Schedule.Repository.V0_1;
    using Domain.Schedule.UseCase;
    using Domain.Schedule.UseCase.V0_1;
    using Microsoft.Extensions.DependencyInjection;

    class ScheduleModule : IModule
    {
        public void Load(IServiceCollection services)
        {
            // Apis
            services.AddSingleton<ScheduleClient>();


            // Converters
            services.AddSingleton<ScheduleRemoteConverter>();
            services.AddSingleton<ScheduleTeacherRemoteConverter>();


            // DataSources
            services.AddSingleton<ScheduleRemoteDataSource>();
            services.AddSingleton<ScheduleTeacherRemoteDataSource>();
            services.AddSingleton<ScheduleLocalDataSource>();


            // Repositories
            services.AddSingleton<IScheduleRepository, ScheduleRepository>();


            // UseCases
            services.AddSingleton<ScheduleUseCase>();
        }
    }
}
