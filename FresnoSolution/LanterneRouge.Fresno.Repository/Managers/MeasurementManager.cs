using LanterneRouge.Fresno.Core.Contracts;
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
            MeasurementRepository = new MeasurementRepository(_connection);
        }

        #endregion

        #region Properties

        private IRepository<IMeasurementEntity, IStepTestEntity> MeasurementRepository { get; }

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

        public List<IMeasurementEntity> GetAllMeasurements() => MeasurementRepository.All().ToList();

        public List<IMeasurementEntity> GetMeasurementsByStepTest(IStepTestEntity parent) => MeasurementRepository.FindByParentId(parent).ToList();

        public int MeasurementsCountByStepTest(IStepTestEntity parent, bool onlyInCalculation) => MeasurementRepository.GetCountByParentId(parent, onlyInCalculation);

        public IMeasurementEntity? GetMeasurementById(Guid id) => MeasurementRepository.FindSingle(id);

        public void UpsertMeasurement(IMeasurementEntity entity)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
                MeasurementRepository.Add(entity);
            }

            else
            {
                MeasurementRepository.Update(entity);
            }
        }

        public void RemoveMeasurement(IMeasurementEntity entity) => MeasurementRepository.Remove(entity);

        public bool IsChanged(IMeasurementEntity entity) => MeasurementRepository.IsChanged(entity);

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
