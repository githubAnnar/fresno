using LanterneRouge.Fresno.Core.Contracts;
using LanterneRouge.Fresno.Core.Entities;
using LanterneRouge.Fresno.Repository.Contracts;
using LanterneRouge.Fresno.Repository.Repositories;
using System.Data;

namespace LanterneRouge.Fresno.Repository.Managers
{
    public class StepTestManager : ManagerBase, IStepTestManager
    {
        #region Fields

        private IRepository<StepTest> _stepTestRepository;
        private List<StepTest> _cachedStepTests;

        #endregion

        #region Constructors
        public StepTestManager(IConnectionFactory connectionFactory) : base(connectionFactory)
        { }

        #endregion

        #region Properties

        private IRepository<StepTest> StepTestRepository => _stepTestRepository ??= new StepTestRepository(_transaction);

        private List<StepTest> CachedStepTests => _cachedStepTests ??= new List<StepTest>();

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
            _cachedStepTests = null!;
            Logger.Info("Cached step tests are reset");

            // Reset repository
            Logger.Info("Repository is reset");
        }

        public override void Commit()
        {
            var updatedStepTests = CachedStepTests.Where(u => u.State == EntityState.Updated);
            var newStepTests = CachedStepTests.Where(u => u.State == EntityState.New);

            foreach (var item in newStepTests)
            {
                Logger.Info($"Adding new step test: {item.UserId}");
                StepTestRepository.Add(item);
            }

            foreach (var item in updatedStepTests)
            {
                Logger.Info($"Update step test: {item.Id}, {item.UserId}");
                StepTestRepository.Update(item);
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

        public List<StepTest> GetAllStepTests(bool refresh = false)
        {
            if (refresh || CachedStepTests.Count == 0)
            {
                _cachedStepTests = StepTestRepository.All().ToList();
            }

            return CachedStepTests;
        }

        public StepTest GetStepTestById(int id, bool refresh = false)
        {
            if (refresh || CachedStepTests.Count == 0)
            {
                _cachedStepTests = StepTestRepository.All().ToList();
            }

            return CachedStepTests.FirstOrDefault(s => s.Id == id);
        }

        public void UpsertStepTest(StepTest entity)
        {
            if (entity.State == EntityState.New)
            {
                StepTestRepository.Add(entity);
            }

            else
            {
                StepTestRepository.Update(entity);
            }
        }

        public void RemoveStepTest(StepTest entity) => StepTestRepository.Remove(entity);

        #endregion

        #region IDisposable Support

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void ResetRepository()
        {
            _stepTestRepository = null!;
            Logger.Debug("StepTest Repository is resetted");
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

        ~StepTestManager()
        {
            Dispose(false);
        }

        #endregion
    }
}
