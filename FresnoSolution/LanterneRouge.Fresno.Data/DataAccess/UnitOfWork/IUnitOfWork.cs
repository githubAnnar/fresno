using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.DataLayer.DataAccess.Repositories;
using System;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Measurement, StepTest> MeasurementRepository { get; }

        IRepository<StepTest, User> StepTestRepository { get; }

        IRepository<User, object> UserRepository { get; }

        void Commit();
    }
}
