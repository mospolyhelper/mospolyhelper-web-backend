﻿namespace Mospolyhelper.DI.V0_2
{
    using Common;
    using Microsoft.Extensions.DependencyInjection;
    using Data.Account.Converters.V0_2;
    using Data.Account.Remote.V0_2;
    using Data.Account.Repository.V0_2;
    using Domain.Account.Repository.V0_2;
    using Domain.Account.UseCase.V0_2;

    public class AccountModule : IModule
    {
        public void Load(IServiceCollection services)
        {
            // Apis
            //services.AddSingleton<AccountClient>();


            // Converters
            services.AddSingleton<AccountConverter>();


            // DataSources
            services.AddSingleton<AccountRemoteDataSource>();


            // Repositories
            services.AddSingleton<IAccountRepository, AccountRepository>();


            // UseCases
            services.AddSingleton<AccountUseCase>();
        }
    }
}
