using Dapper;
using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Repositories
{
    public class MeasurementRepository : RepositoryBase, IRepository<IMeasurement, IStepTest, IMeasurement>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MeasurementRepository));

        public MeasurementRepository(IDbTransaction transaction)
            : base(transaction)
        { }

        public IEnumerable<T> All<T>() where T : IMeasurement
        {
            var measurements = Connection.Query<T>("SELECT * FROM Measurement").ToList();
            measurements.ForEach((T measurement) =>
            {
                measurement.ParentStepTest = new StepTestRepository(Transaction).FindWithParent<IStepTest>(measurement.StepTestId);
                measurement.AcceptChanges();
            });

            Logger.Debug("Returning All");
            return measurements;
        }

        public T FindSingle<T>(int id) where T : IMeasurement
        {
            Logger.Debug($"FindSingle({id})");
            return Connection.Query<T>("SELECT * FROM Measurement WHERE Id = @MeasurementId", param: new { MeasurementId = id }, transaction: Transaction).FirstOrDefault();
        }

        public T FindWithParent<T>(int id) where T : IMeasurement
        {
            Logger.Debug($"FindWithParent({id})");
            var measurement = FindSingle<T>(id);
            measurement.ParentStepTest = new StepTestRepository(Transaction).FindWithParent<IStepTest>(measurement.StepTestId);
            return measurement;
        }

        public TParent FindWithParentAndChilds<TParent, TChild>(int id) where TParent : IMeasurement where TChild : IMeasurement
        {
            Logger.Debug($"FindWithParentAndChilds({id})");
            var measurement = FindWithParent<TParent>(id);
            return measurement;
        }

        public void Add(IMeasurement entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var newId = Connection.ExecuteScalar<int>("INSERT INTO Measurement(HeartRate, Lactate, Load, StepTestId, Sequence) VALUES(@HeartRate, @Lactate, @Load, @StepTestId, @Sequence); SELECT last_insert_rowid()", param: new { entity.HeartRate, entity.Lactate, entity.Load, entity.StepTestId, entity.Sequence }, transaction: Transaction);
            var t = typeof(BaseEntity);
            t.GetProperty("Id").SetValue(entity, newId, null);
            entity.AcceptChanges();
            Logger.Info($"Added {entity.Id}");
        }

        public void Update(IMeasurement entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var tempText = entity.InCalculation.ToString();

            Connection.Execute("UPDATE Measurement SET HeartRate = @HeartRate, Lactate = @Lactate, Load = @Load, StepTestId = @StepTestId, Sequence = @Sequence, InCalculation = @InCalculation WHERE Id = @Id", param: new { entity.Id, entity.HeartRate, entity.Lactate, entity.Load, entity.StepTestId, entity.Sequence, @InCalculation = tempText }, transaction: Transaction);
            entity.AcceptChanges();
            Logger.Info($"Updated {entity.Id}");
        }

        public void Remove(IMeasurement entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Remove(entity.Id);
        }

        public void Remove(int id)
        {
            Connection.Execute("DELETE FROM Measurement WHERE Id = @Id", param: new { Id = id }, transaction: Transaction);
            Logger.Info($"Removed {id}");
        }

        public IEnumerable<T> FindByParentId<T>(IStepTest parent) where T : IMeasurement
        {
            Logger.Debug($"FindByParentId {parent.Id}");
            var measurements = Connection.Query<T>("SELECT * FROM Measurement WHERE StepTestId = @ParentId", param: new { ParentId = parent.Id }, transaction: Transaction).ToList();
            measurements.ForEach(m =>
            {
                m.ParentStepTest = parent;
                m.AcceptChanges();
            });
            return measurements;
        }
    }
}
