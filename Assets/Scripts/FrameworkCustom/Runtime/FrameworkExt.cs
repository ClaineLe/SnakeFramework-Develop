﻿using com.snake.framework.runtime;

namespace com.snake.framework
{
    namespace custom.runtime
    {
        public class FrameworkExt : ISnakeFrameworkExt
        {
            public void Initialization()
            {
                this._regCostomManagers();
                this._regProcedures();
            }

            private void _regCostomManagers()
            {
                SnakeFramework.Instance.RegiestManager<LuaManager>();
            }

            private void _regProcedures()
            {
                ProcedureManager procedureMgr = SnakeFramework.Instance.GetManager<ProcedureManager>();
                /*
                 * 注册自定义流程
                 */
            }

            public void EnterGameContent()
            {
                Debuger.Log("EnterGameContent");
                //Singleton<AppFacade>.GetInstance().GetManager<ProcedureManager>().SwitchProcedure<SplashProcedure>();
            }
        }
    }
}