using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        void Close();

        List<User> GetAllUsers(bool refresh = false);

        User GetUserById(int id, bool refresh = false);

        void UpsertUser(User entity);

        void RemoveUser(User entity);

        List<StepTest> GetAllStepTests(bool refresh = false);

        StepTest GetStepTestById(int id, bool refresh = false);

        void UpsertStepTest(StepTest entity);

        void RemoveStepTest(StepTest entity);

        List<Measurement> GetAllMeasurements(bool refresh = false);

        Measurement GetMeasurementById(int id, bool refresh = false);

        void UpsertMeasurement(Measurement entity);

        void RemoveMeasurement(Measurement entity);
    }
}
