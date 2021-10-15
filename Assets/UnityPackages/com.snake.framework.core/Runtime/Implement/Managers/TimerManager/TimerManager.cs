﻿using System.Collections.Generic;

namespace com.snake.framework
{
    namespace runtime
    {
        public class TimerManager : BaseManager
        {
            private int _timerIndex = int.MinValue;
            
            private List<BaseTimer> _timerList = new List<BaseTimer>();

            protected override void onInitialization()
            {
                this._timerList = new List<BaseTimer>();
            }
            protected override void onPreload()
            {
                base.onPreload();
                mFramework.mLifeCycle.mUpdateHandle.AddEventHandler(this.onTick);
            }
            public int StartTimer(float durationTime, System.Action onCompletedHandle, bool unscaledTime = false)
            {
                DurationTimer timer = unscaledTime ? ReferencePool.Take<UnscaleTimer>() : ReferencePool.Take<DurationTimer>();
                timer.Init(++_timerIndex, onCompletedHandle,
                   UnityEngine.Time.frameCount,
                   UnityEngine.Time.time,
                   UnityEngine.Time.deltaTime,
                   UnityEngine.Time.unscaledTime,
                   UnityEngine.Time.realtimeSinceStartup);
                timer.SetEndDuration(durationTime);
                this._timerList.Add(timer);
                return timer.mId;
            }

            public int StartFramer(int frameCount, System.Action onCompletedHandle)
            {
                FrameTimer timer = ReferencePool.Take<FrameTimer>();
                timer.Init(++_timerIndex, onCompletedHandle,
                   UnityEngine.Time.frameCount,
                   UnityEngine.Time.time,
                   UnityEngine.Time.deltaTime,
                   UnityEngine.Time.unscaledTime,
                   UnityEngine.Time.realtimeSinceStartup);
                timer.SetEndFrameNum(frameCount);
                this._timerList.Add(timer);
                return timer.mId;
            }

            public bool CancelFramer(int timerId)
            {
                int index = _timerList.FindIndex(a => a.mId == timerId);
                if (index < 0)
                    return false;
                _timerList[index].Cancel();
                return true;
            }

            protected void onTick(int frameCount, float time, float deltaTime, float unscaledTime, float realElapseSeconds)
            {
                for (int i = 0; i < this._timerList.Count; i++)
                {
                    BaseTimer timer = this._timerList[i];
                    timer.Tick(frameCount, time, deltaTime, unscaledTime, realElapseSeconds);
                    if (timer.mCompleted == true || timer.mCancel == true)
                    {
                        this._timerList.RemoveAt(i--);
                        ReferencePool.Return(timer);
                    }
                }
            }
        }
    }
}