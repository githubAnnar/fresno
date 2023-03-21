using LanterneRouge.Fresno.Core.Entities;

namespace LanterneRouge.Fresno.Repository.Contracts
{
    public interface IMeasurementManager : IManagerBase, IDisposable
    {
        List<Measurement> GetAllMeasurements(bool refresh = false);

        List<Measurement> GetMeasurementsByStepTest(StepTest parent, bool refresh = false);

        Measurement GetMeasurementById(int id, bool refresh = false);

        void UpsertMeasurement(Measurement entity);

        void RemoveMeasurement(Measurement entity);
    }
}
