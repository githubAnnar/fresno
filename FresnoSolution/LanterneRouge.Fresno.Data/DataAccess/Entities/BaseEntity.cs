using System;
using System.Collections.Generic;
using System.Linq;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.Entities
{
    public abstract class BaseEntity<TEntity> : IEntity<TEntity> where TEntity : class
    {
        private readonly string[] PropertyNameSkipArray = new string[] { "IsValid", "IsChanged", "State" };
        private Dictionary<string, object> OriginalValues { get; set; }

        protected BaseEntity()
        { }

        public Dictionary<string, object> GetChanges()
        {
            if (OriginalValues == null || OriginalValues.Count == 0)
            {
                return new Dictionary<string, object>();
            }

            var properties = GetType().GetProperties();
            var latestChanges = new Dictionary<string, object>();

            var tempProperties = GetType().GetProperties().Where(p => !ExcludeName(p.Name) && !Equals(p.GetValue(this, null), OriginalValues[p.Name]));
            foreach (var item in tempProperties)
            {
                latestChanges.Add(item.Name, item.GetValue(this));
            }

            return latestChanges;
        }

        public int Id { get; private set; }

        public EntityState State
        {
            get
            {
                if (Id == 0)
                {
                    return EntityState.New;
                }

                if (Id > 0 && IsChanged)
                {
                    return EntityState.Updated;
                }

                if (Id > 0 && !IsChanged)
                {
                    return EntityState.Saved;
                }

                return EntityState.Deleted;
            }
        }

        public virtual bool IsValid => throw new NotImplementedException();

        public bool IsChanged => GetChanges().Count != 0;

        public void AcceptChanges()
        {
            OriginalValues = new Dictionary<string, object>();
            var properties = GetType().GetProperties();

            // Save the current value of the properties to our dictionary.
            foreach (var property in properties.Where(p => !ExcludeName(p.Name)))
            {
                OriginalValues.Add(property.Name, property.GetValue(this));
            }
        }

        private bool ExcludeName(string propertyName) => PropertyNameSkipArray.Any(p => p.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
    }
}
