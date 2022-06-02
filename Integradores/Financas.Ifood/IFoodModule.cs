using Autofac;
using Financas.Core;
using System.Reflection;

namespace Financas.Ifood
{
    public class IFoodModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var dataAccess = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(dataAccess)
                   .AssignableTo<ITransientDependency>()
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }
    }
}
