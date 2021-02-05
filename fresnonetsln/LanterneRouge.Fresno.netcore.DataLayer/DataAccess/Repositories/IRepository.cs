using System.Collections.Generic;

namespace LanterneRouge.Fresno.netcore.DataLayer.DataAccess.Repositories
{
    public interface IRepository<TEntity, TParentEntity> where TEntity : class where TParentEntity : class
    {
        void Add(TEntity entity);

        IEnumerable<TEntity> All();

        TEntity FindSingle(int id);
        
        TEntity FindWithParent(int id);

        TEntity FindWithParentAndChilds(int id);

        IEnumerable<TEntity> FindByParentId(TParentEntity parent);

        void Remove(int id);

        void Remove(TEntity entity);

        void Update(TEntity entity);
    }
}
