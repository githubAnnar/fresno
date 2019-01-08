using LanterneRouge.Fresno.WpfClient.Services.Interfaces;
using Unity;
using Unity.ServiceLocation;

namespace LanterneRouge.Fresno.WpfClient.Services
{
    public static class ServiceLocator
    {
        private static UnityServiceLocator _instance;

        public static UnityServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    var container = new UnityContainer().RegisterInstance<IDataService>(new DataService());
                    _instance = new UnityServiceLocator(container);
                }

                return _instance;
            }
        }
    }
}
