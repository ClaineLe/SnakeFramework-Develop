using com.snake.framework.runtime;

namespace com.snake.framework
{
    /// <summary>
    /// ÉÁÆÁÁ÷³Ì
    /// </summary>
    public class PreloadProcedure : BaseProcedure
    {
        protected override void onEnter(ProcedureManager owner, IState<ProcedureManager> fromState, object userData)
        {
            base.onEnter(owner, fromState, userData);
            Debuger.Info("PreloadProcedure.onEnter");
        }

        protected override void onTick(ProcedureManager owner, int frameCount, float time, float deltaTime, float unscaledTime, float realElapseSeconds)
        {
            Debuger.Info("PreloadProcedure.onTick:" + frameCount);
            Singleton<AppFacade>.GetInstance().GetManager<ProcedureManager>().SwitchProcedure<PlayingProcedure>();
        }

        protected override void onExit(ProcedureManager owner, IState<ProcedureManager> toState)
        {
            base.onExit(owner, toState);
            Debuger.Info("PreloadProcedure.onExit");
        }
    }
}
