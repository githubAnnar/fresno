using LanterneRouge.Fresno.Core.Contracts;
using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Entity.Extentions;
using LanterneRouge.Fresno.Core.Infrastructure;
using LanterneRouge.Fresno.Core.Interface;
using LanterneRouge.Fresno.Core.Repository;
using log4net;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LanterneRouge.Fresno.Core.Repositories
{
    public class StepTestRepository : RepositoryBase, IStepTestRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(StepTestRepository));

        public StepTestRepository(StepTestContext context) : base(context)
        { }

        public async Task<IList<StepTest>> GetAllStepTestsAsync(CancellationToken cancellationToken = default)
        {
            var stepTests = Context.StepTests
                .AsNoTracking();
            Logger.Debug("Return all step tests");
            return await stepTests.ToListAsync(cancellationToken);
        }

        public async Task<StepTest?> GetStepTestByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Logger.Debug($"{nameof(GetStepTestByIdAsync)}({id})");
            var stepTest = await Context.StepTests
                .AsNoTracking()
                .SingleOrDefaultAsync(s => s.Id == id, cancellationToken);
            return stepTest;
        }

        public async Task<StepTest?> InsertStepTestAsync(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default)
        {
            if (stepTestEntity == null)
            {
                Logger.Error($"ArgumentNullException, {nameof(stepTestEntity)} is null");
                throw new ArgumentNullException(nameof(stepTestEntity));
            }

            var newId = Guid.NewGuid();

            if (stepTestEntity is StepTest stepTest)
            {
                try
                {
                    stepTest.Id = newId;
                    var newEntity = Context.StepTests.Add(stepTest);
                    await Context.SaveChangesAsync(cancellationToken);
                    Logger.Info($"Added StepTest {newEntity.Entity.Id}");
                }

                catch (Exception ex)
                {
                    Logger.Error($"Commit error for adding step test '{stepTestEntity.Id}", ex);
                }
            }

            var response = await Context.StepTests.AsNoTracking().SingleOrDefaultAsync(s => s.Id == newId, cancellationToken);

            return response;
        }

        public async Task<StepTest?> UpdateStepTestAsync(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default)
        {
            if (stepTestEntity == null)
            {
                Logger.Error($"ArgumentNullException, {nameof(stepTestEntity)} is null");
                throw new ArgumentNullException(nameof(stepTestEntity));
            }

            StepTest? response = null;
            try
            {
                var updateObject = await Context.StepTests.SingleOrDefaultAsync(s => s.Id == stepTestEntity.Id, cancellationToken);
                if (updateObject != null)
                {
                    updateObject.CopyFrom(stepTestEntity);
                    var updateEntity = Context.StepTests.Update(updateObject);
                    await Context.SaveChangesAsync(cancellationToken);
                    response = updateEntity.Entity;
                    Logger.Info($"Updated {stepTestEntity.Id}");
                }

                else
                {
                    Logger.Warn($"{stepTestEntity.Id} was not updated");
                }
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for updating step test {stepTestEntity.Id}", ex);
            }

            return response;
        }

        public async Task DeleteStepTestAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var stepTest = await Context.StepTests.SingleOrDefaultAsync(m => m.Id == id, cancellationToken);
                if (stepTest != null)
                {
                    Context.StepTests.Remove(stepTest);
                    await Context.SaveChangesAsync(cancellationToken);
                    Logger.Info($"Deleted step test {id}");
                }

                else
                {
                    Logger.Warn($"Delete. Step Test {id} was not found");
                }
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for deleting step test {id}", ex);
            }
        }

        public async Task<IList<StepTest>> GetStepTestsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            Logger.Debug($"{nameof(GetStepTestsByUserIdAsync)} {userId}");
            var query = Context.StepTests
                .Where(m => m.UserId == userId)
                .AsNoTracking();

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<int> GetCountByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var result = await Context.StepTests.Where(s => s.UserId == userId).CountAsync(cancellationToken);
            Logger.Debug($"{nameof(GetCountByUserIdAsync)} {userId} = {result}");

            return result;
        }

        public async Task<bool> IsChangedAsync(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default) => await Task.Run(() => stepTestEntity is StepTest stepTest && Context.Entry(stepTestEntity).State != EntityState.Unchanged);
    }
}
