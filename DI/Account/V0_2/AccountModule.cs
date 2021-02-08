namespace Mospolyhelper.DI.V0_2
{
    using Microsoft.Extensions.DependencyInjection;
    using Mospolyhelper.Data.Account.Api;
    using Mospolyhelper.Data.Account.Converters.V0_2;
    using Mospolyhelper.Data.Account.Remote.V0_2;
    using Mospolyhelper.Data.Account.Repository.V0_2;
    using Mospolyhelper.DI.Common;
    using Mospolyhelper.Domain.Account.Repository.V0_2;
    using Mospolyhelper.Domain.Account.UseCase.V0_2;

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
