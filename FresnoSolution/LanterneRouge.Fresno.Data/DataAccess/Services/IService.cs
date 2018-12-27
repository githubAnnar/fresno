using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.Services
{
    public interface IService
    {
        void Commit();

        IEnumerable<User> GetAllUsers(bool refresh = false);

        void UpdateUser(User entity);

        void RemoveUser(User entity);

        void AddUser(User entity);

        IEnumerable<StepTest> GetAllStepTests();

        void UpdateStepTest(StepTest entity);

        void RemoveStepTest(StepTest entity);

        void AddStepTest(StepTest entity);

        IEnumerable<Measurement> GetAllMeasurements();

        void UpdateMeasurement(Measurement entity);

        void RemoveMeasurement(Measurement entity);

        void AddMeasurement(Measurement entity);
    }
}
