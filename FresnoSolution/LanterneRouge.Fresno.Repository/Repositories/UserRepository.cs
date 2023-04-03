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

        public UserRepository(IDbConnection connection) : base(connection)
        { }

        public void Add(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            var transaction = Connection.BeginTransaction();
            try
            {
                var newId = Connection.ExecuteScalar<int>("INSERT INTO User(FirstName, LastName, Email, Street, PostCode, PostCity, BirthDate, Height, Sex, MaxHr) VALUES(@FirstName, @LastName, @Email, @Street, @PostCode, @PostCity, @BirthDate, @Height, @Sex, @MaxHr); SELECT last_insert_rowid()", param: new { entity.FirstName, entity.LastName, entity.Email, entity.Street, entity.PostCode, entity.PostCity, entity.BirthDate, entity.Height, entity.Sex, entity.MaxHr }, transaction: transaction);
                transaction.Commit();
                var t = typeof(BaseEntity<User>);
                t.GetProperty("Id").SetValue(entity, newId, null);
                entity.AcceptChanges();
                Logger.Info($"Added user with ID: {entity.Id}");
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for adding user '{entity.FirstName}, {entity.LastName}'", ex);
                try
                {
                    transaction.Rollback();
                }

                catch (Exception ex2)
                {
                    Logger.Error($"Rollback Exception Type '{ex2.GetType()}' for user '{entity.FirstName}, {entity.LastName}'", ex2);
                }
            }
        }

        public IEnumerable<User> All()
        {
            var allUsers = Connection.Query<User>("SELECT * FROM User").ToList();
            allUsers.ForEach(u =>
            {
                u.IsLoaded = true;
                u.AcceptChanges();
            });

            Logger.Debug("Returning all users");
            return allUsers;
        }

        public User FindSingle(int id)
        {
            Logger.Debug($"FindSingle({id})");
            var user = Connection.Query<User>("SELECT * FROM User WHERE Id = @Id", param: new { Id = id }).FirstOrDefault();
            if (user != null)
            {
                user.IsLoaded = true;
                user.AcceptChanges();
                return user;
            }

            return User.Empty;
        }

        public IEnumerable<User> FindByParentId<TParentEntity>(TParentEntity parent) where TParentEntity : BaseEntity<TParentEntity>
        {
            Logger.Debug($"{nameof(FindByParentId)} is NULL");
            return new List<User>();
        }

        public void Remove(int id)
        {
            var transaction = Connection.BeginTransaction();
            try
            {
                Connection.Execute("DELETE FROM User WHERE Id = @Id", param: new { Id = id }, transaction: transaction);
                transaction.Commit();
                Logger.Info($"Removed user {id}");
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for removing user {id}", ex);
                try
                {
                    transaction.Rollback();
                }

                catch (Exception ex2)
                {
                    Logger.Error($"Rollback Exception Type '{ex2.GetType()}' for user {id}", ex2);
                }
            }
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
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var transaction = Connection.BeginTransaction();
            try
            {
                Connection.Execute("UPDATE User SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Street = @Street, PostCode = @PostCode, PostCity = @PostCity, BirthDate = @BirthDate, Height = @Height, Sex = @Sex, MaxHr = @MaxHr WHERE Id = @Id", param: new { entity.Id, entity.FirstName, entity.LastName, entity.Email, entity.Street, entity.PostCode, entity.PostCity, entity.BirthDate, entity.Height, entity.Sex, entity.MaxHr }, transaction: transaction);
                transaction.Commit();
                entity.AcceptChanges();
                Logger.Info($"Updated {entity.Id}");
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for updating user {entity.Id}", ex);
                try
                {
                    transaction.Rollback();
                }

                catch (Exception ex2)
                {
                    Logger.Error($"Rollback Exception Type '{ex2.GetType()}' for user {entity.Id}", ex2);
                }
            }
        }
    }
}
