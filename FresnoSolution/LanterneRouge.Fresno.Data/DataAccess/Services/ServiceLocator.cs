using LanterneRouge.Fresno.DataLayer.DataAccess.Infrastructure;
using Unity;
using Unity.ServiceLocation;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.Services
{
    public static class ServiceLocator
    {
        private static UnityServiceLocator _instance;
        private static UnitOfWork.UnitOfWork _dbUnitOfWork;
        private static ConnectionFactory _connectionFactory;

        private static ConnectionFactory LocalConnectionFactory => _connectionFactory ?? (_connectionFactory = new ConnectionFactory("Fresno.sqlite"));
        private static UnitOfWork.UnitOfWork UOW => _dbUnitOfWork ?? (_dbUnitOfWork = new UnitOfWork.UnitOfWork(LocalConnectionFactory));

        public static UnityServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    var container = new UnityContainer().RegisterInstance<IService>(new Service(UOW));
                    _instance = new UnityServiceLocator(container);
                }

                return _instance;
            }
        }
    }
}
