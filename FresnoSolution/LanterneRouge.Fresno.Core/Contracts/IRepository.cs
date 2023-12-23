namespace LanterneRouge.Fresno.Core.Contracts
{
    public interface IRepository<TEntity, TParentEntity>
    {
        void Add(TEntity entity);

        IEnumerable<TEntity> All();

        TEntity? FindSingle(Guid id);

        IEnumerable<TEntity> FindByParentId(TParentEntity parent);

        int GetCountByParentId(TParentEntity parent, bool onlyInCalculation);

        void Remove(Guid id);

        void Remove(TEntity entity);

        void Update(TEntity entity);

        bool IsChanged(TEntity entity);
    }
}
