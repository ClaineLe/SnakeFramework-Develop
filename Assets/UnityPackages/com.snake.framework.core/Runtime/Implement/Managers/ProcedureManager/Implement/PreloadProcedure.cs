using com.snake.framework.runtime;

namespace com.snake.framework
{
    /// <summary>
    /// Ԥ��������
    /// </summary>
    internal class PreloadProcedure : BaseProcedure
    {
        protected override void onTick(ProcedureManager owner, int frameCount, float time, float deltaTime, float unscaledTime, float realElapseSeconds)
        {
            Singleton<AppFacade>.GetInstance().EnterGameContent();
        }
    }
}
