using BMBaseCore.Collections;

namespace BMBaseCore.ECS
{
    public static class EntityExtensions
    {
        public static bool IsValid<TEntity>(this Entity<TEntity> entity) where TEntity : EntityType
        {
            return (EntityContext<TEntity>.Instance != null) && (EntityContext<TEntity>.Instance.IsValid(entity));
        }

        public static TComponent GetComponent<TEntity, TComponent>(this Entity<TEntity> entity, ComponentID<TEntity, TComponent> componentID)
            where TEntity : EntityType
            where TComponent : class
        {
            return EntityContext<TEntity>.Instance.GetComponent(entity, componentID);
        }

        public static ComponentIterator GetComponents<TEntity>(this Entity<TEntity> entity) where TEntity : EntityType
        {
            return EntityContext<TEntity>.Instance.GetComponents(entity);
        }

        public static void AddComponent<TEntity, TComponent>(this Entity<TEntity> entity, ComponentID<TEntity, TComponent> componentID, TComponent component)
            where TEntity : EntityType
            where TComponent : class
        {
            EntityContext<TEntity>.Instance.AddComponent(entity, componentID, component);
        }

        public static void RemoveComponent<TEnity, TComponent>(this Entity<TEnity> entity, ComponentID<TEnity, TComponent> componentID)
            where TEnity : EntityType
            where TComponent : class
        {
            EntityContext<TEnity>.Instance.RemoveComponent(entity, componentID);
        }

        public static void HasComponent<TEntity, TComponent>(this Entity<TEntity> entity, ComponentID<TEntity, TComponent> componentID)
            where TEntity : EntityType
            where TComponent : class
        {
            EntityContext<TEntity>.Instance.HasCommponent(entity,componentID);
        }
    }
}
