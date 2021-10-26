using System;
using System.Collections.Generic;
using com.halo.framework.common;
using JetBrains.Annotations;
using UnityEngine;

namespace com.halo.framework
{
    namespace runtime
    {

        /// <summary>
        /// 图集管理器
        /// </summary>
        public class SpriteManager : com.snake.framework.runtime.BaseManager
        {
            /// <summary>
            /// 图集结构
            /// </summary>
            private class AtlasData
            {
                public SubAssetAsyncOperation operation;
                public string atlasName;
                public Dictionary<string, Sprite> spriteDict;
                public int refCount;
                public Queue<Action> waitCallbackQueue;
            }

            /// <summary>
            /// 用于存放图集的字典
            /// </summary>
            private Dictionary<string, AtlasData> _atlasDataDict;

            /// <summary>
            /// 资源管理器
            /// </summary>
            private AssetManager _assetManager;

            /// <summary>
            /// 初始化
            /// </summary>
            protected override void onInitialization()
            {
                _atlasDataDict = new Dictionary<string, AtlasData>(64);
                _assetManager = Singleton<AppFacade>.GetInstance().GetManager<AssetManager>();
            }

            /// <summary>
            /// 加载sprite
            /// </summary>
            /// <param name="atlasName"></param>
            /// <param name="spriteName"></param>
            /// <param name="callback"></param>
            public void LoadSprite(string atlasName, string spriteName, Action<Sprite> callback)
            {
                if (callback == null)
                {
                    FrameworkLog.ErrorFormat("加载sprite时没有设置回调\n图集路径:{0}\nsprite名字:{1}", atlasName, spriteName);
                    return;
                }
                var atlasResult = _atlasDataDict.TryGetValue(atlasName, out var atlasData);

                if (atlasResult)
                {
                    ++atlasData.refCount;
                    if (atlasData.spriteDict == null)
                    {
                        //说明未加载完成
                        if (atlasData.waitCallbackQueue == null)
                        {
                            atlasData.waitCallbackQueue = new Queue<Action>();
                        }
                        atlasData.waitCallbackQueue.Enqueue(() =>
                        {
                            FindSpriteFromAtlasData(atlasData, spriteName, callback);
                        });
                        return;
                    }
                    FindSpriteFromAtlasData(atlasData, spriteName, callback);
                    return;
                }
                atlasData = new AtlasData();
                ++atlasData.refCount;
                atlasData.atlasName = atlasName;
                _atlasDataDict.Add(atlasName, atlasData);
                void OnCompleted(IAssetAsyncOperation o)
                {
                    var op = (SubAssetAsyncOperation)o;
                    if (op.mResultArray == null)
                    {
                        FrameworkLog.ErrorFormat("加载图集错误\n图集路径:{0}", atlasData.atlasName);
                        return;
                    }
                    if (op.mResultArray.Length == 0)
                    {
                        FrameworkLog.ErrorFormat("加载图集错误,图集中sprite个数为0\n图集路径:{0}", atlasData.atlasName);
                        return;
                    }
                    var count = op.mResultArray.Length;
                    atlasData.spriteDict = new Dictionary<string, Sprite>(count);
                    for (var i = 0; i < count; i++)
                    {
                        if (!(op.mResultArray[i] is Sprite item)) continue;
                        atlasData.spriteDict.Add(item.name, item);
                    }

                    if (atlasData.waitCallbackQueue != null)
                    {
                        while (atlasData.waitCallbackQueue.Count > 0)
                        {
                            var waitCallback = atlasData.waitCallbackQueue.Dequeue();
                            waitCallback?.Invoke();
                        }
                    }

                    FindSpriteFromAtlasData(atlasData, spriteName, callback);
                }
                atlasData.operation = _assetManager.AsyncLoadSubAsset<Sprite>(atlasName, OnCompleted);
            }

            /// <summary>
            /// 释放sprite
            /// </summary>
            /// <param name="atlasName"></param>
            /// <param name="spriteName"></param>
            public void ReleaseSprite(string atlasName, string spriteName)
            {
                var atlasResult = _atlasDataDict.TryGetValue(atlasName, out var atlasData);
                if (!atlasResult)
                {
                    FrameworkLog.ErrorFormat("释放sprite过程中,缓存中无法找到图集\n图集名字:{0}\nspriteName:{1}", atlasName, spriteName);
                    return;
                }
                --atlasData.refCount;
                if (atlasData.refCount > 0)
                {
                    return;
                }
                atlasData.refCount = 0;


                atlasData.atlasName = string.Empty;
                atlasData.waitCallbackQueue?.Clear();

                var result = _assetManager.ReleaseSubAsset(atlasData.operation);
                if (result)
                {
                    if (atlasData.spriteDict != null)
                    {
                        foreach (var kv in atlasData.spriteDict)
                        {
                            Resources.UnloadAsset(kv.Value);
                        }
                    }
                }
                atlasData.spriteDict?.Clear();
                _atlasDataDict.Remove(atlasName);
            }

            /// <summary>
            /// 从atlasdata中寻找sprite
            /// </summary>
            /// <param name="atlasData"></param>
            /// <param name="spriteName"></param>
            /// <param name="callback"></param>
            private void FindSpriteFromAtlasData(AtlasData atlasData, string spriteName, [NotNull] Action<Sprite> callback)
            {
                var spriteResult = atlasData.spriteDict.TryGetValue(spriteName, out var sprite);
                if (!spriteResult)
                {
                    FrameworkLog.ErrorFormat("从图集中加载sprite错误\n图集路径:{0}\nsprite名字:{1}", atlasData.atlasName, spriteName);
                    callback.Invoke(null);
                    return;
                }
                callback.Invoke(sprite);
            }
        }
    }
}
