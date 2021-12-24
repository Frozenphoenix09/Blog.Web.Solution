using Autofac;
using System.Linq;
using System.Reflection;

namespace Blog.App.WebApp.Dependency
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("Blog.App.Data"))
                     .Where(t => t.Name.EndsWith("Repository"))
                     .AsImplementedInterfaces()
                     .InstancePerLifetimeScope();
        }
    }
}