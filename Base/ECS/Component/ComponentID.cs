using System;

namespace BMBaseCore.ECS
{
    public struct ComponentID<TEntity, TComponent> : IEquatable<ComponentID<TEntity, TComponent>>
        where TEntity : EntityType
        where TComponent : class
    {
        public static readonly ComponentID<TEntity, TComponent> None = new ComponentID<TEntity, TComponent>(-1);

        internal readonly Int16 ComponentIndex;

        internal ComponentID(int componentIndex)
        {
            ComponentIndex = (Int16)componentIndex;
        }

        #region IEquatable and friends

        public bool Equals(ComponentID<TEntity, TComponent> other)
        {
            return (ComponentIndex == other.ComponentIndex);
        }

        public bool Equals<TOtherComponent>(ComponentID<TEntity, TOtherComponent> other) where TOtherComponent : class
        {
            return ComponentIndex == other.ComponentIndex;
        }

        public override bool Equals(object obj)
        {
            return (obj is ComponentID<TEntity, TComponent>) && Equals((ComponentID<TEntity, TComponent>)obj);
        }

        public static bool operator ==(ComponentID<TEntity, TComponent> a, ComponentID<TEntity, TComponent> b)
        {
            return a.ComponentIndex == b.ComponentIndex;
        }

        public static bool operator !=(ComponentID<TEntity, TComponent> a, ComponentID<TEntity, TComponent> b)
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
