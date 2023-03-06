namespace LanterneRouge.Fresno.Core.Entities
{
    public enum EntityState
    {
        New,
        Saved,
        Updated,
        Deleted
    }

    public interface IEntity
    {
        int Id { get; }

        EntityState State { get; }

        bool IsValid { get; }
    }
}
