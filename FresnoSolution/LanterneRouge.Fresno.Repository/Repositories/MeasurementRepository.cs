using Dapper;
using LanterneRouge.Fresno.Core.Contracts;
using LanterneRouge.Fresno.Core.Entities;
using log4net;
using System.Data;

namespace LanterneRouge.Fresno.Repository.Repositories
{
    public class MeasurementRepository : RepositoryBase, IRepository<Measurement>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MeasurementRepository));

        public MeasurementRepository(IDbTransaction transaction)
            : base(transaction)
        { }

        public IEnumerable<Measurement> All()
        {
            var measurements = Connection.Query<Measurement>("SELECT * FROM Measurement").ToList();
            measurements.ForEach((measurement) =>
            {
                measurement.ParentStepTest = new StepTestRepository(Transaction).FindWithParent(measurement.StepTestId);
                measurement.AcceptChanges();
            });

            Logger.Debug("Returning All");
            return measurements;
        }

        public Measurement FindSingle(int id)
        {
            Logger.Debug($"FindSingle({id})");
            return Connection.Query<Measurement>("SELECT * FROM Measurement WHERE Id = @MeasurementId", param: new { MeasurementId = id }, transaction: Transaction).FirstOrDefault();
        }

        public Measurement FindWithParent(int id)
        {
            Logger.Debug($"FindWithParent({id})");
            var measurement = FindSingle(id);
            measurement.ParentStepTest = new StepTestRepository(Transaction).FindWithParent(measurement.StepTestId);
            return measurement;
        }

        public Measurement FindWithParentAndChilds(int id)
        {
            Logger.Debug($"FindWithParentAndChilds({id})");
            var measurement = FindWithParent(id);
            return measurement;
        }

        public void Add(Measurement entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var newId = Connection.ExecuteScalar<int>("INSERT INTO Measurement(HeartRate, Lactate, Load, StepTestId, Sequence) VALUES(@HeartRate, @Lactate, @Load, @StepTestId, @Sequence); SELECT last_insert_rowid()", param: new { entity.HeartRate, entity.Lactate, entity.Load, entity.StepTestId, entity.Sequence }, transaction: Transaction);
            var t = typeof(BaseEntity<Measurement>);
            t.GetProperty("Id").SetValue(entity, newId, null);
            entity.AcceptChanges();
            Logger.Info($"Added {entity.Id}");
        }

        public void Update(Measurement entity)
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

        public void Remove(Measurement entity)
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

        public IEnumerable<Measurement> FindByParentId<TParentEntity>(TParentEntity parent) where TParentEntity : class, IEntity<TParentEntity>
        {
            Logger.Debug($"FindByParentId {parent.Id}");
            var measurements = Connection.Query<Measurement>("SELECT * FROM Measurement WHERE StepTestId = @ParentId", param: new { ParentId = parent.Id }, transaction: Transaction).ToList();
            measurements.ForEach(m =>
            {
                m.ParentStepTest = parent as IEntity<StepTest>;
                m.AcceptChanges();
            });
            return measurements;
        }
    }
}
