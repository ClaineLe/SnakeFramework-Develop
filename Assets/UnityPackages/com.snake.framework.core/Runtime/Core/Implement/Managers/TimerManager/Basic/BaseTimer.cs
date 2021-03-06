namespace com.snake.framework
{
    namespace runtime
    {
        public abstract class BaseTimer : ITimer, IReference
        {
            public TimerManager mOwner { get; private set; }
            public int mId { get; protected set; }
            public bool mCompleted { get; private set; }
            public bool mCancel { get; private set; }

            protected int startFrameCount;
            protected float startTime;
            protected float startDeltaTime;
            protected float startUnscaledTime;
            protected float startRealElapseSeconds;

            private System.Action _onCompletedHandle;

            public BaseTimer() {
                this.mOwner = SnakeFramework.Instance.GetManager<TimerManager>();
            }

            protected abstract bool tickProcess(int frameCount, float time, float deltaTime, float unscaledTime, float realElapseSeconds);

            public void Init(int id, System.Action onCompletedHandle, int frameCount, float time, float deltaTime, float unscaledTime, float realElapseSeconds)
            {
                this.mId = id;
                this._onCompletedHandle = onCompletedHandle;
                this.startFrameCount = frameCount;
                this.startTime = time;
                this.startDeltaTime = deltaTime;
                this.startUnscaledTime = unscaledTime;
                this.startRealElapseSeconds = realElapseSeconds;
                this.mOwner.mFramework.mLifeCycle.mUpdateHandle.AddEventHandler(this.tick);
            }

            public virtual void OnReferenceClear()
            {
                this.mOwner.mFramework.mLifeCycle.mUpdateHandle.RemoveEventHandler(this.tick);
                this.mId = 0;
                this.mCompleted = false;
                this.mCancel = false;
                this._onCompletedHandle = null;
            }

            public void Cancel()
            {
                this.mCancel = true;
            }

            public void Begin() 
            {
            }

            private void tick(int frameCount, float time, float deltaTime, float unscaledTime, float realElapseSeconds)
            {
                if (mCompleted == true || mCancel == true)
                    return;
                if (tickProcess(frameCount, time, deltaTime, unscaledTime, realElapseSeconds))
                {
                    mCompleted = true;
                    _onCompletedHandle?.Invoke();
                }
            }
        }
    }
}