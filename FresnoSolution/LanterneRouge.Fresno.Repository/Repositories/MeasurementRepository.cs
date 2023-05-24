using Dapper;
using LanterneRouge.Fresno.Core.Contracts;
using LanterneRouge.Fresno.Core.Entities;
using log4net;
using System.Data;
using System.Text;

namespace LanterneRouge.Fresno.Repository.Repositories
{
    public class MeasurementRepository : RepositoryBase, IRepository<Measurement>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MeasurementRepository));

        public MeasurementRepository(IDbConnection connection) : base(connection)
        { }

        public IEnumerable<Measurement> All()
        {
            var measurements = Connection.Query<Measurement>("SELECT * FROM Measurement").ToList();
            measurements.ForEach(m => m.AcceptChanges());

            Logger.Debug("Return all measurements");
            return measurements;
        }

        public Measurement FindSingle(int id)
        {
            Logger.Debug($"FindSingle({id})");
            var measurement = Connection.Query<Measurement>("SELECT * FROM Measurement WHERE Id = @MeasurementId", param: new { MeasurementId = id }).FirstOrDefault();
            if (measurement != null)
            {
                measurement.AcceptChanges();
                return measurement;
            }

            return Measurement.Empty;
        }

        public void Add(Measurement entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var transaction = Connection.BeginTransaction();
            try
            {
                var newId = Connection.ExecuteScalar<int>("INSERT INTO Measurement(HeartRate, Lactate, Load, StepTestId, Sequence) VALUES(@HeartRate, @Lactate, @Load, @StepTestId, @Sequence); SELECT last_insert_rowid()", param: new { entity.HeartRate, entity.Lactate, entity.Load, entity.StepTestId, entity.Sequence }, transaction: transaction);
                transaction.Commit();
                var t = typeof(BaseEntity<Measurement>);
                t.GetProperty("Id").SetValue(entity, newId, null);
                entity.AcceptChanges();
                Logger.Info($"Added {entity.Id}");
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for adding measurement '{entity.StepTestId}, {entity.Sequence}'", ex);
                try
                {
                    transaction.Rollback();
                }

                catch (Exception ex2)
                {
                    Logger.Error($"Rollback Exception Type '{ex2.GetType()}' for measurement '{entity.StepTestId}, {entity.Sequence}'", ex2);
                }
            }
        }

        public void Update(Measurement entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var transaction = Connection.BeginTransaction();
            try
            {
                var tempText = entity.InCalculation.ToString();

                Connection.Execute("UPDATE Measurement SET HeartRate = @HeartRate, Lactate = @Lactate, Load = @Load, StepTestId = @StepTestId, Sequence = @Sequence, InCalculation = @InCalculation WHERE Id = @Id", param: new { entity.Id, entity.HeartRate, entity.Lactate, entity.Load, entity.StepTestId, entity.Sequence, @InCalculation = tempText }, transaction: transaction);
                transaction.Commit();
                entity.AcceptChanges();
                Logger.Info($"Updated {entity.Id}");
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for updating measurement {entity.Id}", ex);
                try
                {
                    transaction.Rollback();
                }

                catch (Exception ex2)
                {
                    Logger.Error($"Rollback Exception Type '{ex2.GetType()}' for measurement {entity.Id}", ex2);
                }
            }
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
            var transaction = Connection.BeginTransaction();
            try
            {
                Connection.Execute("DELETE FROM Measurement WHERE Id = @Id", param: new { Id = id }, transaction: transaction);
                transaction.Commit();
                Logger.Info($"Removed measurement {id}");
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for removing measurement {id}", ex);
                try
                {
                    transaction.Rollback();
                }

                catch (Exception ex2)
                {
                    Logger.Error($"Rollback Exception Type '{ex2.GetType()}' for measurement {id}", ex2);
                }
            }
        }

        public IEnumerable<Measurement> FindByParentId<TParentEntity>(TParentEntity parent) where TParentEntity : BaseEntity<TParentEntity>
        {
            Logger.Debug($"FindByParentId {parent.Id}");
            var measurements = Connection.Query<Measurement>("SELECT * FROM Measurement WHERE StepTestId = @ParentId", param: new { ParentId = parent.Id }).ToList();
            measurements.ForEach(m => m.AcceptChanges());

            return measurements;
        }

        public int GetCountByParentId<TParentEntity>(TParentEntity parent, bool onlyInCalculation) where TParentEntity : BaseEntity<TParentEntity>
        {
            var sqlBuilder = new StringBuilder("SELECT COUNT(Id) FROM Measurement WHERE StepTestId = @ParentId");
            var paramDictionary = new Dictionary<string, object> { { "@ParentId", 1 } };


            if (onlyInCalculation)
            {
                sqlBuilder.Append(" AND InCalculation = @inC");
                paramDictionary.Add("@inC", "True");
            }


            var result = Connection.ExecuteScalar<int>("SELECT COUNT(Id) FROM Measurement WHERE StepTestId = @ParentId", new DynamicParameters(paramDictionary));
            Logger.Debug($"{nameof(GetCountByParentId)} {parent.Id} = {result}");

            return result;
        }
    }
}
