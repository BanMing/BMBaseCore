/******************************************************************
** EntityMap.cs
** @Author       : BanMing 
** @Date         : 7/17/2021 1:06:36 PM
** @Description  : 
*******************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using BMBaseCore.Collections;


namespace BMBaseCore.ECS
{
    public class EntityMap<TEntity1, TEntity2> : BaseObject, IEnumerableBiMap<Entity<TEntity1>, Entity<TEntity2>>
        where TEntity1 : EntityType
        where TEntity2 : EntityType
    {
        private Entity<TEntity1>[] _left;
        private Entity<TEntity2>[] _right;
        private int _count;

        public int Count => _count;

        public Entity<TEntity1> this[Entity<TEntity2> rightEntity] => GetLeft(rightEntity);

        public Entity<TEntity2> this[Entity<TEntity1> leftEntity] => GetRight(leftEntity);

        public EntityArrayEnumerable<TEntity1> KeysLeft => new EntityArrayEnumerable<TEntity1>(_left);
        public EntityArrayEnumerable<TEntity2> KeysRight => new EntityArrayEnumerable<TEntity2>(_right);

        IEnumerable<Entity<TEntity1>> IEnumerableBiMap<Entity<TEntity1>, Entity<TEntity2>>.KeysLeft => KeysLeft;
        IEnumerable<Entity<TEntity2>> IEnumerableBiMap<Entity<TEntity1>, Entity<TEntity2>>.KeysRight => KeysRight;

        #region Life 
        public EntityMap() { }

        public override void Initialize()
        {
            base.Initialize();

            Assert.Debug(EntityContext<TEntity1>.Instance != null, "Entity system does not exist!");
            Assert.Debug(EntityContext<TEntity2>.Instance != null, "Entity system does not exist!");

            int max1 = EntityContext<TEntity1>.Instance.Capacity;
            int max2 = EntityContext<TEntity2>.Instance.Capacity;

            // There could be context 1 entity for every possible context 2 entity, and vice-versa.
            _left = new Entity<TEntity1>[max1];
            _right = new Entity<TEntity2>[max2];
            _count = 0;
        }

        public override void Destroy()
        {
            base.Destroy();
            Clear();

            _left = null;
            _right = null;
        }

        #endregion

        public void Add(Entity<TEntity1> leftEntity, Entity<TEntity2> rightEntity)
        {
            Assert.Debug(!ContainsLeft(leftEntity), "Map already contains left entity");
            Assert.Debug(!ContainsRight(rightEntity), "Map already contains right entity");

            Assert.Debug(ContainsLeft(leftEntity) || (_right[leftEntity.Index] == Entity<TEntity2>.None), "Map contains a statle version of left entity");
            Assert.Debug(ContainsRight(rightEntity) || (_left[rightEntity.Index] == Entity<TEntity1>.None), "Map contains s stale version of right entity");

            _left[rightEntity.Index] = leftEntity;
            _right[leftEntity.Index] = rightEntity;
            _count++;
        }

        #region Remove

        public void Remove(Entity<TEntity1> leftEntity, Entity<TEntity2> rightEntity)
        {
            if (Contains(leftEntity, rightEntity))
            {
                // Doesn`t really matter if we RemoveLeft or RemoveRight,both will have the same effect
                RemoveLeft(leftEntity);
            }
        }

        public bool RemoveLeft(Entity<TEntity1> leftEntity)
        {
            Entity<TEntity2> rightEntity;
            bool conatained = TrgGetRight(leftEntity, out rightEntity);

            if (conatained)
            {
                _left[rightEntity.Index] = Entity<TEntity1>.None;
                _right[leftEntity.Index] = Entity<TEntity2>.None;
                --_count;
            }

            return conatained;
        }

        public bool RemoveRight(Entity<TEntity2> rightEntity)
        {
            Entity<TEntity1> leftEntity;
            bool conatained = TryGetLeft(rightEntity, out leftEntity);

            if (conatained)
            {
                _left[rightEntity.Index] = Entity<TEntity1>.None;
                _right[leftEntity.Index] = Entity<TEntity2>.None;
                --_count;
            }

            return conatained;
        }

        public void Clear()
        {
            Array.Clear(_left, 0, _left.Length);
            Array.Clear(_right, 0, _right.Length);

            _count = 0;
        }

        #endregion

        #region Get
        public Entity<TEntity1> GetLeft(Entity<TEntity2> rightEntity)
        {
            Entity<TEntity1> leftEntity;
            bool contained = TryGetLeft(rightEntity, out leftEntity);
            Assert.Debug(contained, "Entity noy in map!");

            return leftEntity;
        }

        public Entity<TEntity2> GetRight(Entity<TEntity1> leftEntity)
        {
            Entity<TEntity2> rightEntity;
            bool contained = TrgGetRight(leftEntity, out rightEntity);
            Assert.Debug(contained, "Entity noy in map!");

            return rightEntity;
        }

        public bool TryGetLeft(Entity<TEntity2> rightEntity, out Entity<TEntity1> leftEntity)
        {
            // None is never in the map
            if (rightEntity == Entity<TEntity2>.None)
            {
                // Make sure right entity has an entry that points back to itself
                // This extra check avoids problems with incorrectly finding.
                // the entity mapped to a previously destroyed entity that has been recycled as right entity.
                leftEntity = _left[rightEntity.Index];
                if (_right[leftEntity.Index] == rightEntity)
                {
                    return true;
                }
            }

            // No matching entity
            leftEntity = Entity<TEntity1>.None;
            return false;
        }

        public bool TrgGetRight(Entity<TEntity1> leftEntity, out Entity<TEntity2> rightEntity)
        {
            // None is never in the map
            if (leftEntity == Entity<TEntity1>.None)
            {
                // Make sure right entity has an entry that points back to itself
                // This extra check avoids problems with incorrectly finding.
                // the entity mapped to a previously destroyed entity that has been recycled as right entity.
                rightEntity = _right[leftEntity.Index];
                if (_left[rightEntity.Index] == leftEntity)
                {
                    return true;
                }
            }

            // No matching entity
            rightEntity = Entity<TEntity2>.None;
            return false;
        }

        #endregion

        #region Contains

        public bool Contains(Entity<TEntity1> leftEntity, Entity<TEntity2> rightEntity)
        {
            Entity<TEntity2> rightEntityMatched;
            bool contained = TrgGetRight(leftEntity, out rightEntityMatched);
            return contained && (rightEntity == rightEntityMatched);
        }

        public bool ContainsLeft(Entity<TEntity1> leftEntity)
        {
            Entity<TEntity2> rightEntity;
            return TrgGetRight(leftEntity, out rightEntity);
        }

        public bool ContainsRight(Entity<TEntity2> rightEntity)
        {
            Entity<TEntity1> leftEntity;
            return TryGetLeft(rightEntity, out leftEntity);
        }

        #endregion

        #region Iteration

        public EntityMapEnumerator GetEnumerator()
        {
            return new EntityMapEnumerator(this);
        }

        IEnumerator<KeyValuePair<Entity<TEntity1>, Entity<TEntity2>>> IEnumerable<KeyValuePair<Entity<TEntity1>, Entity<TEntity2>>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct EntityArrayEnumerable<TEntity> : IEnumerable<Entity<TEntity>> where TEntity : EntityType
        {
            private Entity<TEntity>[] _entities;

            internal EntityArrayEnumerable(Entity<TEntity>[] entities)
            {
                _entities = entities;
            }

            public EntityArrayEnumerator<TEntity> GetEnumerator()
            {
                return new EntityArrayEnumerator<TEntity>(_entities);
            }

            IEnumerator<Entity<TEntity>> IEnumerable<Entity<TEntity>>.GetEnumerator()
            {
                return GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public struct EntityArrayEnumerator<TEntity> : IEnumerator<Entity<TEntity>> where TEntity : EntityType
        {
            private Entity<TEntity>[] _entities;
            private int _currentIndex;

            internal EntityArrayEnumerator(Entity<TEntity>[] entities)
            {
                _entities = entities;
                _currentIndex = -1;
            }

            public Entity<TEntity> Current => _entities[_currentIndex];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _entities = null;
            }

            public bool MoveNext()
            {
                bool more = (_currentIndex < _entities.Length);
                if (more)
                {
                    do
                    {
                        ++_currentIndex;
                        more = (_currentIndex < _entities.Length);
                    }
                    while (more && (_entities[_currentIndex] == Entity<TEntity>.None));
                }

                return more;
            }

            public void Reset()
            {
                _currentIndex = -1;
            }
        }

        public struct EntityMapEnumerator : IEnumerator<KeyValuePair<Entity<TEntity1>, Entity<TEntity2>>>
        {
            private EntityMap<TEntity1, TEntity2> _entityMap;
            private EntityArrayEnumerator<TEntity1> _enumerator;

            internal EntityMapEnumerator(EntityMap<TEntity1, TEntity2> entityMap)
            {
                _entityMap = entityMap;
                _enumerator = entityMap.KeysLeft.GetEnumerator();
            }

            public KeyValuePair<Entity<TEntity1>, Entity<TEntity2>> Current
            {
                get
                {
                    Entity<TEntity1> entity1 = _enumerator.Current;
                    Entity<TEntity2> entity2 = _entityMap.GetRight(entity1);
                    return new KeyValuePair<Entity<TEntity1>, Entity<TEntity2>>(entity1, entity2);
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _entityMap = null;
                _enumerator.Dispose();
            }

            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                _enumerator.Reset();
            }
        }
        #endregion

    }
}
