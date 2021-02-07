using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        IList<IUser> GetAllUsers<TUser, TStepTest, TMeasurement>(bool refresh = false) where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement;

        IUser GetUserById<TUser, TStepTest, TMeasurement>(int id, bool refresh = false) where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement;

        void UpsertUser(IUser entity);

        void RemoveUser(IUser entity);

        IList<IStepTest> GetAllStepTests<TUser, TStepTest, TMeasurement>(bool refresh = false) where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement;

        IStepTest GetStepTestById<TUser, TStepTest, TMeasurement>(int id, bool refresh = false) where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement;

        void UpsertStepTest(IStepTest entity);

        void RemoveStepTest(IStepTest entity);

        IList<IMeasurement> GetAllMeasurements<TUser, TStepTest, TMeasurement>(bool refresh = false) where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement;

        IMeasurement GetMeasurementById<TUser, TStepTest, TMeasurement>(int id, bool refresh = false) where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement;

        void UpsertMeasurement(IMeasurement entity);

        void RemoveMeasurement(IMeasurement entity);
    }
}
