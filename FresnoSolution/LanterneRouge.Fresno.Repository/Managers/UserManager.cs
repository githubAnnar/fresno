using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;
using LanterneRouge.Fresno.Repository.Contracts;
using LanterneRouge.Fresno.Repository.Repositories;
using System.Data;

namespace LanterneRouge.Fresno.Repository.Managers
{
    public class UserManager : ManagerBase, IUserManager
    {
        public UserManager(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
            Repository = new UserRepository(_connection);
        }

        #region Properties

        private IUserRepository Repository { get; }

        #endregion

        #region Methods

        public override void Close()
        {
            // Close db connection
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }

            _connection.Dispose();
            _connection = null!;
            Logger.Info("Db connection closed");
        }

        public async Task<IList<User>> GetAllUsers(CancellationToken cancellationToken = default) => await Repository.GetAllUsers(cancellationToken);

        public async Task<User?> GetUserById(Guid id, CancellationToken cancellationToken = default) => await Repository.GetUserById(id, cancellationToken);

        public async Task<User?> UpsertUser(IUserEntity userEntity, CancellationToken cancellationToken = default)
        {
            User? response;
            if (userEntity.Id == Guid.Empty)
            {
                response = await Repository.InsertUser(userEntity, cancellationToken);
            }

            else
            {
                response = await Repository.UpdateUser(userEntity, cancellationToken);
            }

            return response;
        }

        public async Task DeleteUser(Guid id, CancellationToken cancellationToken = default) => await Repository.DeleteUser(id, cancellationToken);

        public async Task<bool> IsChanged(IUserEntity userEntity, CancellationToken cancellationToken = default) => await Repository.IsChanged(userEntity, cancellationToken);

        public async Task<User> GetUserByStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default) => await Repository.GetUserByStepTest(stepTestEntity, cancellationToken);

        #endregion

        #region IDisposable Support

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null!;
                    }
                }

                _disposed = true;
            }
        }

        ~UserManager()
        {
            Dispose(false);
        }

        #endregion
    }
}
