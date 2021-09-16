using com.snake.framework.runtime;
using System.Collections.Generic;

namespace com.snake.framework
{
    namespace runtime
    {
        public class AppFacade : Singleton<AppFacade>, ISingleton
        {
            public LifeCycle mLifeCycle;
            private Dictionary<string, System.Tuple<System.Type, IManager>> _managerDic;

            protected override void onInitialize()
            {
                base.onInitialize();
                this._managerDic = new Dictionary<string, System.Tuple<System.Type, IManager>>();
                this.mLifeCycle = LifeCycle.Create();
            }

            public void StartUp(BootDriver bootDriver)
            {
                this._RegiestManager();
                bootDriver.mAppFacadeCostom.Initialization();
                bootDriver.mAppFacadeCostom.GameLaunch();
            }

            public void RegiestManager<T>(bool replace = false) where T : IManager
            {
                string typeName = typeof(T).Name;
                if (replace == false && _managerDic.ContainsKey(typeName) == true)
                {
                    throw new System.Exception("管理器已经存在.MgrName:" + typeName);
                }
                _managerDic[typeName] = new System.Tuple<System.Type, IManager>(typeof(T), null);
            }

            public T GetManager<T>()
                where T : class, IManager
            {
                return GetManager(typeof(T).Name) as T;
            }

            public IManager GetManager(string mgrName)
            {
                if (this._managerDic.TryGetValue(mgrName, out System.Tuple<System.Type, IManager>  managerKVP))
                {
                    return managerKVP.Item2;
                }
                return null;
            }

            private void _RegiestManager()
            {
                this.RegiestManager<AssetManager>();
                this.RegiestManager<ProcedureManager>();
                this.RegiestManager<NetworkManager>();
                this.RegiestManager<UIManager>();
            }

            public void BootUpManagers() 
            {

            }

            public float GetInitProgress() 
            {
                float count = this._managerDic.Count;
                float progress = 0.0f;
                float initCompletedCount = 0.0f;
                var enumerator = this._managerDic.GetEnumerator();
                while (enumerator.MoveNext())
                    progress += enumerator.Current.Value.Item2.mBootUpProgress;
                if (count == progress)
                    return 1.0f;
                return progress / count;
            }
        }
    }
}
