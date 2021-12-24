using Autofac;
using Blog.App.Data.Common;
using Blog.App.Data.Models;

namespace Blog.App.WebApp.Dependency
{
    public class EFModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(BlogDataBaseContext)).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
        }
    }
}