using Dapper;
using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.Repositories
{
    public class StepTestRepository : RepositoryBase, IRepository<StepTest>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(StepTestRepository));

        public StepTestRepository(IDbTransaction transaction) : base(transaction)
        { }

        public void Add(StepTest entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var newId = Connection.ExecuteScalar<int>("INSERT INTO StepTest(UserId, TestType, EffortUnit, StepDuration, LoadPreset, Increase, Temperature, Weight, TestDate) VALUES(@UserId, @TestType, @EffortUnit, @StepDuration, @LoadPreset, @Increase, @Temperature, @Weight, @TestDate); SELECT last_insert_rowid()", param: new { entity.UserId, entity.TestType, entity.EffortUnit, entity.StepDuration, entity.LoadPreset, entity.Increase, entity.Temperature, entity.Weight, entity.TestDate }, transaction: Transaction);
            var t = typeof(BaseEntity<StepTest>);
            t.GetProperty("Id").SetValue(entity, newId, null);
            entity.AcceptChanges();
            Logger.Info($"Added {entity.Id}");
        }

        public IEnumerable<StepTest> All()
        {
            var stepTests = Connection.Query<StepTest>("SELECT Id, UserId, TestType, EffortUnit, StepDuration, LoadPreset, Increase, Temperature, Weight, TestDate FROM StepTest").ToList();

            // Get parent and childs
            stepTests.ForEach((StepTest stepTest) =>
            {
                stepTest.ParentUser = new UserRepository(Transaction).FindWithParent(stepTest.UserId);
                stepTest.Measurements = new MeasurementRepository(Transaction).FindByParentId(stepTest).ToList();
                stepTest.AcceptChanges();
            });

            Logger.Debug("Returning All");
            return stepTests;
        }

        public StepTest FindSingle(int id)
        {
            Logger.Debug($"FindSingle({id})");
            return Connection.Query<StepTest>("SELECT Id, UserId, TestType, EffortUnit, StepDuration, LoadPreset, Increase, Temperature, Weight, TestDate FROM StepTest WHERE Id = @Id", param: new { Id = id }, transaction: Transaction).FirstOrDefault();
        }

        public StepTest FindWithParent(int id)
        {
            Logger.Debug($"FindWithParent({id})");
            var stepTest = FindSingle(id);
            stepTest.ParentUser = new UserRepository(Transaction).FindWithParent(stepTest.UserId);
            return stepTest;
        }

        public StepTest FindWithParentAndChilds(int id)
        {
            Logger.Debug($"FindWithParentAndChilds({id})");
            var stepTest = FindWithParent(id);
            stepTest.Measurements = new MeasurementRepository(Transaction).FindByParentId(stepTest).ToList();
            return stepTest;
        }

        public void Remove(int id)
        {
            Connection.Execute("DELETE FROM StepTest WHERE Id = @Id", param: new { Id = id }, transaction: Transaction);
            Logger.Info($"Removed {id}");
        }

        public void Remove(StepTest entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Remove(entity.Id);
        }

        public void Update(StepTest entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Connection.Execute("UPDATE StepTest SET UserId = @UserId, TestType = @TestType, EffortUnit = @EffortUnit, StepDuration = @StepDuration, LoadPreset = @LoadPreset, Increase = @Increase, Temperature = @Temperature, Weight = @Weight, TestDate = @TestDate WHERE Id = @Id", param: new { entity.Id, entity.UserId, entity.TestType, entity.EffortUnit, entity.StepDuration, entity.LoadPreset, entity.Increase, entity.Temperature, entity.Weight, entity.TestDate }, transaction: Transaction);
            entity.AcceptChanges();
            Logger.Info($"Updated {entity.Id}");
        }

        public IEnumerable<StepTest> FindByParentId<TParentEntity>(TParentEntity parent) where TParentEntity : class, IEntity<TParentEntity>
        {
            Logger.Debug($"FindByParentId {parent.Id}");
            var stepTests = Connection.Query<StepTest>("SELECT Id, UserId, TestType, EffortUnit, StepDuration, LoadPreset, Increase, Temperature, Weight, TestDate FROM StepTest WHERE UserId = @ParentId", param: new { ParentId = parent.Id }, transaction: Transaction).ToList();
            stepTests.ForEach((StepTest stepTest) =>
            {
                stepTest.ParentUser = parent as IEntity<User>;
                stepTest.Measurements = new MeasurementRepository(Transaction).FindByParentId(stepTest).ToList();
                stepTest.AcceptChanges();
            });
            return stepTests;
        }
    }
}
