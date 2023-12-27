using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Core.Contracts
{
    public interface IMeasurementRepository
    {
        Task<IList<Measurement>> GetAllMeasurementsAsync(CancellationToken cancellationToken = default);

        Task<Measurement?> GetMeasurementByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Measurement?> InsertMeasurementAsync(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default);

        Task<Measurement?> UpdateMeasurementAsync(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default);

        Task DeleteMeasurementAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IList<Measurement>> GetMeasurementsByStepTestIdAsync(Guid stepTestId, CancellationToken cancellationToken = default);

        Task<int> GetCountByStepTestIdAsync(Guid stepTestId, bool onlyInCalculation, CancellationToken cancellationToken = default);

        Task<bool> IsChangedAsync(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default);
    }
}
