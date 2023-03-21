using LanterneRouge.Fresno.Core.Contracts;
using LanterneRouge.Fresno.Core.Entities;
using LanterneRouge.Fresno.Repository.Contracts;
using LanterneRouge.Fresno.Repository.Repositories;
using System.Data;

namespace LanterneRouge.Fresno.Repository.Managers
{
    public class UserManager : ManagerBase, IUserManager
    {
        #region Fields

        private IRepository<User> _userRepository;
        private List<User> _cachedUsers;

        #endregion

        public UserManager(IConnectionFactory connectionFactory) : base(connectionFactory)
        { }

        #region Properties

        private IRepository<User> UserRepository => _userRepository ??= new UserRepository(_transaction);

        private List<User> CachedUsers => _cachedUsers ??= new List<User>();

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

            // Reset Caches
            _cachedUsers = null!;
            Logger.Info("Cached users is reset");

            // Reset repository
            Logger.Info("Repository is reset");
        }

        public override void Commit()
        {
            var updatedUsers = CachedUsers.Where(u => u.State == EntityState.Updated);
            var newUsers = CachedUsers.Where(u => u.State == EntityState.New);

            foreach (var item in newUsers)
            {
                Logger.Info($"Adding new user: {item.LastName}, {item.FirstName}");
                UserRepository.Add(item);
            }

            foreach (var item in updatedUsers)
            {
                Logger.Info($"Update user: {item.LastName}, {item.FirstName}");
                UserRepository.Update(item);
            }

            try
            {
                _transaction.Commit();
                Logger.Debug("Transaction Committed");
            }

            catch (Exception e)
            {
                _transaction.Rollback();
                Logger.Error("Error in commit, transaction is rolled back!", e);
                throw;
            }

            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                ResetRepository();
            }
        }

        public List<User> GetAllUsers(bool refresh = false)
        {
            if (refresh || CachedUsers.Count == 0)
            {
                _cachedUsers = UserRepository.All().ToList();
            }

            return CachedUsers;
        }

        public User GetUserById(int id, bool refresh = false)
        {
            if (refresh || CachedUsers.Count == 0)
            {
                _cachedUsers = UserRepository.All().ToList();
            }

            return _cachedUsers.FirstOrDefault(u => u.Id == id);
        }

        public User GetUserByStepTest(StepTest stepTest, bool refresh = false)
        {
            if (refresh || CachedUsers.Count == 0)
            {
                _cachedUsers = UserRepository.All().ToList();
            }

            return CachedUsers.FirstOrDefault(u => u.Id == stepTest.UserId);
        }

        public void UpsertUser(User entity)
        {
            if (entity.State == EntityState.New)
            {
                UserRepository.Add(entity);
            }

            else
            {
                UserRepository.Update(entity);
            }
        }

        public void RemoveUser(User entity) => UserRepository.Remove(entity);

        #endregion

        #region IDisposable Support

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void ResetRepository()
        {
            _userRepository = null!;
            Logger.Debug("User Repository is resetted");
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null!;
                    }

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
