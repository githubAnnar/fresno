using Dapper;
using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Repositories
{
    public class StepTestRepository : RepositoryBase, IRepository<IStepTest, IUser, IMeasurement>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(StepTestRepository));

        public StepTestRepository(IDbTransaction transaction) : base(transaction)
        { }

        public void Add(IStepTest entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var newId = Connection.ExecuteScalar<int>("INSERT INTO StepTest(UserId, TestType, EffortUnit, StepDuration, LoadPreset, Increase, Temperature, Weight, TestDate) VALUES(@UserId, @TestType, @EffortUnit, @StepDuration, @LoadPreset, @Increase, @Temperature, @Weight, @TestDate); SELECT last_insert_rowid()", param: new { entity.UserId, entity.TestType, entity.EffortUnit, entity.StepDuration, entity.LoadPreset, entity.Increase, entity.Temperature, entity.Weight, entity.TestDate }, transaction: Transaction);
            var t = typeof(BaseEntity);
            t.GetProperty("Id").SetValue(entity, newId, null);
            entity.AcceptChanges();
            Logger.Info($"Added {entity.Id}");
        }

        public IEnumerable<T> All<T>() where T : IStepTest
        {
            var stepTests = Connection.Query<T>("SELECT Id, UserId, TestType, EffortUnit, StepDuration, LoadPreset, Increase, Temperature, Weight, TestDate FROM StepTest").ToList();

            // Get parent and childs
            stepTests.ForEach((T stepTest) =>
            {
                stepTest.ParentUser = new UserRepository(Transaction).FindWithParent<IUser>(stepTest.UserId);
                stepTest.Measurements = new MeasurementRepository(Transaction).FindByParentId<IMeasurement>(stepTest).ToList();
                stepTest.AcceptChanges();
            });

            Logger.Debug("Returning All");
            return stepTests;
        }

        public T FindSingle<T>(int id) where T : IStepTest
        {
            Logger.Debug($"FindSingle({id})");
            return Connection.Query<T>("SELECT Id, UserId, TestType, EffortUnit, StepDuration, LoadPreset, Increase, Temperature, Weight, TestDate FROM StepTest WHERE Id = @Id", param: new { Id = id }, transaction: Transaction).FirstOrDefault();
        }

        public T FindWithParent<T>(int id) where T : IStepTest
        {
            Logger.Debug($"FindWithParent({id})");
            var stepTest = FindSingle<T>(id);
            stepTest.ParentUser = new UserRepository(Transaction).FindWithParent<IUser>(stepTest.UserId);
            return stepTest;
        }

        public TParent FindWithParentAndChilds<TParent, TChild>(int id) where TParent : IStepTest where TChild : IMeasurement
        {
            Logger.Debug($"FindWithParentAndChilds({id})");
            var stepTest = FindWithParent<TParent>(id);
            stepTest.Measurements = new MeasurementRepository(Transaction).FindByParentId<TChild>(stepTest).Cast<IMeasurement>().ToList();
            return stepTest;
        }

        public IEnumerable<T> FindByParentId<T>(IUser parent) where T : IStepTest
        {
            Logger.Debug($"FindByParentId {parent.Id}");
            var stepTests = Connection.Query<T>("SELECT Id, UserId, TestType, EffortUnit, StepDuration, LoadPreset, Increase, Temperature, Weight, TestDate FROM StepTest WHERE UserId = @ParentId", param: new { ParentId = parent.Id }, transaction: Transaction).ToList();
            stepTests.ForEach((T stepTest) =>
            {
                stepTest.ParentUser = parent;
                stepTest.Measurements = new MeasurementRepository(Transaction).FindByParentId<IMeasurement>(stepTest).ToList();
                stepTest.AcceptChanges();
            });
            return stepTests;
        }

        public void Remove(int id)
        {
            Connection.Execute("DELETE FROM StepTest WHERE Id = @Id", param: new { Id = id }, transaction: Transaction);
            Logger.Info($"Removed {id}");
        }

        public void Remove(IStepTest entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Remove(entity.Id);
        }

        public void Update(IStepTest entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Connection.Execute("UPDATE StepTest SET UserId = @UserId, TestType = @TestType, EffortUnit = @EffortUnit, StepDuration = @StepDuration, LoadPreset = @LoadPreset, Increase = @Increase, Temperature = @Temperature, Weight = @Weight, TestDate = @TestDate WHERE Id = @Id", param: new { entity.Id, entity.UserId, entity.TestType, entity.EffortUnit, entity.StepDuration, entity.LoadPreset, entity.Increase, entity.Temperature, entity.Weight, entity.TestDate }, transaction: Transaction);
            entity.AcceptChanges();
            Logger.Info($"Updated {entity.Id}");
        }
    }
}
