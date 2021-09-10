using com.snake.framework.runtime;

namespace com.snake.framework
{
    /// <summary>
    /// ÉÁÆÁÁ÷³Ì
    /// </summary>
    public class PlayingProcedure : BaseProcedure
    {
        protected override void onEnter(ProcedureManager owner, IState<ProcedureManager> fromState, object userData)
        {
            base.onEnter(owner, fromState, userData);
            SnakeLog.Info("PlayingProcedure.onEnter");

        }
        protected override void onTick(ProcedureManager owner, int frameCount, float time, float deltaTime, float unscaledTime, float realElapseSeconds)
        {
            SnakeLog.Info("PlayingProcedure.onTick:");
        }

        protected override void onExit(ProcedureManager owner, IState<ProcedureManager> toState)
        {
            base.onExit(owner, toState);
            SnakeLog.Info("PlayingProcedure.onExit");
        }
    }
}
