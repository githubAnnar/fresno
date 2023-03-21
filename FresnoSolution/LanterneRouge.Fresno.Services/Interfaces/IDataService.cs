using LanterneRouge.Fresno.Core.Entities;

namespace LanterneRouge.Fresno.Services.Interfaces
{
    public delegate void CommittedHandler();

    public interface IDataService
    {
        void Commit();

        event CommittedHandler Committed;

        IEnumerable<User> GetAllUsers(bool refresh = false);

        User GetUserByStepTest(StepTest stepTest,bool refresh=false);

        void UpdateUser(User entity);

        void RemoveUser(User entity);

        void AddUser(User entity);

        IEnumerable<StepTest> GetAllStepTests();

        IEnumerable<StepTest> GetAllStepTestsByUser(User entity);

        void UpdateStepTest(StepTest entity);

        void RemoveStepTest(StepTest entity);

        void AddStepTest(StepTest entity);

        IEnumerable<Measurement> GetAllMeasurements();

        IEnumerable<Measurement> GetAllMeasurementsByStepTest(StepTest entity);

        void UpdateMeasurement(Measurement entity);

        void RemoveMeasurement(Measurement entity);

        void AddMeasurement(Measurement entity);

        bool LoadDatabase(string filename);

        bool CloseDatabase();
    }
}
