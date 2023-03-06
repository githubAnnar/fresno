using LanterneRouge.Fresno.Core.Entities;

namespace LanterneRouge.Fresno.Core.Contracts
{
    public interface IRepository<TEntity> where TEntity : BaseEntity<TEntity>
    {
        void Add(TEntity entity);

        IEnumerable<TEntity> All();

        TEntity FindSingle(int id);

        TEntity FindWithParent(int id);

        TEntity FindWithParentAndChilds(int id);

        IEnumerable<TEntity> FindByParentId<TParentEntity>(TParentEntity parent) where TParentEntity : BaseEntity<TParentEntity>;

        void Remove(int id);

        void Remove(TEntity entity);

        void Update(TEntity entity);
    }
}
