﻿using LanterneRouge.Fresno.Core.Contracts;
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
    public class MeasurementRepository : RepositoryBase, IMeasurementRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MeasurementRepository));

        public MeasurementRepository(StepTestContext context) : base(context)
        { }

        public async Task<IList<Measurement>> GetAllMeasurementsAsync(CancellationToken cancellationToken = default)
        {
            var measurements = Context.Measurements
                .AsNoTracking();

            Logger.Debug("Return all measurements");
            return await measurements.ToListAsync(cancellationToken);
        }

        public async Task<Measurement?> GetMeasurementByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Logger.Debug($"{nameof(GetMeasurementByIdAsync)}({id})");
            var measurement = await Context.Measurements
                .AsNoTracking()
                .SingleOrDefaultAsync(measurement => measurement.Id == id, cancellationToken);
            return measurement;
        }

        public async Task<Measurement?> InsertMeasurementAsync(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default)
        {
            if (measurementEntity == null)
            {
                Logger.Error($"ArgumentNullException, {nameof(measurementEntity)} is null");
                throw new ArgumentNullException(nameof(measurementEntity));
            }

            var newId = Guid.NewGuid();

            if (measurementEntity is Measurement measurement)
            {
                try
                {
                    measurement.Id = newId;
                    var newEntity = Context.Measurements.Add(measurement);
                    await Context.SaveChangesAsync(cancellationToken);
                    Logger.Info($"Added Measurement {newEntity.Entity.Id}");
                }

                catch (Exception ex)
                {
                    Logger.Error($"Commit error for adding measurement '{measurementEntity.StepTestId}, {measurementEntity.Sequence}'", ex);
                }
            }

            var response = await Context.Measurements.AsNoTracking().SingleOrDefaultAsync(measurement => measurement.Id == newId, cancellationToken);

            return response;
        }

        public async Task<Measurement?> UpdateMeasurementAsync(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default)
        {
            if (measurementEntity == null)
            {
                Logger.Error($"ArgumentNullException, {nameof(measurementEntity)} is null");
                throw new ArgumentNullException(nameof(measurementEntity));
            }

            Measurement? response = null;
            try
            {
                var updateObject = await Context.Measurements.SingleOrDefaultAsync(measurement => measurement.Id == measurementEntity.Id, cancellationToken);
                if (updateObject != null)
                {
                    updateObject.CopyFrom(measurementEntity);
                    var updateEntity = Context.Measurements.Update(updateObject);
                    await Context.SaveChangesAsync(cancellationToken);
                    response = updateEntity.Entity;
                    Logger.Info($"Updated {measurementEntity.Id}");
                }

                else
                {
                    Logger.Warn($"{measurementEntity.Id} was not updated");
                }
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for updating measurement {measurementEntity.Id}", ex);
            }

            return response;
        }

        public async Task DeleteMeasurementAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var measurement = await Context.Measurements.SingleOrDefaultAsync(m => m.Id == id, cancellationToken);
                if (measurement != null)
                {
                    Context.Measurements.Remove(measurement);
                    await Context.SaveChangesAsync(cancellationToken);
                    Logger.Info($"Deleted measurement {id}");
                }

                else
                {
                    Logger.Warn($"Delete. Measurement {id} was not found");
                }
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for deleting measurement {id}", ex);
            }
        }

        public async Task<IList<Measurement>> GetMeasurementsByStepTestIdAsync(Guid stepTestId, CancellationToken cancellationToken = default)
        {
            Logger.Debug($"{nameof(GetMeasurementsByStepTestIdAsync)} {stepTestId}");
            var measurementQuery = Context.Measurements
                .Where(m => m.StepTestId == stepTestId)
                .AsNoTracking();

            return await measurementQuery.ToListAsync(cancellationToken);
        }

        public async Task<int> GetCountByStepTestIdAsync(Guid stepTestId, bool onlyInCalculation, CancellationToken cancellationToken = default)
        {
            var result = await Context.Measurements.Where(m => m.StepTestId == stepTestId && m.InCalculation == onlyInCalculation).CountAsync(cancellationToken);
            Logger.Debug($"{nameof(GetCountByStepTestIdAsync)} {stepTestId} = {result}");

            return result;
        }

        public async Task<bool> IsChangedAsync(IMeasurementEntity entity, CancellationToken cancellationToken = default) => await Task.Run(() => entity is Measurement measurementsEntity && Context.Entry(measurementsEntity).State != EntityState.Unchanged);
    }
}