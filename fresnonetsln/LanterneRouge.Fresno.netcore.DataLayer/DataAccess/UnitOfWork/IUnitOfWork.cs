using LanterneRouge.Fresno.netcore.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.netcore.DataLayer.DataAccess.Repositories;
using System;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.netcore.DataLayer.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

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
