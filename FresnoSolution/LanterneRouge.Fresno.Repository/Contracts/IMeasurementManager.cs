using LanterneRouge.Fresno.Core.Entities;

namespace LanterneRouge.Fresno.Repository.Contracts
{
    public interface IMeasurementManager : IDisposable
    {
        List<Measurement> GetAllMeasurements(bool refresh = false);

        Measurement GetMeasurementById(int id, bool refresh = false);

        void UpsertMeasurement(Measurement entity);

        void RemoveMeasurement(Measurement entity);
    }
}
