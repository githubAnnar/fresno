using LanterneRouge.Fresno.Core.Contracts;
using LanterneRouge.Fresno.Core.Entities;
using LanterneRouge.Fresno.Repository.Contracts;
using LanterneRouge.Fresno.Repository.Repositories;
using System.Data;

namespace LanterneRouge.Fresno.Repository.Managers
{
    public class MeasurementManager : ManagerBase, IMeasurementManager
    {
        #region Constructors
        public MeasurementManager(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
            MeasurementRepository = new MeasurementRepository(_connection);
        }

        #endregion

        #region Properties

        private IRepository<Measurement> MeasurementRepository { get; }

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

        public List<Measurement> GetAllMeasurements() => MeasurementRepository.All().ToList();

        public List<Measurement> GetMeasurementsByStepTest(StepTest parent) => MeasurementRepository.FindByParentId(parent).ToList();

        public Measurement GetMeasurementById(int id) => MeasurementRepository.FindSingle(id);

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

        ~MeasurementManager()
        {
            Dispose(false);
        }

        #endregion
    }
}
