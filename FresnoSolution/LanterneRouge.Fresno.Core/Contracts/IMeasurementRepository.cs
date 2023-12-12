using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Core.Contracts
{
    public interface IMeasurementRepository
    {
        Task<IList<Measurement>> GetAllMeasurements(CancellationToken cancellationToken = default);

        Task<Measurement?> GetMeasurementById(Guid id, CancellationToken cancellationToken = default);

        Task<Measurement?> InsertMeasurement(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default);

        Task<Measurement?> UpdateMeasurement(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default);

        Task DeleteMeasurement(Guid id, CancellationToken cancellationToken = default);

        Task<IList<Measurement>> GetMeasurementsByStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task<int> GetCountByStepTest(IStepTestEntity stepTestEntity, bool onlyInCalculation, CancellationToken cancellationToken = default);

        Task<bool> IsChanged(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default);
    }
}
