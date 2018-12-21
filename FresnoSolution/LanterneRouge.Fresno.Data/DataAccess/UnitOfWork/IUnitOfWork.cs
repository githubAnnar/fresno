using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.DataLayer.DataAccess.Repositories;
using System;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Measurement, StepTest> MeasurementRepository { get; }

        IRepository<StepTest, User> StepTestRepository { get; }

        IRepository<User, object> UserRepository { get; }

        void Commit();

        IEnumerable<User> GetAllUsers(bool refresh = false);
    }
}
