using com.snake.framework.runtime;

namespace com.snake.framework
{
    namespace custom.runtime
    {
        public class AppFacadeCostom : IAppFacadeCostom
        {
            private AppFacade _appFacade;
            public void Initialization()
            {
                _appFacade = Singleton<AppFacade>.GetInstance();
                this._regCostomManagers();
                this._regProcedures();
            }

            private void _regCostomManagers()
            {
                this._appFacade.RegiestManager<LuaManager>();
            }

            private void _regProcedures()
            {
                ProcedureManager procedureMgr = Singleton<AppFacade>.GetInstance().GetManager<ProcedureManager>();
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