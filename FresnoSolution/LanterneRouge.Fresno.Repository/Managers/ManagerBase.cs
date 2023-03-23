using LanterneRouge.Fresno.Repository.Contracts;
using log4net;
using System.Data;

namespace LanterneRouge.Fresno.Repository.Managers
{
    public abstract class ManagerBase : IManagerBase
    {
        #region Fields

        protected static readonly ILog Logger = LogManager.GetLogger(typeof(ManagerBase));
        protected IDbConnection _connection;
        protected IDbTransaction _transaction;
        protected bool _disposed;

        #endregion

        #region Constructors

        public ManagerBase(IConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.GetConnection;
            Logger.Debug($"Connection set: {_connection.ConnectionString}");
            _transaction = _connection.BeginTransaction();
        }

        #endregion

        #region Methods

        public abstract void Close();

        #endregion
    }
}
