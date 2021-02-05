using System;

namespace LanterneRouge.Fresno.netcore.DataLayer.DataAccess.Entities
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
