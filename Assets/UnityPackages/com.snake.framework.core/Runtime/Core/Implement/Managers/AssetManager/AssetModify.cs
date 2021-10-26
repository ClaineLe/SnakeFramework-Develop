#if UNITY_EDITOR
using com.halo.framework.runtime;
using HaloFramework.Common.GPUI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace com.halo.framework
{
    namespace runtime
    {
        static public class AssetModify 
        {
            private class ShaderInfo
            {
                /// <summary>
                public string name = "";
                /// <summary>
                /// 缓存的shader
                /// </summary>
                public Shader shader = null;
            }

            /// <summary>
            /// 缓存的shader
            /// </summary>
            static private List<ShaderInfo> shaders = new List<ShaderInfo>();

            /// <summary>
            /// 缓存Material
            /// </summary>
            static private Dictionary<string, Material> cacheMats = new Dictionary<string, Material>();


            static public List<string> preloadShaders = new List<string>();


            /// <summary>
            /// 生成对应的Shader
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            static private Shader GetShader(string name)
            {
                Shader shader = null;
                for (int i = 0; i < shaders.Count; i++)
                {
                    if (shaders[i].name == name)
                    {
                        shader = shaders[i].shader;
                    }
                }

                if (shader == null)
                {
                    shader = Shader.Find(name);
                    if (shader != null)
                    {
                        shaders.Add(new ShaderInfo()
                        {
                            name = name,
                            shader = shader
                        });
                    }
                    else
                    {
                        Debug.LogError("找不到shader : " + name);
                    }
                }

                return shader;
            }

            /// <summary>
            /// 获取mat
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            static private Material GetMatInCache(string name)
            {
                if (cacheMats.ContainsKey(name))
                {
                    return cacheMats[name];
                }
                else
                {
                    //这里的路径需要改动 暂时不知道改成啥  先留着  董俊杰
                    string path = "Assets/ResExport/Material/" + name + ".mat";
                    Material mat = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(path);
                    if (mat != null)
                    {
                        cacheMats.Add(name, mat);
                    }
                    return mat;
                }
            }
            static public void ModifyAsset(GameObject gameObject)
            {
                if (AppConst.ASSET_BUNDLE_MODE == false)
                    return;
                if (gameObject.name.Contains("Panel"))
                {
                    ModifyShaderForUi(gameObject);
                }
                ModifyShaderForGameObject(gameObject);
            }

            static public void ModifyScene(Scene scene)
            {
                if (AppConst.ASSET_BUNDLE_MODE == false)
                    return;

                GameObject[] objs = scene.GetRootGameObjects();
                if (objs != null && objs.Length > 0)
                {
                    for (int i = 0; i < objs.Length; i++)
                    {
                        ModifyShaderForGameObject(objs[i]);
                    }
                }
            }

            static private void ModifyShaderForGameObject(GameObject go)
            {
                if (go)
                {
                    DrawMeshGPUInstance[] gpuInstances = go.GetComponentsInChildren<DrawMeshGPUInstance>(true);
                    foreach (var renderer in gpuInstances)
                    {
                        Material mat = renderer.mat_instance;
                        if (mat)
                        {
                            int oldrenderque = mat.renderQueue;
                            Shader shader = mat.shader;
                            if (shader)
                            {
                                Shader sd = GetShader(shader.name) as Shader;
                                if (sd && sd.name.Equals(shader.name))
                                {
                                    mat.shader = sd;
                                    mat.renderQueue = oldrenderque;
                                }
                            }
                        }
                    }

                    Renderer[] renderers = go.GetComponentsInChildren<Renderer>(true);
                    foreach (var renderer in renderers)
                    {

                        Material[] materials = renderer.sharedMaterials;
                        for (int i = 0; i < materials.Length; i++)
                        {
                            Material mat = materials[i];
                            if (mat)
                            {
                                int oldrenderque = mat.renderQueue;
                                Shader shader = mat.shader;
                                if (shader)
                                {
                                    Shader sd = GetShader(shader.name) as Shader;
                                    if (sd && sd.name.Equals(shader.name))
                                    {
                                        mat.shader = sd;
                                        mat.renderQueue = oldrenderque;
                                    }
                                }
                            }
                        }

                    }
                }
            }

            static private void ModifyShaderForUi(GameObject go)
            {
                Image[] images = go.GetComponentsInChildren<Image>(true);
                for (int i = 0; i < images.Length; i++)
                {
                    var image = images[i];

                    if (image.material)
                    {
                        if (image.material.name == "UI_Add_SoftMask")
                        {
                            image.material = GetMatInCache("UI_Add_SoftMask");
                        }

                        var shader = image.material.shader;
                        if (shader)
                        {
                            var sd = GetShader(shader.name) as Shader;
                            if (sd && sd.name.Equals(shader.name))
                            {
                                image.material.shader = sd;
                            }
                        }
                    }
                }
            }
        }
    }
}
#endif