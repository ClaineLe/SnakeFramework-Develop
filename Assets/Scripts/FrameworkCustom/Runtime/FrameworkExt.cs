using com.snake.framework.runtime;

namespace com.snake.framework
{
    namespace custom.runtime
    {
        public class SplashUserInterface : ISplashUserInterface
        {
            public string Name => throw new System.NotImplementedException();

            public bool mIsDone => throw new System.NotImplementedException();

            public float mProgress => throw new System.NotImplementedException();

            public string mError => throw new System.NotImplementedException();

            public void Dispose()
            {
                throw new System.NotImplementedException();
            }

            public void Enter(object userData = null)
            {
                throw new System.NotImplementedException();
            }

            public void Exit()
            {
                throw new System.NotImplementedException();
            }

            public void Init()
            {
                throw new System.NotImplementedException();
            }

            public void Tick(int frameCount, float time, float deltaTime, float unscaledTime, float realElapseSeconds)
            {
                throw new System.NotImplementedException();
            }
        }

        public class UpdateController : IUpdateController
        {
            public string Name => throw new System.NotImplementedException();

            public bool mIsDone => throw new System.NotImplementedException();

            public float mProgress => throw new System.NotImplementedException();

            public string mError => throw new System.NotImplementedException();

            public void Dispose()
            {
                throw new System.NotImplementedException();
            }

            public void Enter(object userData = null)
            {
                throw new System.NotImplementedException();
            }

            public void Exit()
            {
                throw new System.NotImplementedException();
            }

            public void Init()
            {
                throw new System.NotImplementedException();
            }

            public void Tick(int frameCount, float time, float deltaTime, float unscaledTime, float realElapseSeconds)
            {
                throw new System.NotImplementedException();
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