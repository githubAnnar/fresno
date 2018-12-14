using Dapper;
using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.Repositories
{
    internal class UserRepository : RepositoryBase, IRepository<User>
    {
        public UserRepository(IDbTransaction transaction) : base(transaction)
        { }

        public void Add(User entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            entity.Id = Connection.ExecuteScalar<int>("INSERT INTO User(FirstName, LastName, Email) VALUES(@FirstName, @LastName, @Email); SELECT SCOPE_IDENTITY()", param: new { entity.FirstName, entity.LastName, entity.Email }, transaction: Transaction
            );
        }

        public IEnumerable<User> All()
        {
            return Connection.Query<User>("SELECT * FROM User");
        }

        public User Find(int id)
        {
            return Connection.Query<User>("SELECT * FROM User WHERE Id = @Id", param: new { Id = id }, transaction: Transaction).FirstOrDefault();
        }

        public IEnumerable<User> FindByParentId(int parentId)
        {
            return null;
        }

        public void Remove(int id)
        {
            Connection.Execute("DELETE FROM User WHERE Id = @Id", param: new { Id = id }, transaction: Transaction);
        }

        public void Remove(User entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Remove(entity.Id);
        }

        public void Update(User entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Connection.Execute("UPDATE User SET FirstName = @FirstName, LastName = @LastName, Email = @Email WHERE Id = @Id", param: new { entity.Id, entity.FirstName, entity.LastName, entity.Email }, transaction: Transaction);
        }
    }
}
