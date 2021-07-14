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
        private IndexPool _entityPool;

        internal Entity<TEntity>[] entities;        // "internal" for access from EntityQuery
        internal ComponentFlags[] componentFlags;   // "internal" for access from EntityQuery

        #endregion

        #region Life 
        public override void Init()
        {
            base.Init();
            Instance = this;
        }

        public override void Destroy()
        {
            base.Destroy();
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
