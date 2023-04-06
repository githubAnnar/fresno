using LanterneRouge.Fresno.Core.Entities;

namespace LanterneRouge.Fresno.Services.Interfaces
{
    public delegate void CommittedHandler();

    public interface IDataService
    {
        IEnumerable<User> GetAllUsers();

        User GetUser(int id);

        User GetUserByStepTest(StepTest stepTest);

        void SaveUser(User entity);

        void RemoveUser(User entity);

        IEnumerable<StepTest> GetAllStepTests();

        IEnumerable<StepTest> GetAllStepTestsByUser(User entity);

        void SaveStepTest(StepTest entity);

        void RemoveStepTest(StepTest entity);

        IEnumerable<Measurement> GetAllMeasurements();

        IEnumerable<Measurement> GetAllMeasurementsByStepTest(StepTest entity);

        void SaveMeasurement(Measurement entity);

        void RemoveMeasurement(Measurement entity);

        bool LoadDatabase(string filename);

        bool CloseDatabase();
    }
}
