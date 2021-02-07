using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Repositories
{
    public interface IRepository<TEntity, TParentEntity, TChildEntity> where TEntity : IBaseEntity where TParentEntity : IBaseEntity where TChildEntity : IBaseEntity
    {
        void Add(TEntity entity);

        IEnumerable<TBase> All<TBase>() where TBase : TEntity;

        TBase FindSingle<TBase>(int id) where TBase : TEntity;

        TBase FindWithParent<TBase>(int id) where TBase : TEntity;

        TParent FindWithParentAndChilds<TParent, TChild>(int id) where TParent : TEntity where TChild : TChildEntity;

        IEnumerable<TBase> FindByParentId<TBase>(TParentEntity parent) where TBase : TEntity;

        void Remove(int id);

        void Remove(TEntity entity);

        void Update(TEntity entity);
    }
}
