﻿using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Repository.Contracts
{
    public interface IUserManager : IManagerBase, IDisposable
    {
        IList<IUserEntity> GetAllUsers();

        IUserEntity? GetUserById(Guid id);

        IUserEntity? GetUserByStepTest(IStepTestEntity stepTest);

        void UpsertUser(IUserEntity entity);

        void RemoveUser(IUserEntity entity);

        bool IsChanged(IUserEntity entity);
    }
}
