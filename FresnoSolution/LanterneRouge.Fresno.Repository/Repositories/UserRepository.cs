using Dapper;
using LanterneRouge.Fresno.Core.Contracts;
using LanterneRouge.Fresno.Core.Entities;
using log4net;
using System.Data;

namespace LanterneRouge.Fresno.Repository.Repositories
{
    public class UserRepository : RepositoryBase, IRepository<User>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(UserRepository));

        public UserRepository(IDbTransaction transaction) : base(transaction)
        { }

        public void Add(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var newId = Connection.ExecuteScalar<int>("INSERT INTO User(FirstName, LastName, Email, Street, PostCode, PostCity, BirthDate, Height, Sex, MaxHr) VALUES(@FirstName, @LastName, @Email, @Street, @PostCode, @PostCity, @BirthDate, @Height, @Sex, @MaxHr); SELECT last_insert_rowid()", param: new { entity.FirstName, entity.LastName, entity.Email, entity.Street, entity.PostCode, entity.PostCity, entity.BirthDate, entity.Height, entity.Sex, entity.MaxHr }, transaction: Transaction);
            var t = typeof(BaseEntity<User>);
            t.GetProperty("Id").SetValue(entity, newId, null);
            entity.AcceptChanges();
            Logger.Info($"Added {entity.Id}");
        }

        public IEnumerable<User> All()
        {
            var allUsers = Connection.Query<User>("SELECT * FROM User").ToList();

            Logger.Debug("Returning all users");
            return allUsers;
        }

        public User FindSingle(int id)
        {
            Logger.Debug($"FindSingle({id})");
            var user = Connection.Query<User>("SELECT * FROM User WHERE Id = @Id", param: new { Id = id }, transaction: Transaction).FirstOrDefault();
            return user ?? User.Empty;
        }

        public IEnumerable<User> FindByParentId<TParentEntity>(TParentEntity parent) where TParentEntity : BaseEntity<TParentEntity>
        {
            Logger.Debug($"{nameof(FindByParentId)} is NULL");
            return new List<User>();
        }

        public void Remove(int id)
        {
            Connection.Execute("DELETE FROM User WHERE Id = @Id", param: new { Id = id }, transaction: Transaction);
            Logger.Info($"Removed {id}");
        }

        public void Remove(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Remove(entity.Id);
        }

        public void Update(User entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Connection.Execute("UPDATE User SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Street = @Street, PostCode = @PostCode, PostCity = @PostCity, BirthDate = @BirthDate, Height = @Height, Sex = @Sex, MaxHr = @MaxHr WHERE Id = @Id", param: new { entity.Id, entity.FirstName, entity.LastName, entity.Email, entity.Street, entity.PostCode, entity.PostCity, entity.BirthDate, entity.Height, entity.Sex, entity.MaxHr }, transaction: Transaction);
            entity.AcceptChanges();
            Logger.Info($"Updated {entity.Id}");
        }
    }
}
