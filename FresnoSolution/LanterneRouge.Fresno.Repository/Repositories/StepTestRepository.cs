using LanterneRouge.Fresno.Core.Contracts;
using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;
using log4net;
using System.Data;

namespace LanterneRouge.Fresno.Repository.Repositories
{
    public class StepTestRepository : RepositoryBase, IRepository<IStepTestEntity, IUserEntity>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(StepTestRepository));

        public StepTestRepository(IDbConnection connection) : base(connection)
        { }

        public void Add(IStepTestEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity is StepTest stepTest)
            {
                try
                {
                    var newId = Context.StepTests.Add(stepTest);
                    Context.SaveChanges();
                    Logger.Info($"Added {newId.Entity.Id}");
                }

                catch (Exception ex)
                {
                    Logger.Error($"Commit error for adding step test for user '{entity.UserId}'", ex);
                }
            }
        }

        public IEnumerable<IStepTestEntity> All()
        {
            var stepTests = Context.StepTests;
            Logger.Debug("Return all steptests");
            return stepTests.ToList();
        }

        public IStepTestEntity? FindSingle(int id)
        {
            Logger.Debug($"FindSingle({id})");
            var stepTest = Context.StepTests.SingleOrDefault(x => x.Id == id);
            return stepTest;
        }

        public void Remove(int id)
        {
            try
            {
                var stepTest = Context.StepTests.Single(m => m.Id == id);
                Context.StepTests.Remove(stepTest);
                Logger.Info($"Removed step test {id}");
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for removing step test {id}", ex);
            }
        }

        public void Remove(IStepTestEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Remove(entity.Id);
        }

        public void Update(IStepTestEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                var stepTest = Context.StepTests.Single(m => m.Id == entity.Id);
                if (stepTest.TestType != entity.TestType)
                {
                    stepTest.TestType = entity.TestType;
                }

                if (stepTest.EffortUnit != entity.EffortUnit)
                {
                    stepTest.EffortUnit = entity.EffortUnit;
                }

                if (stepTest.StepDuration != entity.StepDuration)
                {
                    stepTest.StepDuration = entity.StepDuration;
                }

                if (stepTest.LoadPreset != entity.LoadPreset)
                {
                    stepTest.LoadPreset = entity.LoadPreset;
                }

                if (stepTest.Increase != entity.Increase)
                {
                    stepTest.Increase = entity.Increase;
                }

                if (stepTest.Temperature != entity.Temperature)
                {
                    stepTest.Temperature = entity.Temperature;
                }

                if (stepTest.Weight != entity.Weight)
                {
                    stepTest.Weight = entity.Weight;
                }

                if (stepTest.TestDate != entity.TestDate)
                {
                    stepTest.TestDate = entity.TestDate;
                }

                Context.SaveChanges();


                Logger.Info($"Updated {entity.Id}");
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for updating step test {entity.Id}", ex);
            }
        }

        public IEnumerable<IStepTestEntity> FindByParentId(IUserEntity parent)
        {
            Logger.Debug($"FindByParentId {parent.Id}");
            var stepTests = Context.StepTests.Where(m => m.UserId == parent.Id);

            return stepTests.ToList();
        }

        public int GetCountByParentId(IUserEntity parent, bool onlyInCalculation)
        {
            var result = Context.StepTests.Where(m => m.UserId == parent.Id).Count();
            Logger.Debug($"{nameof(GetCountByParentId)} {parent.Id} = {result}");

            return result;
        }
    }
}
