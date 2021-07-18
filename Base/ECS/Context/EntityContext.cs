using System;
using System.Collections.Generic;
using BMBaseCore.Threading;


namespace BMBaseCore.ECS
{
    using ComponentFlags = Collections.BoolArray128;
    public sealed class EntityContext<TEntity> : BaseObject where TEntity : EntityType
    {
        /// <summary>
        /// This singleton exists to enable typesafe EntityExtensions.
        /// </summary>
        internal static EntityContext<TEntity> Instance;

        #region Implementation data structures

        /// <summary>
        /// cause we map entity to components by BoolArray128 
        /// </summary>
        public const int kMaxComponentsPerEntity = 128;

        /// <summary>
        /// If set,then used to ensure access to entity validity and components occur on same thread as owner of the
        /// EntitySystem (to catch erroneous calls into sim).
        /// </summary>
        private readonly Threading.ThreadAffinity _threadAffinity;

        private readonly int _maxComponentsPerEntity;
        private readonly short _capacity;

        private int _componentCount;
        private IndexPool _entityPool;
        private object[] _components;
        private ushort _entityCreationID; // Ever-increasing entity "generation" ID

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

        public EntityContext(short capacity, int maxComponentsPerEntity, Threading.ThreadAffinity threadAffinity = null)
        {
            Assert.Debug(maxComponentsPerEntity <= kMaxComponentsPerEntity, "max components is too high");
            _maxComponentsPerEntity = maxComponentsPerEntity;
            _capacity = capacity;
            _threadAffinity = threadAffinity;
        }

        public override void Initialize()
        {
            base.Initialize();

            Assert.Debug(Instance == null, "entity system has already been initialized");

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

        #region Thread Affinity
        public void RegisterWokerThread(int threadID)
        {
            if (_threadAffinity != null)
            {
                _threadAffinity.RegisterWorkerThread(threadID);
            }
        }

        public void UnregisterWokerThread(int threadID)
        {
            if (_threadAffinity != null)
            {
                _threadAffinity.UnregisterWorkerThread(threadID);
            }
        }
        #endregion

        #region Entity Operation

        public Entity<TEntity> CreateEntity(string debugLabel)
        {
            _threadAffinity.Validate();

            short entityIndex = _entityPool.Reserve();
            if (entityIndex >= 0)
            {
                _entityCreationID++;

                // Make sure that we add an extra creation ID jump if the creation ID roll over to zero.
                if (_entityCreationID == 0)
                {
                    _entityCreationID++;
                }

                Entity<TEntity> entity = new Entity<TEntity>(entityIndex, _entityCreationID);
                entities[entityIndex] = entity;

                return entity;
            }

            Assert.DebugFail("entity system overflow");
            return Entity<TEntity>.None;
        }

        public void DeleteEntity(Entity<TEntity> entity)
        {
            _threadAffinity.Validate();
            Assert.Debug(IsValid(entity), "Invalid entity,already deleted?");

            // Release all pooled components
            //entity.ge
        }

        public bool IsValid(Entity<TEntity> entity)
        {
            _threadAffinity.Validate();

            return IsValidNoAffinitCheck(entity);
        }

        public bool IsValidNoAffinitCheck(Entity<TEntity> entity)
        {
            int index = entity.Index;
            uint creationID = entity.CreationIDCheck;

            return (index + creationID != 0) && (creationID == entities[index].CreationIDCheck);
        }

        public EntityQuery<TEntity> Query()
        {
            return new EntityQuery<TEntity>(this);
        }

        #endregion

        #region Component Operation
        public ComponentID<TEntity, TComponent> RegisterComponentType<TComponent>() where TComponent : class
        {
            _threadAffinity.Validate();

            Assert.Debug(_componentCount < _maxComponentsPerEntity, "max component count exceeded");
            int component = _componentCount++;
            var id = new ComponentID<TEntity, TComponent>(component);

            return id;
        }

        public void AddComponent<TComponent>(Entity<TEntity> entity, ComponentID<TEntity, TComponent> id, TComponent component) where TComponent : class
        {
            _threadAffinity.Validate();

            Assert.Debug(IsValid(entity), $"Invalid entity: Index: {entity.Index} CreationID: {entity.CreationIDCheck}");
            Assert.Debug((ushort)id.ComponentIndex < _maxComponentsPerEntity, "Component index out of range");
            Assert.Debug(!HasCommponent(entity, id), $"Entity already has component {Utility.Reflection.GetType<TComponent>.kFullName}");

            int entityIndex = entity.Index;
            int componentIndex = id.ComponentIndex;
            int offset = entityIndex * _maxComponentsPerEntity + componentIndex;

            componentFlags[entityIndex][componentIndex] = true;
        }

        public TComponent GetComponent<TComponent>(Entity<TEntity> entity, ComponentID<TEntity, TComponent> id) where TComponent : class
        {
            _threadAffinity.Validate();

            return GetComponentNoAffinityCheck(entity, id);
        }

        public TComponent GetComponentNoAffinityCheck<TComponent>(Entity<TEntity> entity, ComponentID<TEntity, TComponent> id) where TComponent : class
        {
            Assert.Debug((ushort)id.ComponentIndex < _maxComponentsPerEntity, "Component index out of range");
            Assert.Debug(IsValidNoAffinitCheck(entity), $"Invalid entity: Index: {entity.Index} CreationID: {entity.CreationIDCheck}");

            int offset = entity.Index * _maxComponentsPerEntity + id.ComponentIndex;

            return (TComponent)_components[offset];
        }

        public ComponentIterator GetComponents(Entity<TEntity> entity)
        {
            _threadAffinity.Validate();
            Assert.Debug(IsValid(entity), $"Invalid entity: Index: {entity.Index} CreationID: {entity.CreationIDCheck}");

            int startingOffset = entity.Index * _maxComponentsPerEntity;
            return new ComponentIterator(componentFlags[entity.Index], _components, startingOffset);
        }

        public void RemoveComponent<TComponent>(Entity<TEntity> entity, ComponentID<TEntity, TComponent> id) where TComponent : class
        {
            _threadAffinity.Validate();
            Assert.Debug(IsValid(entity), $"Invalid entity: Index: {entity.Index} CreationID: {entity.CreationIDCheck}");

            Assert.Debug((ushort)id.ComponentIndex < _maxComponentsPerEntity, "Component index out of range");

            int entityIndex = entity.Index;
            int componentIndex = id.ComponentIndex;
            int offset = entityIndex * _maxComponentsPerEntity + componentIndex;

            object component = _components[offset];
            IReleasable releasableComponent = component as IReleasable;
            if (releasableComponent != null)
            {
                releasableComponent.Release();
            }

            _components[offset] = null;

            componentFlags[entityIndex][componentIndex] = false;

        }

        public bool HasCommponent<TComponent>(Entity<TEntity> entity, ComponentID<TEntity, TComponent> id) where TComponent : class
        {
            _threadAffinity.Validate();

            Assert.Debug((ushort)id.ComponentIndex < _maxComponentsPerEntity, "Component index out of range");

            int entityIndex = entity.Index;
            int componentIndex = id.ComponentIndex;

            return componentFlags[entityIndex][componentIndex];
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
