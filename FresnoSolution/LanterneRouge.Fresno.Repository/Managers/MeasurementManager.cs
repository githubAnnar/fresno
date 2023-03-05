using LanterneRouge.Fresno.Core.Contracts;
using LanterneRouge.Fresno.Core.Entities;
using LanterneRouge.Fresno.Repository.Contracts;
using LanterneRouge.Fresno.Repository.Repositories;
using System.Data;

namespace LanterneRouge.Fresno.Repository.Managers
{
    public class MeasurementManager : ManagerBase, IMeasurementManager
    {
        #region Fields

        private IRepository<Measurement> _measurementRepository;
        private List<Measurement> _cachedMeasurements;

        #endregion

        #region Constructors
        public MeasurementManager(IConnectionFactory connectionFactory) : base(connectionFactory)
        { }

        #endregion

        #region Properties

        private IRepository<Measurement> MeasurementRepository => _measurementRepository ??= new MeasurementRepository(_transaction);

        private List<Measurement> CachedMeasurements => _cachedMeasurements ??= new List<Measurement>();

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
            _cachedMeasurements = null!;
            Logger.Info("Cached measurements are reset");

            // Reset repository
            Logger.Info("Repository is reset");
        }

        public override void Commit()
        {
            var updatedMeasurements = CachedMeasurements.Where(u => u.State == EntityState.Updated);
            var newMeasurements = CachedMeasurements.Where(u => u.State == EntityState.New);

            foreach (var item in newMeasurements)
            {
                Logger.Info($"Adding new measurement: {item.StepTestId}");
                MeasurementRepository.Add(item);
            }

            foreach (var item in updatedMeasurements)
            {
                Logger.Info($"Update measurement: {item.Id}, {item.StepTestId}");
                MeasurementRepository.Update(item);
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
                if (_connection != null)
                {
                    _transaction = _connection.BeginTransaction();
                }

                ResetRepository();
            }
        }

        public List<Measurement> GetAllMeasurements(bool refresh = false)
        {
            if (refresh || CachedMeasurements.Count == 0)
            {
                _cachedMeasurements = MeasurementRepository.All().ToList();
            }

            return CachedMeasurements;
        }

        public Measurement GetMeasurementById(int id, bool refresh = false)
        {
            if (refresh || CachedMeasurements.Count == 0)
            {
                _cachedMeasurements = MeasurementRepository.All().ToList();
            }

            return CachedMeasurements.FirstOrDefault(m => m.Id == id);
        }

        public void UpsertMeasurement(Measurement entity)
        {
            if (entity.State == EntityState.New)
            {
                MeasurementRepository.Add(entity);
            }

            else
            {
                MeasurementRepository.Update(entity);
            }
        }

        public void RemoveMeasurement(Measurement entity) => MeasurementRepository.Remove(entity);

        #endregion

        #region IDisposable Support

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void ResetRepository()
        {
            _measurementRepository = null!;
            Logger.Debug("Measurement Repository is resetted");
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

        ~MeasurementManager()
        {
            Dispose(false);
        }

        #endregion
    }
}
