/******************************************************************
** EntitySystem.cs
** @Author       : BanMing 
** @Date         : 7/17/2021 3:47:35 PM
** @Description  : 
*******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMBaseCore.ECS
{
    public abstract class EntityProcess<TEntity> : BaseObject where TEntity : EntityType
    {
        // Used for validating code execution on the correct thread when EntitySystem is running.
        private readonly Threading.ThreadAffinity _threadAffinity;

        private EntityContext<TEntity> _entityContext = null;
        //private List<>

        #region Life
        public EntityProcess()
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Destroy()
        {
            base.Destroy();
        }

        #endregion
    }
}
