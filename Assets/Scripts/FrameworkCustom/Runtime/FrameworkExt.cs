using com.snake.framework.runtime;

namespace com.snake.framework
{
    namespace custom.runtime
    {
        public class SplashUserInterface : ISplashUserInterface
        {
            private bool _isDone = false;
            public bool mIsDone => _isDone;

            public void Start()
            {
                SnakeDebuger.Log("SplashUserInterface");
            }

            public void Tick(float progress)
            {
                _isDone = true;
            }
        }

        public class UpdateController : IUpdateController
        {
            private bool _isDone = false;
            public bool mIsDone => _isDone;

            public void Dispose()
            {
            }

            public void Enter()
            {
            }

            public void Tick()
            {
                _isDone = true;
            }
        }

        public class FrameworkExt : ISnakeFrameworkExt
        {
            public void Initialization()
            {
                SnakeFramework.Instance.SetupSplashUserInterface<SplashUserInterface>();
                SnakeFramework.Instance.SetupUpdateController<UpdateController>();

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
                procedureMgr.RegiestProcedure<GameProcedure>();
                /*
                 * 注册自定义流程
                 */
            }

            public void EnterGameContent()
            {
                SnakeDebuger.Log("EnterGameContent");
                ProcedureManager procedureMgr = SnakeFramework.Instance.GetManager<ProcedureManager>();
                procedureMgr.SwitchProcedure<GameProcedure>();
            }

        }
    }
}