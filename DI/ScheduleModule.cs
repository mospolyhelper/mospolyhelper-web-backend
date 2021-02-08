﻿namespace Mospolyhelper.DI
{
    using Microsoft.Extensions.DependencyInjection;
    using Mospolyhelper.Data.Schedule.Api;
    using Mospolyhelper.Data.Schedule.Converters;
    using Mospolyhelper.Data.Schedule.Local;
    using Mospolyhelper.Data.Schedule.Remote;
    using Mospolyhelper.Data.Schedule.Repository;
    using Mospolyhelper.DI.Common;
    using Mospolyhelper.Domain.Schedule.Repository;
    using Mospolyhelper.Domain.Schedule.UseCase;

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
