using System;
using System.Collections.Generic;


namespace BMBaseCore.Entities
{
    public sealed class EntitySystem<TEntity> : BaseObject where TEntity : EntityType
    {
        /// <summary>
        /// This singleton exists to enable typesafe EntityExtensions.
        /// </summary>
        internal static EntitySystem<TEntity> Instance;

        #region Life 
        public override void Init()
        {
            base.Init();
        }

        public override void Destroy()
        {
            base.Destroy();
        }
        #endregion
    }
}
