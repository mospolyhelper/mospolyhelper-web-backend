using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac;
using Mospolyhelper.Data.Account.Api;
using Mospolyhelper.Data.Account.Converters;
using Mospolyhelper.Data.Account.Remote;
using Mospolyhelper.Data.Account.Repository;
using Mospolyhelper.Data.Schedule.Api;
using Mospolyhelper.Data.Schedule.Converters;
using Mospolyhelper.Data.Schedule.Remote;
using Mospolyhelper.Data.Schedule.Repository;
using Mospolyhelper.Domain.Account.Repository;
using Mospolyhelper.Domain.Account.UseCase;
using Mospolyhelper.Domain.Schedule.Repository;
using Mospolyhelper.Domain.Schedule.UseCase;

namespace Mospolyhelper.DI
{
    public class AccountModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Apis
            builder
                .Register(c => new AccountClient(c.Resolve<HttpClient>()))
                .As<AccountClient>()
                .SingleInstance();


            // Converters
            builder
                .Register(c => new AccountConverter())
                .As<AccountConverter>()
                .SingleInstance();


            // DataSources
            builder
                .Register(c =>
                    new AccountRemoteDataSource(
                        c.Resolve<AccountClient>(),
                        c.Resolve<AccountConverter>()
                    )
                )
                .As<AccountRemoteDataSource>()
                .SingleInstance();


            // Repositories
            builder
                .Register(c => new AccountRepository(c.Resolve<AccountRemoteDataSource>()))
                .As<IAccountRepository>()
                .SingleInstance();


            // UseCases
            builder
                .Register(c => new AccountUseCase(c.Resolve<IAccountRepository>()))
                .As<AccountUseCase>()
                .SingleInstance();
        }
    }
}
