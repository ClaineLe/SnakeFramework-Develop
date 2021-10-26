using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using com.halo.framework.common;

namespace com.halo.framework
{
    namespace runtime
    {
        public class BundleManager : com.snake.framework.runtime.BaseManager
        {
            private Dictionary<string, BundleAsyncOperation> _bundleLoadedDic;
            private Dictionary<string, BundleAsyncOperation> _bundleLoadingDic;
            private List<BundleAsyncOperation> _waitReduceRefList;
            private List<BundleAsyncOperation> _tmpLoading2LoadedList;
            private Dictionary<string, string[]> _depensMapping;
            private Dictionary<string, string> bundleRealPathDict;
            private PatchManager _patchManager;
            protected override void onInitialization()
            {
                _bundleLoadedDic = new Dictionary<string, BundleAsyncOperation>(1024);
                _bundleLoadingDic = new Dictionary<string, BundleAsyncOperation>(1024);
                _waitReduceRefList = new List<BundleAsyncOperation>(64);
                _tmpLoading2LoadedList = new List<BundleAsyncOperation>(64);
                bundleRealPathDict = new Dictionary<string, string>(1024);

                this._patchManager = Singleton<AppFacade>.GetInstance().GetManager<PatchManager>();
            }

            protected override void onPreload()
            {
                base.onPreload();
                mFramework.mLifeCycle.mUpdateHandle.AddEventHandler(this.onTick);
            }
            public void SetDepensMapping(Dictionary<string, string[]> depensMapping)
            {
                _depensMapping = depensMapping;
            }

            private string OperationBundleLoadPath(string bundleName)
            {
                var result = bundleRealPathDict.TryGetValue(bundleName, out var runtimePath);
                if (result)
                {
                    return runtimePath;
                }
                runtimePath = Path.Combine(
                        _patchManager.IsExtInPatcher(bundleName) ? PathConst.PERSISTENT_DATA_PATH : PathConst.STREAMING_ASSET_PATH,
                        bundleName);
                bundleRealPathDict[bundleName] = runtimePath;
                return runtimePath;
            }

            public void AsyncLoadBundle(string bundleName, out BundleOperationCollection collection)
            {
                collection = new BundleOperationCollection();
                if (TryGetBundleOperationInCache(bundleName, out var mainBundleOperation) == false)
                {
                    mainBundleOperation = new BundleAsyncOperation(OperationBundleLoadPath(bundleName), bundleName);
                    mainBundleOperation.AddReferenceCount();
                    _bundleLoadingDic.Add(bundleName, mainBundleOperation);
                }
                collection.mMainBundleAsyncOperation = mainBundleOperation;
                if (_depensMapping.TryGetValue(bundleName, out string[] dependBundleName) == false)
                {
                    FrameworkLog.ErrorFormat("无法在依赖列表中找到bundlename为{0}的数据，不应该存在此情况", bundleName);
                    return;
                }
                var dependBundleOperations = new BundleAsyncOperation[dependBundleName.Length];
                for (var i = 0; i < dependBundleName.Length; i++)
                {
                    var singleDepend = dependBundleName[i];
                    if (TryGetBundleOperationInCache(singleDepend, out BundleAsyncOperation dependBundleOperation) == false)
                    {
                        dependBundleOperation = new BundleAsyncOperation(OperationBundleLoadPath(singleDepend), singleDepend);
                        dependBundleOperation.AddReferenceCount();
                        _bundleLoadingDic.Add(singleDepend, dependBundleOperation);
                    }
                    dependBundleOperations[i] = dependBundleOperation;
                }
                collection.mDependAsyncOperation = dependBundleOperations;
            }

            private bool TryGetBundleOperationInCache(string bundleName, out BundleAsyncOperation operation)
            {
                if (!_bundleLoadedDic.TryGetValue(bundleName, out operation) &&
                    !_bundleLoadingDic.TryGetValue(bundleName, out operation)) return false;
                operation.AddReferenceCount();
                return true;
            }

            public void ReleaseBundleCollection(BundleOperationCollection collection)
            {
                _waitReduceRefList.Add(collection.mMainBundleAsyncOperation);
                var count = collection.mDependAsyncOperation.Length;
                for (var i = 0; i < count; i++)
                {
                    _waitReduceRefList.Add(collection.mDependAsyncOperation[i]);
                }
            }

            protected void onTick(int frameCount, float time, float deltaTime, float unscaledTime, float realElapseSeconds)
            {
                var count = _waitReduceRefList.Count;
                if (count > 0)
                {
                    for (var i = 0; i < count; i++)
                    {
                        var waitReduceRefOperation = _waitReduceRefList[i];
                        var bundleName = waitReduceRefOperation.mBundleFullName;
                        waitReduceRefOperation.ReduceReferenceCount();
                        if (waitReduceRefOperation.mReferenceCount > 0) continue;
                        if (_bundleLoadedDic.ContainsKey(bundleName))
                        {
                            _bundleLoadedDic.Remove(waitReduceRefOperation.mBundleFullName);
                        }
                        else if (_bundleLoadingDic.ContainsKey(bundleName))
                        {
                            _bundleLoadingDic.Remove(waitReduceRefOperation.mBundleFullName);
                        }
                        else
                        {
                            throw new Exception($"卸载bundle过程中错误,要卸载的bundle不在缓存中,错误bundlename:{bundleName}");
                        }
                        waitReduceRefOperation.Release(true);
                    }
                    _waitReduceRefList.Clear();
                }
                foreach (var kv in _bundleLoadingDic)
                {
                    if (!kv.Value.GetIsDone()) continue;
                    _tmpLoading2LoadedList.Add(kv.Value);
                }
                count = _tmpLoading2LoadedList.Count;
                if (count > 0)
                {
                    for (var i = 0; i < count; i++)
                    {
                        var item = _tmpLoading2LoadedList[i];
                        _bundleLoadingDic.Remove(item.mBundleFullName);
                        _bundleLoadedDic.Add(item.mBundleFullName, item);
                    }
                    _tmpLoading2LoadedList.Clear();
                }
            }

        }
    }
}