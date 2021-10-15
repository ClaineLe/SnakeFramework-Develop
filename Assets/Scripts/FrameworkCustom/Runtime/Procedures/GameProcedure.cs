using com.snake.framework.runtime;
using UnityEngine;

namespace com.snake.framework 
{
    namespace custom.runtime 
    {

        public class GameProcedure : BaseProcedure
        {
            private TimerManager _timerMgr;
            int index = 0;
            int id;
            protected override void onEnter(ProcedureManager owner, IState<ProcedureManager> fromState, object userData)
            {
                Debug.LogError("onEnter");
                base.onEnter(owner, fromState, userData);
                _timerMgr = owner.mFramework.GetManager<TimerManager>();

                int scrCnt = Time.frameCount;
                float scrTime = Time.realtimeSinceStartup;
                id = _timerMgr.StartFramer(120, () => {
                    Debug.LogError("StartFramer-0 -> " + (Time.frameCount - scrCnt));
                });

                _timerMgr.StartTimer(2.2f, () => {
                    Debug.LogError("StartFramer-1 -> " + (Time.realtimeSinceStartup - scrTime));
                });
            }

            protected override void onTick(ProcedureManager owner, int frameCount, float time, float deltaTime, float unscaledTime, float realElapseSeconds)
            {
                if (++index > 20)
                    _timerMgr.CancelFramer(id);
            }
        }
    }
}