using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac;
using Mospolyhelper.Data.Schedule.Api;
using Mospolyhelper.Data.Schedule.Converters;
using Mospolyhelper.Data.Schedule.Remote;
using Mospolyhelper.Data.Schedule.Repository;
using Mospolyhelper.Features.Controllers.Schedule;

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
