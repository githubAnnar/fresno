using Dapper;
using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Repositories
{
    public class UserRepository : RepositoryBase, IRepository<IUser, IUser, IStepTest, IMeasurement>
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

        public IEnumerable<IUser> All<TUser, TStepTest, TMeasurement>() where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement
        {
            var allUsers = Connection.Query<TUser>("SELECT * FROM User").ToList();

            allUsers.ForEach((TUser user) =>
            {
                user.StepTests = new StepTestRepository(Transaction).FindByParentId<TUser, TStepTest, TMeasurement>(user).Cast<IStepTest>().ToList();
                user.IsLoaded = true;
                user.AcceptChanges();
            });

            Logger.Debug("Returning All");
            return allUsers.Cast<IUser>();
        }

        public IUser FindSingle<TUser>(int id) where TUser : IUser
        {
            Logger.Debug($"FindSingle({id})");
            return Connection.Query<TUser>("SELECT * FROM User WHERE Id = @Id", param: new { Id = id }, transaction: Transaction).FirstOrDefault();
        }

        public IUser FindWithParent<TUser, TStepTest, TMeasurement>(int id) where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement
        {
            Logger.Debug($"FindWithParent({id})");
            return FindSingle<TUser>(id);
        }

        public IUser FindWithParentAndChilds<TUser, TStepTest, TMeasurement>(int id) where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement
        {
            Logger.Debug($"FindWithParentAndChilds({id})");
            var user = FindSingle<TUser>(id);
            user.StepTests = new StepTestRepository(Transaction).FindByParentId<TUser, TStepTest, TMeasurement>(user).Cast<IStepTest>().ToList();

            return user;
        }

        public IEnumerable<IUser> FindByParentId<TUser, TStepTest, TMeasurement>(IBaseEntity user) where TUser : IUser where TStepTest : IStepTest where TMeasurement : IMeasurement
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
