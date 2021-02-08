using LanterneRouge.Fresno.netcore.AvaloniaClient.Models;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.netcore.AvaloniaClient.Services.Interfaces
{
    public delegate void CommittedHandler();

    public interface IDataService
    {
        void Commit();

        event CommittedHandler Committed;

        IEnumerable<User> GetAllUsers(bool refresh = false);

        void UpdateUser(User entity);

        void RemoveUser(User entity);

        void AddUser(User entity);

        IEnumerable<StepTest> GetAllStepTests();

        IEnumerable<StepTest> GetAllStepTestsByUser(User entity);

        void UpdateStepTest(StepTest entity);

        void RemoveStepTest(StepTest entity);

        void AddStepTest(StepTest entity);

        IEnumerable<Measurement> GetAllMeasurements();

        void UpdateMeasurement(Measurement entity);

        void RemoveMeasurement(Measurement entity);

        void AddMeasurement(Measurement entity);
    }
}
