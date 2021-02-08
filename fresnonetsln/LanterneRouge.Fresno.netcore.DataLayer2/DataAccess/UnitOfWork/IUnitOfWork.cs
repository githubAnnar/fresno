using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        IList<TUser> GetAllUsers<TUser, TStepTest, TMeasurement>(bool refresh = false) where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement;

        TUser GetUserById<TUser, TStepTest, TMeasurement>(int id, bool refresh = false) where TUser : class, IUser where TStepTest : IStepTest where TMeasurement : IMeasurement;

        void UpsertUser(IUser entity);

        void RemoveUser(IUser entity);

        IList<TStepTest> GetAllStepTests<TUser, TStepTest, TMeasurement>(bool refresh = false) where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement;

        TStepTest GetStepTestById<TUser, TStepTest, TMeasurement>(int id, bool refresh = false) where TUser : IUser where TStepTest : class, IStepTest where TMeasurement : IMeasurement;

        void UpsertStepTest(IStepTest entity);

        void RemoveStepTest(IStepTest entity);

        IList<TMeasurement> GetAllMeasurements<TUser, TStepTest, TMeasurement>(bool refresh = false) where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement;

        TMeasurement GetMeasurementById<TUser, TStepTest, TMeasurement>(int id, bool refresh = false) where TUser : IUser where TStepTest : IStepTest where TMeasurement : class, IMeasurement;

        void UpsertMeasurement(IMeasurement entity);

        void RemoveMeasurement(IMeasurement entity);
    }
}
