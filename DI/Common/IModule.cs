namespace Mospolyhelper.DI.Common
{
    using Microsoft.Extensions.DependencyInjection;

    public interface IModule
    {
        public void Load(IServiceCollection services);
    }
}
