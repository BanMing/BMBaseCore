/******************************************************************
** ComponentBase.cs
** @Author       : BanMing 
** @Date         : 7/17/2021 6:31:13 PM
** @Description  : 
*******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMBaseCore.ECS
{
    public abstract class ComponentBase<TEntity, TComponent>
        where TEntity : EntityType
        where TComponent : ComponentBase<TEntity, TComponent>
    {
        public static ComponentID<TEntity, TComponent> ComponentID { get { return _componentID; } }
        private static ComponentID<TEntity, TComponent> _componentID = ComponentID<TEntity, TComponent>.None;

        protected static void InitializeComponentID(EntityContext<TEntity> entityContext)
        {
            Assert.Debug(_componentID == ComponentID<TEntity, TComponent>.None, $"Component type {Utility.Reflection.GetType<TComponent>.kFullName}");
            _componentID = entityContext.RegisterComponentType<TComponent>();
        }

        protected static void ShutdownComponentID() {

            Assert.Debug(_componentID != ComponentID<TEntity, TComponent>.None,$"Non-initialized component type {Utility.Reflection.GetType<TComponent>.kFullName} is being shutdown!");
            _componentID = ComponentID<TEntity, TComponent>.None;
        }
    }
}
