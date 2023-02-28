using LanterneRouge.Fresno.Core.Entities;

namespace LanterneRouge.Fresno.Core.Contracts
{
    public interface IRepository<TEntity> where TEntity : class, IEntity<TEntity>
    {
        void Add(TEntity entity);

        IEnumerable<TEntity> All();

        TEntity FindSingle(int id);

        TEntity FindWithParent(int id);

        TEntity FindWithParentAndChilds(int id);

        IEnumerable<TEntity> FindByParentId<TParentEntity>(TParentEntity parent) where TParentEntity : class, IEntity<TParentEntity>;

        void Remove(int id);

        void Remove(TEntity entity);

        void Update(TEntity entity);
    }
}
