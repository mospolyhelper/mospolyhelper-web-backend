namespace Mospolyhelper.DI
{
    using Microsoft.Extensions.DependencyInjection;
    using Mospolyhelper.Data.Account.Api;
    using Mospolyhelper.Data.Account.Converters;
    using Mospolyhelper.Data.Account.Remote;
    using Mospolyhelper.Data.Account.Repository;
    using Mospolyhelper.DI.Common;
    using Mospolyhelper.Domain.Account.Repository;
    using Mospolyhelper.Domain.Account.UseCase;

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
