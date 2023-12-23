using Autofac;
using AutoMapper;
using LanterneRouge.Fresno.Services.Data;
using LanterneRouge.Fresno.Services.Email;
using LanterneRouge.Fresno.Services.Interfaces;
using LanterneRouge.Fresno.Services.Profiles;
using LanterneRouge.Fresno.WpfClient.Services.Settings;

namespace LanterneRouge.Fresno.Services
{
    public static class ServiceLocator
    {
        private static IContainer? _container;

        public static IContainer Instance
        {
            get
            {
                if (_container == null)
                {
                    var builder = new ContainerBuilder();
                    builder.RegisterType<DataService>().As<IDataService>().SingleInstance();
                    builder.RegisterType<EmailService>().As<IEmailService>().SingleInstance();
                    builder.RegisterType<ApplicationSettingsService>().As<IApplicationSettingsService>().SingleInstance();

                    builder.Register<IConfigurationProvider>(ctx => new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>()));
                    builder.Register<IMapper>(ctx => new Mapper(ctx.Resolve<IConfigurationProvider>(), ctx.Resolve)).InstancePerDependency();

                    _container = builder.Build();
                }

                return _container;
            }
        }
    }
}
