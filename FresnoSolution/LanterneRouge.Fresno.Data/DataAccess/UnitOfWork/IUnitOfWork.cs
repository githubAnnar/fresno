using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.DataLayer.DataAccess.Repositories;
using System;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Measurement> MeasurementRepository { get; }

        IRepository<StepTest> StepTestRepository { get; }

        IRepository<User> UserRepository { get; }

        void Commit();
    }
}
