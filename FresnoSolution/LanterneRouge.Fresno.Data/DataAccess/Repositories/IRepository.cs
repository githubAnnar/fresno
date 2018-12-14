using System.Collections.Generic;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        IEnumerable<T> All();

        T Find(int id);

        IEnumerable<T> FindByParentId(int parentId);

        void Remove(int id);

        void Remove(T entity);

        void Update(T entity);
    }
}
