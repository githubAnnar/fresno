namespace LanterneRouge.Fresno.DataLayer.DataAccess.Entities
{
    public enum EntityState
    {
        New,
        Saved,
        Updated,
        Deleted
    }

    public interface IEntity<TEntity> where TEntity : class
    {
        int Id { get; }

        EntityState State { get; }

        bool IsValid { get; }
    }
}
