using Dapper;
using LanterneRouge.Fresno.Core.Contracts;
using LanterneRouge.Fresno.Core.Entities;
using log4net;
using System.Data;

namespace LanterneRouge.Fresno.Repository.Repositories
{
    public class StepTestRepository : RepositoryBase, IRepository<StepTest>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(StepTestRepository));

        public StepTestRepository(IDbConnection connection) : base(connection)
        { }

        public void Add(StepTest entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var transaction = Connection.BeginTransaction();
            try
            {
                var newId = Connection.ExecuteScalar<int>("INSERT INTO StepTest(UserId, TestType, EffortUnit, StepDuration, LoadPreset, Increase, Temperature, Weight, TestDate) VALUES(@UserId, @TestType, @EffortUnit, @StepDuration, @LoadPreset, @Increase, @Temperature, @Weight, @TestDate); SELECT last_insert_rowid()", param: new { entity.UserId, entity.TestType, entity.EffortUnit, entity.StepDuration, entity.LoadPreset, entity.Increase, entity.Temperature, entity.Weight, entity.TestDate }, transaction: transaction);
                transaction.Commit();
                var t = typeof(BaseEntity<StepTest>);
                t.GetProperty("Id").SetValue(entity, newId, null);
                entity.AcceptChanges();
                Logger.Info($"Added steptest with ID: {entity.Id}");
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for adding steptest '{entity.UserId}, {entity.TestDate}'", ex);
                try
                {
                    transaction.Rollback();
                }

                catch (Exception ex2)
                {
                    Logger.Error($"Rollback Exception Type '{ex2.GetType()}' for steptest '{entity.UserId}, {entity.TestDate}'", ex2);
                }
            }
        }

        public IEnumerable<StepTest> All()
        {
            var stepTests = Connection.Query<StepTest>("SELECT Id, UserId, TestType, EffortUnit, StepDuration, LoadPreset, Increase, Temperature, Weight, TestDate FROM StepTest").ToList();
            stepTests.ForEach(s => s.AcceptChanges());

            Logger.Debug("Return all steptests");
            return stepTests;
        }

        public StepTest FindSingle(int id)
        {
            Logger.Debug($"FindSingle({id})");
            var stepTest = Connection.Query<StepTest>("SELECT Id, UserId, TestType, EffortUnit, StepDuration, LoadPreset, Increase, Temperature, Weight, TestDate FROM StepTest WHERE Id = @Id", param: new { Id = id }).FirstOrDefault();
            if (stepTest != null)
            {
                stepTest.AcceptChanges();
                return stepTest;
            }

            return StepTest.Empty;
        }

        public void Remove(int id)
        {
            var transaction = Connection.BeginTransaction();
            try
            {
                Connection.Execute("DELETE FROM StepTest WHERE Id = @Id", param: new { Id = id }, transaction: transaction);
                transaction.Commit();
                Logger.Info($"Removed {id}");
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for removing steptest {id}", ex);
                try
                {
                    transaction.Rollback();
                }

                catch (Exception ex2)
                {
                    Logger.Error($"Rollback Exception Type '{ex2.GetType()}' for steptest {id}", ex2);
                }
            }
        }

        public void Remove(StepTest entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Remove(entity.Id);
        }

        public void Update(StepTest entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var transaction = Connection.BeginTransaction();
            try
            {
                Connection.Execute("UPDATE StepTest SET UserId = @UserId, TestType = @TestType, EffortUnit = @EffortUnit, StepDuration = @StepDuration, LoadPreset = @LoadPreset, Increase = @Increase, Temperature = @Temperature, Weight = @Weight, TestDate = @TestDate WHERE Id = @Id", param: new { entity.Id, entity.UserId, entity.TestType, entity.EffortUnit, entity.StepDuration, entity.LoadPreset, entity.Increase, entity.Temperature, entity.Weight, entity.TestDate }, transaction: transaction);
                entity.AcceptChanges();
                transaction.Commit();
                Logger.Info($"Updated {entity.Id}");
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for updating steptest {entity.Id}", ex);
                try
                {
                    transaction.Rollback();
                }

                catch (Exception ex2)
                {
                    Logger.Error($"Rollback Exception Type '{ex2.GetType()}' for steptest {entity.Id}", ex2);
                }
            }
        }

        public IEnumerable<StepTest> FindByParentId<TParentEntity>(TParentEntity parent) where TParentEntity : BaseEntity<TParentEntity>
        {
            Logger.Debug($"FindByParentId {parent.Id}");
            var stepTests = Connection.Query<StepTest>("SELECT Id, UserId, TestType, EffortUnit, StepDuration, LoadPreset, Increase, Temperature, Weight, TestDate FROM StepTest WHERE UserId = @ParentId", param: new { ParentId = parent.Id }).ToList();
            stepTests.ForEach(s => { s.AcceptChanges(); });
            return stepTests;
        }
    }
}
