using System;
using System.Collections;
using System.Collections.Generic;

namespace BMBaseCore.Entities
{
    using ComponentFlags = BMBaseCore.Collections.BoolArray128;

    public struct EntityQuery<TEntity> : IEnumerable<EntityQuery<TEntity>> where TEntity : EntityType
    {
        private readonly EntitySystem<TEntity> _entitySystem;
        private ComponentFlags _hasAll;
        private ComponentFlags _hasNone;

        internal EntityQuery(EntitySystem<TEntity> entitySystem)
        {
            _entitySystem = entitySystem;
            _hasAll = default(ComponentFlags);
            _hasNone = default(ComponentFlags);
        }

        public EntityQuery<TEntity> Has<TComponent>(ComponentId<TEntity, TComponent> id) where TComponent : class
        {
            _hasAll[id.ComponentIndex] = true;
            return this;
        }

        public EntityQuery<TEntity> HasNot<TComponent>(ComponentId<TEntity, TComponent> id) where TComponent : class
        {
            _hasNone[id.ComponentIndex] = true;
            return this;
        }

        /// <summary>
        /// Preform an actions on all entities matched by the query
        /// </summary>
        /// <param name="process">The operation to perform. Passing a method will cause
        /// an allocation to wrap it in an Action delegate, so it is preferable to pre-create
        /// and cache the delegate.</param>
        /// <returns></returns>
        public EntityQuery<TEntity> Do(Action<Entity<TEntity>> process)
        {
            //IndexPool.E
            return this;
        }

        public IEnumerator<EntityQuery<TEntity>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
