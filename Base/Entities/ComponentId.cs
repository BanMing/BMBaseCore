using System;

namespace BMBaseCore.Entities
{
    public struct ComponentId<TEntity, TComponent> : IEquatable<ComponentId<TEntity, TComponent>>
        where TEntity : EntityType
        where TComponent : class
    {
        public static readonly ComponentId<TEntity, TComponent> None = new ComponentId<TEntity, TComponent>(-1);

        internal readonly Int16 ComponentIndex;

        internal ComponentId(int componentIndex)
        {
            ComponentIndex = (Int16)componentIndex;
        }

        #region IEquatable and friends

        public bool Equals(ComponentId<TEntity, TComponent> other)
        {
            return (ComponentIndex == other.ComponentIndex);
        }

        public bool Equals<TOtherComponent>(ComponentId<TEntity, TOtherComponent> other) where TOtherComponent : class
        {
            return ComponentIndex == other.ComponentIndex;
        }

        public override bool Equals(object obj)
        {
            return (obj is ComponentId<TEntity, TComponent>) && Equals((ComponentId<TEntity, TComponent>)obj);
        }

        public static bool operator ==(ComponentId<TEntity, TComponent> a, ComponentId<TEntity, TComponent> b)
        {
            return a.ComponentIndex == b.ComponentIndex;
        }

        public static bool operator !=(ComponentId<TEntity, TComponent> a, ComponentId<TEntity, TComponent> b)
        {
            return a.ComponentIndex != b.ComponentIndex;
        }

        public override int GetHashCode()
        {
            return ComponentIndex;
        }

        #endregion
        
        public override string ToString()
        {
            return $"[Index={ComponentIndex}]";
        }
    }
}
