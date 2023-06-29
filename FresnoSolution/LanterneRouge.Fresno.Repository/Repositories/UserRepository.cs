using LanterneRouge.Fresno.Core.Contracts;
using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;
using log4net;
using System.Data;

namespace LanterneRouge.Fresno.Repository.Repositories
{
    public class UserRepository : RepositoryBase, IRepository<IUserEntity, IUserEntity>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(UserRepository));

        public UserRepository(IDbConnection connection) : base(connection)
        { }

        public void Add(IUserEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity is User user)
            {
                try
                {
                    var newId = Context.Users.Add(user);
                    Context.SaveChanges();
                    Logger.Info($"Added {newId.Entity.Id}");
                }

                catch (Exception ex)
                {
                    Logger.Error($"Commit error for adding user '{entity.LastName}, {entity.FirstName}'", ex);
                }
            }
        }

        public IEnumerable<IUserEntity> All()
        {
            var users = Context.Users;

            Logger.Debug("Return all users");
            return users.ToList();
        }

        public IUserEntity? FindSingle(int id)
        {
            Logger.Debug($"FindSingle({id})");
            var user = Context.Users.SingleOrDefault(x => x.Id == id);
            return user;
        }

        public IEnumerable<IUserEntity> FindByParentId(IUserEntity user)
        {
            Logger.Debug($"{nameof(FindByParentId)} is NULL");
            return new List<IUserEntity>();
        }

        public int GetCountByParentId(IUserEntity parent, bool onlyInCalculation) => 0;

        public void Remove(int id)
        {
            try
            {
                var user = Context.Users.Single(m => m.Id == id);
                Context.Users.Remove(user);
                Logger.Info($"Removed user {id}");
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for removing user {id}", ex);
            }
        }

        public void Remove(IUserEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Remove(entity.Id);
        }

        public void Update(IUserEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                var user = Context.Users.Single(m => m.Id == entity.Id);
                if (user.FirstName != entity.FirstName)
                {
                    user.FirstName = entity.FirstName;
                }

                if (user.LastName != entity.LastName)
                {
                    user.LastName = entity.LastName;
                }

                if (user.Email != entity.Email)
                {
                    user.Email = entity.Email;
                }

                if (user.Street != entity.Street)
                {
                    user.Street = entity.Street;
                }

                if (user.PostCode != entity.PostCode)
                {
                    user.PostCode = entity.PostCode;
                }

                if (user.PostCity != entity.PostCity)
                {
                    user.PostCity = entity.PostCity;
                }

                if (user.BirthDate != entity.BirthDate)
                {
                    user.BirthDate = entity.BirthDate;
                }

                if (user.Height != entity.Height)
                {
                    user.Height = entity.Height;
                }

                if (user.Sex != entity.Sex)
                {
                    user.Sex = entity.Sex;
                }

                if (user.MaxHr != entity.MaxHr)
                {
                    user.MaxHr = entity.MaxHr;
                }

                Context.SaveChanges();


                Logger.Info($"Updated {entity.Id}");
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for updating user {entity.Id}", ex);
            }
        }
    }
}
