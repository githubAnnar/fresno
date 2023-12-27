using LanterneRouge.Fresno.Core.Contracts;
using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Entity.Extentions;
using LanterneRouge.Fresno.Core.Infrastructure;
using LanterneRouge.Fresno.Core.Interface;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace LanterneRouge.Fresno.Core.Repository
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(UserRepository));

        public UserRepository(StepTestContext context) : base(context)
        { }

        public async Task<IList<User>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            var users = Context.Users
                .AsNoTracking();
            Logger.Debug("Return all users");
            return await users.ToListAsync(cancellationToken);
        }

        public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Logger.Debug($"{nameof(GetUserByIdAsync)}({id})");
            var user = await Context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(s => s.Id == id, cancellationToken);
            return user;
        }

        public async Task<User?> InsertUserAsync(IUserEntity userEntity, CancellationToken cancellationToken = default)
        {
            if (userEntity == null)
            {
                Logger.Error($"ArgumentNullException, {nameof(userEntity)} is null");
                throw new ArgumentNullException(nameof(userEntity));
            }

            var newId = Guid.NewGuid();

            if (userEntity is User user)
            {
                try
                {
                    user.Id = newId;
                    var newEntity = Context.Users.Add(user);
                    await Context.SaveChangesAsync(cancellationToken);
                    Logger.Info($"Added User {newEntity.Entity.Id}");
                }

                catch (Exception ex)
                {
                    Logger.Error($"Commit error for adding user '{userEntity.Id}", ex);
                }
            }

            var response = await Context.Users.AsNoTracking().SingleOrDefaultAsync(s => s.Id == newId, cancellationToken);

            return response;
        }

        public async Task<User?> UpdateUserAsync(IUserEntity userEntity, CancellationToken cancellationToken = default)
        {
            if (userEntity == null)
            {
                Logger.Error($"ArgumentNullException, {nameof(userEntity)} is null");
                throw new ArgumentNullException(nameof(userEntity));
            }

            User? response = null;
            try
            {
                var updateObject = await Context.Users.SingleOrDefaultAsync(s => s.Id == userEntity.Id, cancellationToken);
                if (updateObject != null)
                {
                    updateObject.CopyFrom(userEntity);
                    var updateEntity = Context.Users.Update(updateObject);
                    await Context.SaveChangesAsync(cancellationToken);
                    response = updateEntity.Entity;
                    Logger.Info($"Updated {userEntity.Id}");
                }

                else
                {
                    Logger.Warn($"{userEntity.Id} was not updated");
                }
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for updating user {userEntity.Id}", ex);
            }

            return response;
        }

        public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await Context.Users.SingleOrDefaultAsync(m => m.Id == id, cancellationToken);
                if (user != null)
                {
                    Context.Users.Remove(user);
                    await Context.SaveChangesAsync(cancellationToken);
                    Logger.Info($"Deleted user {id}");
                }

                else
                {
                    Logger.Warn($"Delete. User {id} was not found");
                }
            }

            catch (Exception ex)
            {
                Logger.Error($"Commit error for deleting user {id}", ex);
            }
        }

        public async Task<bool> IsChangedAsync(IUserEntity userEntity, CancellationToken cancellationToken = default) => await Task.Run(() => userEntity is User user && Context.Entry(userEntity).State != EntityState.Unchanged);

        public async Task<User> GetUserByStepTestIdAsync(Guid stepTestId, CancellationToken cancellationToken = default)
        {
            Logger.Debug($"{nameof(GetUserByStepTestIdAsync)}({stepTestId})");
            var user = await Context.Users.AsNoTracking().SingleAsync(u => u.Id == stepTestId, cancellationToken);
            return user;
        }
    }
}
