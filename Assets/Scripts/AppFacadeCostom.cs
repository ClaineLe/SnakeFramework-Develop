namespace com.snake.framework
{
    namespace runtime
    {
        using com.snake.framework.custom.manager;

        public class AppFacadeCostom : IAppFacadeCostom
        {
            private AppFacade _appFacade;
            public void Initialization()
            {
                _appFacade = Singleton<AppFacade>.GetInstance();
                this._regCostomManagers();
                this._regProcedures();
            }

            public void GameLaunch()
            {
                Singleton<AppFacade>.GetInstance().GetManager<ProcedureManager>().SwitchProcedure<SplashProcedure>();
            }

            private void _regCostomManagers()
            {
                this._appFacade.RegiestManager<NetworkManager>();
                this._appFacade.RegiestManager<ProcedureManager>();
                this._appFacade.RegiestManager<UIManager>();
            }

            private void _regProcedures()
            {
                ProcedureManager procedureMgr = Singleton<AppFacade>.GetInstance().GetManager<ProcedureManager>();
                procedureMgr.RegiestProcedure<SplashProcedure>();
            }


        }
    }
}