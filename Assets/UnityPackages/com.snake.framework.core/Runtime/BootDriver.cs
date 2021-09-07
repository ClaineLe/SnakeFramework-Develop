using UnityEngine;

namespace com.snake.framework
{
    namespace runtime
    {
        public class BootDriver : MonoBehaviour
        {
            public IAppFacadeCostom mAppFacadeCostom { get; private set; }
            private void Awake()
            {
                mAppFacadeCostom = new AppFacadeCostom();
            }

            private void Start()
            {
                Singleton<AppFacade>.GetInstance().StartUp(this);
            }
        }
    }
}