namespace com.snake.framework
{
    namespace runtime
    {
        public class ProcedureManager : BaseManager, IFiniteStateMachineOwner
        {
            private IFiniteStateMachine<ProcedureManager> _procedureFsm;

            public override void Initialization()
            {
                this._procedureFsm = new FiniteStateMachine<ProcedureManager>(this);
                LifeCycle.mUpdateHandle.AddEventHandler(this._procedureFsm.Tick);
            }


            public T RegiestProcedure<T>()
                where T : BaseProcedure, new()
            {
                T procedure = this._procedureFsm.AddState<T>();
                procedure.Init(this);
                return procedure;
            }

            public bool SwitchProcedure<T>(object userData = null)
                where T : BaseProcedure
            {
                return SwitchProcedure(typeof(T).Name, userData);
            }

            public bool SwitchProcedure(string procedureName, object userData = null)
            {
                if (this._procedureFsm.HasState(procedureName) == false)
                {
                    SnakeLog.ErrorFormat("切换流程失败。不存在流程:{0}", procedureName);
                    return false;
                }
                return this._procedureFsm.Switch(procedureName, userData);
            }
            public bool CanSwitch()
            {
                return this._procedureFsm.CanSwitch();
            }

        }
    }
}
