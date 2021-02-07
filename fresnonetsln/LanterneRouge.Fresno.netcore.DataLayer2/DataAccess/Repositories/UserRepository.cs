using Dapper;
using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Repositories
{
    public class UserRepository : RepositoryBase, IRepository<IUser, IUser, IStepTest>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(UserRepository));

        public UserRepository(IDbTransaction transaction) : base(transaction)
        { }

        public void Add(IUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var newId = Connection.ExecuteScalar<int>("INSERT INTO User(FirstName, LastName, Email, Street, PostCode, PostCity, BirthDate, Height, Sex, MaxHr) VALUES(@FirstName, @LastName, @Email, @Street, @PostCode, @PostCity, @BirthDate, @Height, @Sex, @MaxHr); SELECT last_insert_rowid()", param: new { entity.FirstName, entity.LastName, entity.Email, entity.Street, entity.PostCode, entity.PostCity, entity.BirthDate, entity.Height, entity.Sex, entity.MaxHr }, transaction: Transaction);
            var t = typeof(BaseEntity);
            t.GetProperty("Id").SetValue(entity, newId, null);
            entity.AcceptChanges();
            Logger.Info($"Added {entity.Id}");
        }

        public IEnumerable<T> All<T>() where T : IUser
        {
            var allUsers = Connection.Query<T>("SELECT * FROM User").ToList();

            allUsers.ForEach((T user) =>
            {
                user.StepTests = new StepTestRepository(Transaction).FindByParentId<IStepTest>(user).ToList();
                user.IsLoaded = true;
                user.AcceptChanges();
            });

            Logger.Debug("Returning All");
            return allUsers;
        }

        public T FindSingle<T>(int id) where T : IUser
        {
            Logger.Debug($"FindSingle({id})");
            return Connection.Query<T>("SELECT * FROM User WHERE Id = @Id", param: new { Id = id }, transaction: Transaction).FirstOrDefault();
        }

        public T FindWithParent<T>(int id) where T : IUser
        {
            Logger.Debug($"FindWithParent({id})");
            return FindSingle<T>(id);
        }

        public TUserImpl FindWithParentAndChilds<TUserImpl, TStepTestImpl>(int id) where TUserImpl : IUser where TStepTestImpl : IStepTest
        {
            Logger.Debug($"FindWithParentAndChilds({id})");
            var user = FindSingle<TUserImpl>(id);
            user.StepTests = new StepTestRepository(Transaction).FindByParentId<TStepTestImpl>(user).Cast<IStepTest>().ToList();

            return user;
        }

        public IEnumerable<T> FindByParentId<T>(IUser user) where T : IUser
        {
            Logger.Debug($"FindByParentId NULL");
            return null;
        }

        public void Remove(int id)
        {
            Connection.Execute("DELETE FROM User WHERE Id = @Id", param: new { Id = id }, transaction: Transaction);
            Logger.Info($"Removed {id}");
        }

        public void Remove(IUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Remove(entity.Id);
        }

        public void Update(IUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Connection.Execute("UPDATE User SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Street = @Street, PostCode = @PostCode, PostCity = @PostCity, BirthDate = @BirthDate, Height = @Height, Sex = @Sex, MaxHr = @MaxHr WHERE Id = @Id", param: new { entity.Id, entity.FirstName, entity.LastName, entity.Email, entity.Street, entity.PostCode, entity.PostCity, entity.BirthDate, entity.Height, entity.Sex, entity.MaxHr }, transaction: Transaction);
            entity.AcceptChanges();
            Logger.Info($"Updated {entity.Id}");
        }
    }
}
