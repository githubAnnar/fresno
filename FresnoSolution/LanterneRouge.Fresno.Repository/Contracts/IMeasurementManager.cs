using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Repository.Contracts
{
    public interface IMeasurementManager : IManagerBase, IDisposable
    {
        List<IMeasurementEntity> GetAllMeasurements();

        List<IMeasurementEntity> GetMeasurementsByStepTest(IStepTestEntity parent);

        int MeasurementsCountByStepTest(IStepTestEntity parent, bool onlyInCalculation);

        IMeasurementEntity? GetMeasurementById(Guid id);

        void UpsertMeasurement(IMeasurementEntity entity);

        void RemoveMeasurement(IMeasurementEntity entity);

        bool IsChanged(IMeasurementEntity entity);
    }
}
