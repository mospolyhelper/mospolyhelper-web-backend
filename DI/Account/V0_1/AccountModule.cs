namespace Mospolyhelper.DI.Account.V0_1
{
    using Common;
    using Data.Account.Api.V0_1;
    using Data.Account.Converters.V0_1;
    using Data.Account.Remote.V0_1;
    using Data.Account.Repository.V0_1;
    using Domain.Account.Repository;
    using Domain.Account.Repository.V0_1;
    using Domain.Account.UseCase;
    using Domain.Account.UseCase.V0_1;
    using Microsoft.Extensions.DependencyInjection;

    public class AccountModule : IModule
    {
        public void Load(IServiceCollection services)
        {
            // Apis
            services.AddSingleton<AccountClient>();


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
