using Dapper;
using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.Repositories
{
    public class MeasurementRepository : RepositoryBase, IRepository<Measurement>
    {
        public MeasurementRepository(IDbTransaction transaction)
            : base(transaction)
        { }

        public IEnumerable<Measurement> All()
        {
            return Connection.Query<Measurement>("SELECT * FROM Measurement");
        }

        public Measurement Find(int id)
        {
            return Connection.Query<Measurement>("SELECT * FROM Measurement WHERE Id = @MeasurementId", param: new { MeasurementId = id }, transaction: Transaction).FirstOrDefault();
        }

        public void Add(Measurement entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            entity.Id = Connection.ExecuteScalar<int>("INSERT INTO Measurement(HeartRate, Lactate, Load, StepTestId) VALUES(@HeartRate, @Lactate, @Load, @STepTestId); SELECT last_insert_rowid()", param: new { entity.HeartRate, entity.Lactate, entity.Load, entity.StepTestId }, transaction: Transaction
            );
        }

        public void Update(Measurement entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Connection.Execute("UPDATE Measurement SET Lactate = @Lactate, Load = @Load, StepTestId = @StepTestId WHERE Id = @Id", param: new { entity.Id, entity.Lactate, entity.Load, entity.StepTestId }, transaction: Transaction);
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

        public IEnumerable<Measurement> FindByParentId(int parentId)
        {
            return Connection.Query<Measurement>("SELECT * FROM Measurement WHERE StepTestId = @ParentId", param: new { ParentId = parentId }, transaction: Transaction);
        }
    }
}
