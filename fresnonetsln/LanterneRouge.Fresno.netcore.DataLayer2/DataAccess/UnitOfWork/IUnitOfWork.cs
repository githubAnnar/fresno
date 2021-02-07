using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        IList<IUser> GetAllUsers<T>(bool refresh = false) where T : IUser;

        IUser GetUserById<T>(int id, bool refresh = false) where T : IUser;

        void UpsertUser(IUser entity);

        void RemoveUser(IUser entity);

        IList<IStepTest> GetAllStepTests<T>(bool refresh = false) where T : IUser;

        IStepTest GetStepTestById<T>(int id, bool refresh = false) where T : IUser;

        void UpsertStepTest(IStepTest entity);

        void RemoveStepTest(IStepTest entity);

        IList<IMeasurement> GetAllMeasurements<T>(bool refresh = false) where T : IUser;

        IMeasurement GetMeasurementById<T>(int id, bool refresh = false) where T : IUser;

        void UpsertMeasurement(IMeasurement entity);

        void RemoveMeasurement(IMeasurement entity);
    }
}
