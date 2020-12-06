using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Autofac;
using Mospolyhelper.Data.Schedule.Api;
using Mospolyhelper.Data.Schedule.Converters;
using Mospolyhelper.Data.Schedule.Local;
using Mospolyhelper.Data.Schedule.Remote;
using Mospolyhelper.Data.Schedule.Repository;
using Mospolyhelper.Domain.Schedule.Repository;
using Mospolyhelper.Domain.Schedule.UseCase;
using Mospolyhelper.Features.Controllers.Schedule;

namespace Mospolyhelper.DI
{
    class ScheduleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Apis
            builder
                .Register(c => new ScheduleClient(c.Resolve<HttpClient>()))
                .As<ScheduleClient>()
                .SingleInstance();


            // Converters
            builder
                .Register(c => new ScheduleRemoteConverter())
                .As<ScheduleRemoteConverter>()
                .SingleInstance();


            // DataSources
            builder
                .Register(c => 
                    new ScheduleRemoteDataSource(
                        c.Resolve<ScheduleClient>(), 
                        c.Resolve<ScheduleRemoteConverter>()
                        )
                )
                .As<ScheduleRemoteDataSource>()
                .SingleInstance();

            builder
                .Register(c => new ScheduleLocalDataSource())
                .As<ScheduleLocalDataSource>()
                .SingleInstance();


            // Repositories
            builder
                .Register(c => new ScheduleRepository(
                    c.Resolve<ScheduleRemoteDataSource>(),
                    c.Resolve<ScheduleLocalDataSource>()
                    ))
                .As<IScheduleRepository>()
                .SingleInstance();


            // UseCases
            builder
                .Register(c => new ScheduleUseCase(c.Resolve<IScheduleRepository>()))
                .As<ScheduleUseCase>()
                .SingleInstance();
        }
    }
}
