using System;
using System.Collections.Generic;


namespace BMBaseCore.Entities
{
    using ComponentFlags = Collections.BoolArray128;
    public sealed class EntitySystem<TEntity> : BaseObject where TEntity : EntityType
    {
        /// <summary>
        /// This singleton exists to enable typesafe EntityExtensions.
        /// </summary>
        internal static EntitySystem<TEntity> Instance;

        #region Implementation data structures
        private readonly int _maxComponentsPerEntity;
        private readonly short _capacity;

        private int _componentCount;
        private IndexPool _entityPool;
        private object[] _components;

        internal Entity<TEntity>[] entities;        // "internal" for access from EntityQuery
        internal ComponentFlags[] componentFlags;   // "internal" for access from EntityQuery

        #endregion

        #region Property

        public short Count
        {
            get { return _entityPool.Count; }
        }

        public short Capacity
        {
            get { return _capacity; }
        }

        #endregion

        #region Life 

        public EntitySystem(short capacity, int maxComponentsPerEntity)
        {
            _maxComponentsPerEntity = maxComponentsPerEntity;
            _capacity = capacity;
        }

        public override void Init()
        {
            base.Init();
            Instance = this;
            _entityPool = new IndexPool(_capacity, TypeName + typeof(TEntity).Name);

            entities = new Entity<TEntity>[_capacity];
            componentFlags = new ComponentFlags[_capacity];
            _components = new object[_capacity];
            _componentCount = 0;
        }

        public override void Destroy()
        {
            base.Destroy();

            _entityPool.ReleaseAll();
            _entityPool = null;

            entities = null;
            Instance = null;
            componentFlags = null;

            _components = null;
        }
        #endregion

        #region Entity Operation
        #endregion

        #region Component Operation
        public ComponentId<TEntity, TComponent> AddComponent<TComponent>() where TComponent : class
        {
            Assert.Debug(_componentCount < _maxComponentsPerEntity, "max component count exceeded");
            int component = _componentCount++;
            var id = new ComponentId<TEntity, TComponent>(component);
        }
        #endregion

        #region EntityQuery iteration helpers

        internal IndexPool.Enumerator GetEntityIndexEnumerator()
        {
            return _entityPool.GetEnumerator();
        }

        #endregion
    }
}
