using System.Collections.Generic;

namespace LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities
{
    public interface IBaseEntity : IEntity
    {
        public Dictionary<string, object> GetChanges();

        public bool IsChanged { get; }

        public void AcceptChanges();
    }
}
