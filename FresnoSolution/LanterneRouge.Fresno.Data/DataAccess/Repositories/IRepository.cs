﻿using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IEntity<TEntity>
    {
        void Add(TEntity entity);

        IEnumerable<TEntity> All();

        TEntity FindSingle(int id);

        TEntity FindWithParent(int id);

        TEntity FindWithParentAndChilds(int id);

        IEnumerable<TEntity> FindByParentId<TParentEntity>(TParentEntity parent) where TParentEntity : class, IEntity<TParentEntity>;

        void Remove(int id);

        void Remove(TEntity entity);

        void Update(TEntity entity);
    }
}
