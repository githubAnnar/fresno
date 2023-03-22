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

            Logger.Debug("Returning All");
            return measurements;
        }

        public Measurement? FindSingle(int id)
        {
            Logger.Debug($"FindSingle({id})");
            return Connection.Query<Measurement>("SELECT * FROM Measurement WHERE Id = @MeasurementId", param: new { MeasurementId = id }, transaction: Transaction).FirstOrDefault();
        }

        public void Add(Measurement entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
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
                throw new ArgumentNullException(nameof(entity));
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
                throw new ArgumentNullException(nameof(entity));
            }

            Remove(entity.Id);
        }

        public void Remove(int id)
        {
            Connection.Execute("DELETE FROM Measurement WHERE Id = @Id", param: new { Id = id }, transaction: Transaction);
            Logger.Info($"Removed {id}");
        }

        public IEnumerable<Measurement> FindByParentId<TParentEntity>(TParentEntity parent) where TParentEntity : BaseEntity<TParentEntity>
        {
            Logger.Debug($"FindByParentId {parent.Id}");
            var measurements = Connection.Query<Measurement>("SELECT * FROM Measurement WHERE StepTestId = @ParentId", param: new { ParentId = parent.Id }, transaction: Transaction).ToList();

            return measurements;
        }
    }
}
