using Dapper;
using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.Repositories
{
    public class StepTestRepository : RepositoryBase, IRepository<StepTest>
    {
        public StepTestRepository(IDbTransaction transaction) : base(transaction)
        { }

        public void Add(StepTest entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            entity.Id = Connection.ExecuteScalar<int>("INSERT INTO StepTest(Sequence, UserId) VALUES(@Sequence, @UserId); SELECT last_insert_rowid()", param: new { entity.Sequence, entity.UserId }, transaction: Transaction
            );
        }

        public IEnumerable<StepTest> All()
        {
            return Connection.Query<StepTest>("SELECT * FROM StepTest");
        }

        public StepTest Find(int id)
        {
            return Connection.Query<StepTest>("SELECT * FROM StepTest WHERE Id = @Id", param: new { Id = id }, transaction: Transaction).FirstOrDefault();
        }

        public IEnumerable<StepTest> FindByParentId(int parentId)
        {
            return Connection.Query<StepTest>("SELECT * FROM StepTest WHERE UserId = @ParentId", param: new { ParentId = parentId }, transaction: Transaction);
        }

        public void Remove(int id)
        {
            Connection.Execute("DELETE FROM StepTest WHERE Id = @Id", param: new { Id = id }, transaction: Transaction);
        }

        public void Remove(StepTest entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Remove(entity.Id);
        }

        public void Update(StepTest entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Connection.Execute("UPDATE StepMessage SET Sequence = @Sequence, UserId = @UserId WHERE Id = @Id", param: new { entity.Id, entity.Sequence, entity.UserId }, transaction: Transaction);
        }
    }
}
