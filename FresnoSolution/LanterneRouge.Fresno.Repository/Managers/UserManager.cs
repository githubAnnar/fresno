using LanterneRouge.Fresno.Core.Contracts;
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
            UserRepository = new UserRepository(_connection);
        }

        #region Properties

        private IRepository<IUserEntity, IUserEntity> UserRepository { get; }

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

        public List<IUserEntity> GetAllUsers() => UserRepository.All().ToList();

        public IUserEntity GetUserById(Guid id) => UserRepository.FindSingle(id);

        public IUserEntity GetUserByStepTest(IStepTestEntity stepTest) => UserRepository.FindSingle(stepTest.UserId);

        public void UpsertUser(IUserEntity entity)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
                UserRepository.Add(entity);
            }

            else
            {
                UserRepository.Update(entity);
            }
        }

        public void RemoveUser(IUserEntity entity) => UserRepository.Remove(entity);

        public bool IsChanged(IUserEntity entity) => UserRepository.IsChanged(entity);

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
