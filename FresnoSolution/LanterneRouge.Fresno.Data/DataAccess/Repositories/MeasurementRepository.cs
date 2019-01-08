using Dapper;
using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.Repositories
{
    public class MeasurementRepository : RepositoryBase, IRepository<Measurement, StepTest>
    {
        public MeasurementRepository(IDbTransaction transaction)
            : base(transaction)
        { }

        public IEnumerable<Measurement> All()
        {
            var measurements = Connection.Query<Measurement>("SELECT * FROM Measurement").ToList();
            measurements.ForEach((Measurement measurement) =>
            {
                measurement.ParentStepTest = new StepTestRepository(Transaction).FindWithParent(measurement.StepTestId);
                measurement.AcceptChanges();
            });
            return measurements;
        }

        public Measurement FindSingle(int id)
        {
            return Connection.Query<Measurement>("SELECT * FROM Measurement WHERE Id = @MeasurementId", param: new { MeasurementId = id }, transaction: Transaction).FirstOrDefault();
        }

        public Measurement FindWithParent(int id)
        {
            var measurement = FindSingle(id);
            measurement.ParentStepTest = new StepTestRepository(Transaction).FindWithParent(measurement.StepTestId);
            return measurement;
        }

        public Measurement FindWithParentAndChilds(int id)
        {
            var measurement = FindWithParent(id);
            return measurement;
        }

        public void Add(Measurement entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var newId = Connection.ExecuteScalar<int>("INSERT INTO Measurement(HeartRate, Lactate, Load, StepTestId, Sequence) VALUES(@HeartRate, @Lactate, @Load, @StepTestId, @Sequence); SELECT last_insert_rowid()", param: new { entity.HeartRate, entity.Lactate, entity.Load, entity.StepTestId, entity.Sequence }, transaction: Transaction);
            var t = typeof(BaseEntity<Measurement>);
            t.GetProperty("Id").SetValue(entity, newId, null);
            entity.AcceptChanges();
        }

        public void Update(Measurement entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Connection.Execute("UPDATE Measurement SET HeartRate = @HeartRate, Lactate = @Lactate, Load = @Load, StepTestId = @StepTestId, Sequence = @Sequence WHERE Id = @Id", param: new { entity.Id, entity.HeartRate, entity.Lactate, entity.Load, entity.StepTestId, entity.Sequence }, transaction: Transaction);
            entity.AcceptChanges();
        }

        public void Remove(Measurement entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Remove(entity.Id);
        }

        public void Remove(int id)
        {
            Connection.Execute("DELETE FROM Measurement WHERE Id = @Id", param: new { Id = id }, transaction: Transaction);
        }

        public IEnumerable<Measurement> FindByParentId(StepTest parent)
        {
            var measurements = Connection.Query<Measurement>("SELECT * FROM Measurement WHERE StepTestId = @ParentId", param: new { ParentId = parent.Id }, transaction: Transaction).ToList();
            measurements.ForEach(m =>
            {
                m.ParentStepTest = parent;
                m.AcceptChanges();
            });
            return measurements;
        }
    }
}
