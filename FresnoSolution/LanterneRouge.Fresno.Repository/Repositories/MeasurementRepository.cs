using LanterneRouge.Fresno.Core.Contracts;
using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;
using log4net;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LanterneRouge.Fresno.Repository.Repositories
{
    public class MeasurementRepository : RepositoryBase, IRepository<IMeasurementEntity, IStepTestEntity>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MeasurementRepository));

        public MeasurementRepository(IDbConnection connection) : base(connection)
        { }

        public IEnumerable<IMeasurementEntity> All()
        {
            var measurements = Context.Measurements;

            Logger.Debug("Return all measurements");
            return measurements.ToList();
        }

        public IMeasurementEntity? FindSingle(Guid id)
        {
            Logger.Debug($"FindSingle({id})");
            var measurement = Context.Measurements.SingleOrDefault(x => x.Id == id);
            return measurement;
        }

        public void Add(IMeasurementEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity is Measurement measurement)
            {
                try
                {
                    var newId = Context.Measurements.Add(measurement);
                    Context.SaveChanges();
                    Logger.Info($"Added {newId.Entity.Id}");
                }

                catch (Exception ex)
                {
                    Logger.Error($"Commit error for adding measurement '{entity.StepTestId}, {entity.Sequence}'", ex);
                }
            }
        }

        public void Update(IMeasurementEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                if (entity is Measurement entityObject)
                {
                    Context.Measurements.Update(entityObject);
                    Context.SaveChanges();
                    Logger.Info($"Updated {entity.Id}"); 
                }
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for updating measurement {entity.Id}", ex);
            }
        }

        public void Remove(IMeasurementEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Remove(entity.Id);
        }

        public void Remove(Guid id)
        {
            try
            {
                var measurement = Context.Measurements.Single(m => m.Id == id);
                Context.Measurements.Remove(measurement);
                Logger.Info($"Removed measurement {id}");
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for removing measurement {id}", ex);
            }
        }

        public IEnumerable<IMeasurementEntity> FindByParentId(IStepTestEntity parent)
        {
            Logger.Debug($"FindByParentId {parent.Id}");
            var measurements = Context.Measurements.Where(m => m.StepTestId == parent.Id);

            return measurements.ToList();
        }

        public int GetCountByParentId(IStepTestEntity parent, bool onlyInCalculation)
        {
            var result = Context.Measurements.Where(m => m.StepTestId == parent.Id && m.InCalculation == onlyInCalculation).Count();
            Logger.Debug($"{nameof(GetCountByParentId)} {parent.Id} = {result}");

            return result;
        }

        public bool IsChanged(IMeasurementEntity entity) => entity is Measurement measurementsEntity && Context.Entry(measurementsEntity).State != EntityState.Unchanged;
    }
}
