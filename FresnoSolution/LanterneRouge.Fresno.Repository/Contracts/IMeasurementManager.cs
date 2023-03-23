using LanterneRouge.Fresno.Core.Entities;

namespace LanterneRouge.Fresno.Repository.Contracts
{
    public interface IMeasurementManager : IManagerBase, IDisposable
    {
        List<Measurement> GetAllMeasurements();

        List<Measurement> GetMeasurementsByStepTest(StepTest parent);

        Measurement GetMeasurementById(int id);

        void UpsertMeasurement(Measurement entity);

        void RemoveMeasurement(Measurement entity);
    }
}
