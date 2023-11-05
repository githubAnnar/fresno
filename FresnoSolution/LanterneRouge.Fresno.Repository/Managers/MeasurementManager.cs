using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;
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
            Repository = new MeasurementRepository(_connection);
        }

        #endregion

        #region Properties

        private IMeasurementRepository Repository { get; }

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

        public async Task<IList<Measurement>> GetAllMeasurements(CancellationToken cancellationToken = default) => await Repository.GetAllMeasurements(cancellationToken);

        public async Task<IList<Measurement>> GetMeasurementsByStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default) => await Repository.GetMeasurementsByStepTest(stepTestEntity, cancellationToken);

        public async Task<int> MeasurementsCountByStepTest(IStepTestEntity stepTestEntity, bool onlyInCalculation, CancellationToken cancellationToken = default) => await Repository.GetCountByStepTest(stepTestEntity, onlyInCalculation, cancellationToken);

        public async Task<Measurement?> GetMeasurementById(Guid id, CancellationToken cancellationToken = default) => await Repository.GetMeasurementById(id, cancellationToken);

        public async Task<Measurement?> UpsertMeasurement(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default)
        {
            Measurement? response;
            if (measurementEntity.Id == Guid.Empty)
            {
                response = await Repository.InsertMeasurement(measurementEntity, cancellationToken);
            }

            else
            {
                response = await Repository.UpdateMeasurement(measurementEntity, cancellationToken);
            }

            return response;
        }

        public async Task DeleteMeasurement(Guid id, CancellationToken cancellationToken) => await Repository.DeleteMeasurement(id, cancellationToken);

        public async Task<int> GetCountByStepTest(IStepTestEntity stepTestEntity, bool onlyInCalculation, CancellationToken cancellationToken = default) => await Repository.GetCountByStepTest(stepTestEntity, onlyInCalculation, cancellationToken);

        public async Task<bool> IsChanged(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default) => await Repository.IsChanged(measurementEntity, cancellationToken);

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
