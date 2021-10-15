using com.snake.framework.runtime;

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
                procedureMgr.RegiestProcedure<GameProcedure>();
                /*
                 * 注册自定义流程
                 */
            }

            public void EnterGameContent()
            {
                ProcedureManager procedureMgr = SnakeFramework.Instance.GetManager<ProcedureManager>();
                procedureMgr.SwitchProcedure<GameProcedure>();
            }
        }
    }
}