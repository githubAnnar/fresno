using Autofac;
using LanterneRouge.Fresno.WpfClient.Services.Interfaces;

namespace LanterneRouge.Fresno.WpfClient.Services
{
    public static class ServiceLocator
    {
        private static IContainer _container;

        public static IContainer Instance
        {
            get
            {
                if (_container == null)
                {
                    var builder = new ContainerBuilder();
                    builder.RegisterType<DataService>().As<IDataService>().SingleInstance();
                    _container = builder.Build();
                }

                return _container;
            }
        }
    }
}
