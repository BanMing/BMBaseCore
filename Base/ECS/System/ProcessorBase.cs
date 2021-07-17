/******************************************************************
** Processor.cs
** @Author       : BanMing 
** @Date         : 7/17/2021 6:05:48 PM
** @Description  : 
*******************************************************************/

using System;
using System.Collections.Generic;

namespace BMBaseCore.ECS
{
    /// <summary>
    /// A processor is a sequence of one or more methods that game loop executes on the entities in EntityContext
    /// </summary>
    public abstract class ProcessorBase<TEntity> : BaseObject where TEntity : EntityType
    {
        protected EntityProcess<TEntity> _entityProcess;

        protected ProcessorBase(EntityProcess<TEntity> entityProcess)
        {
            _entityProcess = entityProcess;
        }

        public override void Initialize()
        {
            OnInitialize(_entityProcess);

            base.Initialize();
        }

        public override void Destroy()
        {
            OnDestroy(_entityProcess);
            base.Destroy();
        }

        // Bind event and if needed grab entity maker
        protected abstract void OnInitialize(EntityProcess<TEntity> entityProcess);

        // Clear event listener, if need
        protected abstract void OnDestroy(EntityProcess<TEntity> entityProcess);

        public void Process(EntityProcess<TEntity> entityProcess)
        {
            DoProcess(entityProcess);
        }

        protected abstract void DoProcess(EntityProcess<TEntity> entityProcess);

    }
}
