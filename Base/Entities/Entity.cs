using System;
using System.Collections.Generic;

namespace BMBaseCore.Entities
{
    public struct Entity<TEntity> : IComparable<Entity<TEntity>>, IEquatable<Entity<TEntity>> where TEntity : EntityType
    {

        private class EntityComparer : IEqualityComparer<Entity<TEntity>>
        {
            public bool Equals(Entity<TEntity> x, Entity<TEntity> y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(Entity<TEntity> obj)
            {
                return obj.GetHashCode();
            }
        }

        public static readonly Entity<TEntity> None = default(Entity<TEntity>);
        public static readonly IEqualityComparer<Entity<TEntity>> kDefaultComparer = new EntityComparer();

        /// <summary>
        /// Must match StructPool`s PoolIndex in the entity system`s entity descriptor table.
        /// </summary>
        internal readonly Int16 Index;

        /// <summary>
        /// Holds enough of the CreationId for validation checks
        /// </summary>
        internal readonly UInt16 CreationIdCheck;

        internal Entity(Int16 index, UInt16 creationId)
        {
            Index = index;
            CreationIdCheck = creationId;
        }

        public int ToReference()
        {
            int entityReference = (Index << 16) | CreationIdCheck;
            return entityReference;
        }

        public static Entity<TEntity> FromReference(int entityReference)
        {
            short index = (short)(entityReference >> 16);
            ushort creationIdCheck = (ushort)(entityReference & 0xffff);
            return new Entity<TEntity>(index, creationIdCheck);
        }

        public override string ToString()
        {
            if (this == None)
            {
                return "None";
            }

            string entityString = $"[Index={Index},CreationId={CreationIdCheck}]";
            
            return entityString;
        }

        #region Equality and related boilerplate

        public int CompareTo(Entity<TEntity> other)
        {
            return Index.CompareTo(other.Index);
        }

        public bool Equals(Entity<TEntity> other)
        {
            return (Index == other.Index) && (CreationIdCheck == other.CreationIdCheck);
        }

        public override bool Equals(object obj)
        {
            return (obj is Entity<TEntity>) && Equals((Entity<TEntity>)obj);
        }

        public static bool operator ==(Entity<TEntity> a, Entity<TEntity> b)
        {
            return (a.Index == b.Index) && (a.CreationIdCheck == b.CreationIdCheck);
        }

        public static bool operator !=(Entity<TEntity> a, Entity<TEntity> b)
        {
            return (a.Index != b.Index) || (a.CreationIdCheck != b.CreationIdCheck);
        }

        public override int GetHashCode()
        {
            return (Index << 16) | CreationIdCheck;
        }

        #endregion
    }
}
