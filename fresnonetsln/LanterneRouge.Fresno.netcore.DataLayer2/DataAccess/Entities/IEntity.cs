namespace LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities
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
