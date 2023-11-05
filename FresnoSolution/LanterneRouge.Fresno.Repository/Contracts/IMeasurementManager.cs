using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Repository.Contracts
{
    public interface IMeasurementManager : IManagerBase, IDisposable
    {
        Task<IList<Measurement>> GetAllMeasurements(CancellationToken cancellationToken = default);

        Task<IList<Measurement>> GetMeasurementsByStepTest(IStepTestEntity parent, CancellationToken cancellationToken = default);

        Task<int> GetCountByStepTest(IStepTestEntity parent, bool onlyInCalculation, CancellationToken cancellationToken = default);

        Task<Measurement?> GetMeasurementById(Guid id, CancellationToken cancellationToken = default);

        Task<Measurement?> UpsertMeasurement(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default);

        Task DeleteMeasurement(Guid id, CancellationToken cancellationToken = default);

        Task<bool> IsChanged(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default);
    }
}
