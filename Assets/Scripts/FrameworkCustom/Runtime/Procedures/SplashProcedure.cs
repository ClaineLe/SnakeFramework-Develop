using com.snake.framework.runtime;

namespace com.snake.framework
{
    /// <summary>
    /// ÉÁÆÁÁ÷³Ì
    /// </summary>
    public class SplashProcedure : BaseProcedure
    {
        protected override void onEnter(ProcedureManager owner, IState<ProcedureManager> fromState, object userData)
        {
            base.onEnter(owner, fromState, userData);
            Debuger.Info("SplashProcedure.onEnter");

        }
        protected override void onTick(ProcedureManager owner, int frameCount, float time, float deltaTime, float unscaledTime, float realElapseSeconds)
        {
            //float initProgress = owner.mAppFacade.GetInitProgress();
            //SnakeLog.Info("SplashProcedure.onTick:" + initProgress);
            //if (initProgress >= 1.0f)
            //    Singleton<AppFacade>.GetInstance().GetManager<ProcedureManager>().SwitchProcedure<PreloadProcedure>();
        }

        protected override void onExit(ProcedureManager owner, IState<ProcedureManager> toState)
        {
            base.onExit(owner, toState);
            Debuger.Info("SplashProcedure.onExit");
        }
    }
}
