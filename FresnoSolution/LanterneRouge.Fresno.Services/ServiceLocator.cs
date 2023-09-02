using Autofac;
using LanterneRouge.Fresno.Services.Data;
using LanterneRouge.Fresno.Services.Email;
using LanterneRouge.Fresno.Services.Interfaces;
using LanterneRouge.Fresno.WpfClient.Services.Interfaces;

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
                    _container = builder.Build();
                }

                return _container;
            }
        }
    }
}
