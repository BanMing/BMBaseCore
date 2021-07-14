using System;
using System.Collections;
using System.Collections.Generic;

namespace BMBaseCore.Entities
{
    using ComponentFlags = BMBaseCore.Collections.BoolArray128;

    public struct EntityQuery<TEntity> : IEnumerable<Entity<TEntity>> where TEntity : EntityType
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
            IndexPool.Enumerator enumerator = _entitySystem.GetEntityIndexEnumerator();

            // If we`re not excluding components ("HasNot") then use simpler bit test case within the loop
            if (_hasNone == ComponentFlags.None)
            {
                while (enumerator.MoveNext())
                {
                    int index = enumerator.Current;
                    if (_entitySystem.componentFlags[index].Matches(_hasAll))
                    {
                        process(_entitySystem.entities[index]);
                    }
                }
            }
            else
            {
                while (enumerator.MoveNext())
                {
                    int index = enumerator.Current;
                    if (_entitySystem.componentFlags[index].Matches(_hasAll, _hasNone))
                    {
                        process(_entitySystem.entities[index]);
                    }
                }
            }

            return this;
        }
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this._entitySystem, _hasAll, _hasNone);
        }

        IEnumerator<Entity<TEntity>> IEnumerable<Entity<TEntity>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct Enumerator : IEnumerator<Entity<TEntity>>
        {
            private EntitySystem<TEntity> _entitySystem;
            private IndexPool.Enumerator _entityIndexEnumerator;
            private Entity<TEntity> _current;
            private ComponentFlags _hasAll;
            private ComponentFlags _hasNone;

            public Entity<TEntity> Current => _current;

            object IEnumerator.Current => throw new NotImplementedException();

            internal Enumerator(EntitySystem<TEntity> entitySystem, ComponentFlags hasAll, ComponentFlags hasNone)
            {
                _entitySystem = entitySystem;
                _entityIndexEnumerator = entitySystem.GetEntityIndexEnumerator();
                _current = Entity<TEntity>.None;
                _hasAll = hasAll;
                _hasNone = hasNone;
            }

            public void Dispose()
            {
                _entityIndexEnumerator.Dispose();
                _entitySystem = null;
            }

            public bool MoveNext()
            {
                while (_entityIndexEnumerator.MoveNext())
                {
                    int index = _entityIndexEnumerator.Current;
                    if (_entitySystem.componentFlags[index].Matches(_hasAll, _hasNone))
                    {
                        _current = _entitySystem.entities[index];
                        return true;
                    }
                }
                return false;
            }

            public void Reset()
            {
                _entityIndexEnumerator.Reset();
            }
        }
    }
}
