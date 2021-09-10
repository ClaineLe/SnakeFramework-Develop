namespace com.snake.framework
{
    namespace runtime
    {
        public abstract class BaseManager : IManager
        {
            private AppFacade _appFacade;
            public AppFacade mAppFacade 
            {
                get 
                {
                    if (_appFacade == null)
                        _appFacade = Singleton<AppFacade>.GetInstance();
                    return _appFacade;
                }
            }
            public virtual string mName => this.GetType().Name;

            public float mPreloadProgress{ get; protected set; } = 0.0f;
            public virtual void Initialization()
            {

            }
        }
    }
}