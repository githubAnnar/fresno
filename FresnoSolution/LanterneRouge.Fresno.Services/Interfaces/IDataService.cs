using LanterneRouge.Fresno.Core.Interface;
using LanterneRouge.Fresno.Services.Models;

namespace LanterneRouge.Fresno.Services.Interfaces
{
    public delegate void CommittedHandler();

    public interface IDataService
    {
        IEnumerable<UserModel> GetAllUsers();

        IUserEntity GetUser(int id);

        UserModel GetUserByStepTest(StepTestModel stepTest);

        void SaveUser(UserModel entity);

        void RemoveUser(UserModel entity);

        IList<StepTestModel> GetAllStepTests();

        IEnumerable<StepTestModel> GetAllStepTestsByUser(UserModel entity);

        int StepTestCountByUser(UserModel entity, bool onlyInCalculation = false);

        void SaveStepTest(StepTestModel entity);

        void RemoveStepTest(StepTestModel entity);

        IEnumerable<MeasurementModel> GetAllMeasurements();

        IEnumerable<MeasurementModel> GetAllMeasurementsByStepTest(StepTestModel entity);

        int MeasurementsCountByStepTest(StepTestModel entity, bool onlyInCalculation = false);

        void SaveMeasurement(MeasurementModel entity);

        void RemoveMeasurement(MeasurementModel entity);

        bool LoadDatabase(string filename);

        bool CloseDatabase();
    }
}
