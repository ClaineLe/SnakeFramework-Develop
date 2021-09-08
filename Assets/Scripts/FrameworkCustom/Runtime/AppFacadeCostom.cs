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
                procedureMgr.RegiestProcedure<SplashProcedure>();
            }

            public void GameLaunch()
            {
                Singleton<AppFacade>.GetInstance().GetManager<ProcedureManager>().SwitchProcedure<SplashProcedure>();
            }
        }
    }
}