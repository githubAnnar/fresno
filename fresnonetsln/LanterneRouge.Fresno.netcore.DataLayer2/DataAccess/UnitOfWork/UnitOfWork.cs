using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities;
using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Infrastructure;
using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Repositories;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof(UnitOfWork));
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private IRepository<IMeasurement, IUser, IStepTest, IMeasurement> _measurementRepository;
        private IRepository<IStepTest, IUser, IStepTest, IMeasurement> _stepTestRepository;
        private IRepository<IUser, IUser, IStepTest, IMeasurement> _userRepository;
        private bool _disposed;
        private IList<IUser> _cachedUsers;

        #endregion

        #region Constructors

        public UnitOfWork(IConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.GetConnection;
            Logger.Debug($"Connection set: {_connection.ConnectionString}");
            _transaction = _connection.BeginTransaction();
        }

        #endregion

        #region Properties

        private IRepository<IMeasurement, IUser, IStepTest, IMeasurement> MeasurementRepository => _measurementRepository ??= new MeasurementRepository(_transaction);

        private IRepository<IStepTest, IUser, IStepTest, IMeasurement> StepTestRepository => _stepTestRepository ??= new StepTestRepository(_transaction);

        private IRepository<IUser, IUser, IStepTest, IMeasurement> UserRepository => _userRepository ??= new UserRepository(_transaction);

        private IList<IUser> CachedUsers => _cachedUsers ??= new List<IUser>();

        private IList<IStepTest> CachedStepTests => CachedUsers.SelectMany(u => u.StepTests).ToList();

        private IList<IMeasurement> CachedMeasurements => CachedUsers.SelectMany(u => u.StepTests.SelectMany(s => s.Measurements)).ToList();

        #endregion

        #region Methods

        public void Commit()
        {
            var updatedUsers = CachedUsers.Where(u => u.State == EntityState.Updated);
            var newUsers = CachedUsers.Where(u => u.State == EntityState.New);
            var updatedStepTests = CachedStepTests.Where(s => s.State == EntityState.Updated);
            var newStepTests = CachedStepTests.Where(s => s.State == EntityState.New);
            var updatedMeasurements = CachedMeasurements.Where(m => m.State == EntityState.Updated);
            var newMeasurements = CachedMeasurements.Where(m => m.State == EntityState.New);

            foreach (var item in newUsers)
            {
                Logger.Info($"Adding new user: {item.LastName}, {item.FirstName}");
                UserRepository.Add(item);
            }

            foreach (var item in newStepTests)
            {
                Logger.Info($"Adding new steptest for user: {item.ParentUser.LastName}, {item.ParentUser.FirstName}");
                StepTestRepository.Add(item);
            }

            foreach (var item in newMeasurements)
            {
                Logger.Info($"Adding new measurement: {item.ParentStepTest.ParentUser.LastName}, {item.ParentStepTest.ParentUser.FirstName}, Steptest: {item.ParentStepTest.Id}");
                MeasurementRepository.Add(item);
            }

            foreach (var item in updatedUsers)
            {
                Logger.Info($"Update user: {item.LastName}, {item.FirstName}");
                UserRepository.Update(item);
            }

            foreach (var item in updatedStepTests)
            {
                Logger.Info($"Update steptest for user: {item.ParentUser.LastName}, {item.ParentUser.FirstName}");
                StepTestRepository.Update(item);
            }

            foreach (var item in updatedMeasurements)
            {
                Logger.Info($"Update measurement: {item.ParentStepTest.ParentUser.LastName}, {item.ParentStepTest.ParentUser.FirstName}, Steptest: {item.ParentStepTest.Id}");
                MeasurementRepository.Update(item);
            }

            try
            {
                _transaction.Commit();
                Logger.Debug("Transaction Committed");
            }

            catch (Exception e)
            {
                _transaction.Rollback();
                Logger.Error("Error in commit, transaction is rolled back!", e);
                throw;
            }

            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                ResetRepositories();
            }
        }

        #region User Methods

        public IList<IUser> GetAllUsers<TUser, TStepTest, TMeasurement>(bool refresh = false) where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement
        {
            if (refresh || CachedUsers.Count == 0)
            {
                _cachedUsers = UserRepository.All<TUser, TStepTest, TMeasurement>().ToList();
            }

            return CachedUsers;
        }

        public IUser GetUserById<TUser, TStepTest, TMeasurement>(int id, bool refresh = false) where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement
        {
            if (refresh || CachedUsers.Count == 0)
            {
                _cachedUsers = UserRepository.All<TUser, TStepTest, TMeasurement>().ToList();
            }

            return _cachedUsers.FirstOrDefault(u => u.Id == id);
        }

        public void UpsertUser(IUser entity)
        {
            if (entity.State == EntityState.New)
            {
                UserRepository.Add(entity);
            }

            else
            {
                UserRepository.Update(entity);
            }
        }

        public void RemoveUser(IUser entity) => UserRepository.Remove(entity);

        #endregion

        #region Step Test Methods

        public IList<IStepTest> GetAllStepTests<TUser, TStepTest, TMeasurement>(bool refresh = false) where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement
        {
            if (refresh || CachedUsers.Count == 0)
            {
                _cachedUsers = UserRepository.All<TUser, TStepTest, TMeasurement>().ToList();
            }

            return CachedStepTests;
        }

        public IStepTest GetStepTestById<TUser, TStepTest, TMeasurement>(int id, bool refresh = false) where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement
        {
            if (refresh || CachedUsers.Count == 0)
            {
                _cachedUsers = UserRepository.All<TUser, TStepTest, TMeasurement>().ToList();
            }

            return CachedStepTests.FirstOrDefault(s => s.Id == id);
        }

        public void UpsertStepTest(IStepTest entity)
        {
            if (entity.State == EntityState.New)
            {
                StepTestRepository.Add(entity);
            }

            else
            {
                StepTestRepository.Update(entity);
            }
        }

        public void RemoveStepTest(IStepTest entity) => StepTestRepository.Remove(entity);

        #endregion

        #region Measurements Methods

        public IList<IMeasurement> GetAllMeasurements<TUser, TStepTest, TMeasurement>(bool refresh = false) where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement
        {
            if (refresh || CachedUsers.Count == 0)
            {
                _cachedUsers = UserRepository.All<TUser, TStepTest, TMeasurement>().ToList();
            }

            return CachedMeasurements;
        }

        public IMeasurement GetMeasurementById<TUser, TStepTest, TMeasurement>(int id, bool refresh = false) where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement
        {
            if (refresh || CachedUsers.Count == 0)
            {
                _cachedUsers = UserRepository.All<TUser, TStepTest, TMeasurement>().ToList();
            }

            return CachedMeasurements.FirstOrDefault(m => m.Id == id);
        }

        public void UpsertMeasurement(IMeasurement entity)
        {
            if (entity.State == EntityState.New)
            {
                MeasurementRepository.Add(entity);
            }

            else
            {
                MeasurementRepository.Update(entity);
            }
        }

        public void RemoveMeasurement(IMeasurement entity) => MeasurementRepository.Remove(entity);

        #endregion

        #endregion

        #region IDisposable Support

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void ResetRepositories()
        {
            _measurementRepository = null;
            _stepTestRepository = null;
            _userRepository = null;
            Logger.Debug("Repositories is resetted");
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }

                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }

                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        #endregion
    }
}
