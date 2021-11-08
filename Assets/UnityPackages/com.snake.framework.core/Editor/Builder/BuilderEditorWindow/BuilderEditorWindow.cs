using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Build.Pipeline;
using System.Collections.Generic;
using com.snake.framework.runtime;
using System.IO;

namespace com.snake.framework
{
    namespace editor
    {
        public class BuilderEditorWindow : EditorWindow
        {
            private Vector2 _assetRuleViewScrollPosition;
            private List<AssetRuleDraw> assetRuleDrawList;
            private EnvironmentSetting _envSetting;
            private BuilderSetting _builderSetting;

            private DefaultAsset _foldDefAsset;
            private string tmpRename;

            private void OnEnable()
            {
                assetRuleDrawList = new List<AssetRuleDraw>();
                loadAssetRule();
            }

            private void loadAssetRule()
            {
                assetRuleDrawList.Clear();
                _envSetting = EnvironmentSetting.Get();
                _builderSetting = BuilderSetting.EditorGet();
                if (string.IsNullOrEmpty(_builderSetting.mAssetRulesPath))
                {
                    SnakeDebuger.Error("δ��EnvironmentSetting.asset�����ã���Դ����·��.");
                    return;
                }
                string[] files = Directory.GetFiles(_builderSetting.mAssetRulesPath, "*.asset");

                string tmpPath = string.Empty;
                Dictionary<string, string> tmpDict = new Dictionary<string, string>();
                for (int i = 0; i < files.Length; i++)
                {
                    tmpPath = files[i].Replace("\\", "/");
                    AssetRuleDraw assetRuleDraw = new AssetRuleDraw(tmpPath);
                    assetRuleDrawList.Add(assetRuleDraw);
                }
                assetRuleDrawList.Sort((left, right) => left.mAssetRule.priority.CompareTo(right.mAssetRule.priority));
            }
            public void CreateGUI()
            {
                VisualElement root = rootVisualElement;
                var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UnityPackages/com.snake.framework.core/Editor/Builder/BuilderEditorWindow/BuilderEditorWindow.uxml");
                VisualElement labelFromUXML = visualTree.Instantiate();
                labelFromUXML.Q<IMGUIContainer>().onGUIHandler = OnGUIWithAssetRoleView;
                root.Add(labelFromUXML);
            }

            public void AssetBundleBuild(BundleBuildParameters parameters)
            {
                AssetBundleBuilder.BuildAssetBundle(parameters, null);
            }

            private void MoveItem(int index, bool moveUp) 
            {
                int dir = moveUp ? -1 : 1;
                AssetRuleDraw tmp = assetRuleDrawList[index + dir];
                assetRuleDrawList[index + dir] = assetRuleDrawList[index];
                assetRuleDrawList[index] = tmp;

                for (int i = 0; i < assetRuleDrawList.Count; i++)
                {
                    assetRuleDrawList[i].mAssetRule.priority = i;
                }
            }

            /// <summary>
            /// ��Դ�����б����
            /// </summary>
            private void OnGUIWithAssetRoleView()
            {
                GUILayout.Label("����Ŀ¼��" + this._builderSetting.mAssetRulesPath);
                GUILayout.Label("��ԴĿ¼��" + this._envSetting.mResRootPath);
                GUILayout.Label("���ƽ̨��" + EditorUserBuildSettings.activeBuildTarget);

                using (EditorGUILayout.ScrollViewScope svs = new EditorGUILayout.ScrollViewScope(_assetRuleViewScrollPosition, GUILayout.MaxWidth(360)))
                {
                    _assetRuleViewScrollPosition = svs.scrollPosition;
                    if (GUILayout.Button("����"))
                    {
                        string savePath = Path.Combine(this._builderSetting.mAssetRulesPath, "TmpAssetRule.asset");
                        if (AssetDatabase.LoadAssetAtPath<AssetRule>(savePath) != null)
                        {
                            SnakeDebuger.Error("����ʧ�ܣ��Ѵ�����Դ���������ļ�:TmpAssetRule.asset,����ɾ������ļ�����������");
                            return;
                        }

                        AssetRule assetRule = CreateInstance<AssetRule>();
                        assetRule.types = new string[0];
                        assetRule.filters = new string[0];
                        AssetDatabase.CreateAsset(assetRule, savePath);
                        AssetDatabase.Refresh();
                        AssetRuleDraw assetRuleDraw = new AssetRuleDraw(savePath);
                        assetRuleDraw.mRenameing = true;
                        tmpRename = "TmpAssetRule";
                        assetRuleDrawList.Add(assetRuleDraw);
                    }


                    for (int i = 0; i < assetRuleDrawList.Count; i++)
                    {
                        using (new EditorGUILayout.VerticalScope("HelpBox"))
                        {
                            AssetRuleDraw assetRuleDraw = assetRuleDrawList[i];

                            using (new EditorGUILayout.HorizontalScope())
                            {
                                if (assetRuleDraw.mRenameing)
                                    tmpRename = EditorGUILayout.TextField(tmpRename);
                                else
                                    assetRuleDraw.mFoldout = EditorGUILayout.Foldout(assetRuleDraw.mFoldout, assetRuleDraw.mName);

                                if (GUILayout.Button(assetRuleDraw.mRenameing ? "ȷ��" : "����", GUILayout.Width(60)))
                                {
                                    if (assetRuleDraw.mRenameing == true)
                                    {
                                        string foldPath = Path.GetDirectoryName(assetRuleDraw.mAssetRulePath);
                                        string newAssetPath = Path.Combine(foldPath, tmpRename + ".asset").Replace("\\", "/");
                                        File.Move(assetRuleDraw.mAssetRulePath, newAssetPath);
                                        AssetDatabase.Refresh();
                                        loadAssetRule();
                                        return;
                                    }
                                    else
                                    {
                                        tmpRename = assetRuleDraw.mName;
                                    }
                                    assetRuleDraw.mRenameing = !assetRuleDraw.mRenameing;
                                }
                                if (GUILayout.Button("ɾ��", GUILayout.Width(60)))
                                {
                                    AssetDatabase.DeleteAsset(assetRuleDraw.mAssetRulePath);
                                    assetRuleDrawList.RemoveAt(i--);
                                    return;
                                }

                                using (var bdg = new EditorGUI.DisabledScope(i == 0))
                                {
                                    if (GUILayout.Button("��", GUILayout.Width(20)))
                                    {
                                        MoveItem(i, true);
                                        return;
                                    }
                                }
                                using (var bdg = new EditorGUI.DisabledScope(i == assetRuleDrawList.Count - 1))
                                {
                                    if (GUILayout.Button("��", GUILayout.Width(20)))
                                    {
                                        MoveItem(i, false);
                                        return;
                                    }
                                }
                            }

                            if (assetRuleDraw.mFoldout)
                            {
                                using (new EditorGUILayout.VerticalScope("HelpBox"))
                                {
                                    using (new EditorGUILayout.HorizontalScope())
                                    {
                                        if (assetRuleDraw.modifyFoldPath == true)
                                        {
                                            using (var ccs = new EditorGUI.ChangeCheckScope())
                                            {
                                                _foldDefAsset = EditorGUILayout.ObjectField(_foldDefAsset, typeof(DefaultAsset), false) as DefaultAsset;
                                                if (ccs.changed)
                                                {
                                                    string resPath = AssetDatabase.GetAssetPath(_foldDefAsset);
                                                    if (resPath.StartsWith(_envSetting.mResRootPath) == false)
                                                    {
                                                        SnakeDebuger.Error("��Դ��Ŀ¼����Ϊ��" + _envSetting.mResRootPath);
                                                        return;
                                                    }
                                                    assetRuleDraw.mAssetRule.foldPath = resPath;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            GUILayout.Label("��ԴĿ¼��" + assetRuleDraw.mAssetRule.foldPath);
                                        }

                                        if (GUILayout.Button(assetRuleDraw.modifyFoldPath ? "ȷ��" : "�޸�", GUILayout.Width(60)))
                                        {
                                            assetRuleDraw.modifyFoldPath = !assetRuleDraw.modifyFoldPath;
                                        }
                                    }
                                    EditorGUILayout.PropertyField(assetRuleDraw.mSerializedObject.FindProperty("single"), new GUIContent("�������"));
                                    using (new EditorGUILayout.HorizontalScope())
                                    {
                                        using (new EditorGUILayout.VerticalScope("HelpBox"))
                                        {
                                            using (new EditorGUILayout.HorizontalScope())
                                            {
                                                GUILayout.Label("��Դ���ͣ�");
                                                if (GUILayout.Button("����", GUILayout.Width(40)))
                                                {
                                                    List<string> tmp = new List<string>(assetRuleDraw.mAssetRule.types);
                                                    tmp.Add(string.Empty);
                                                    assetRuleDraw.mAssetRule.types = tmp.ToArray();
                                                }
                                                if (GUILayout.Button("����", GUILayout.Width(40)))
                                                {
                                                    List<string> tmp = new List<string>();
                                                    for (int j = 0; j < assetRuleDraw.mAssetRule.types.Length; j++)
                                                    {
                                                        if (string.IsNullOrEmpty(assetRuleDraw.mAssetRule.types[j]))
                                                            continue;
                                                        tmp.Add(assetRuleDraw.mAssetRule.types[j]);
                                                    }
                                                    assetRuleDraw.mAssetRule.types = tmp.ToArray();
                                                }
                                            }
                                            for (int j = 0; j < assetRuleDraw.mAssetRule.types.Length; j++)
                                            {
                                                assetRuleDraw.mAssetRule.types[j] = EditorGUILayout.TextField(assetRuleDraw.mAssetRule.types[j]);
                                            }
                                        }
                                        using (new EditorGUILayout.VerticalScope("HelpBox"))
                                        {
                                            using (new EditorGUILayout.HorizontalScope())
                                            {
                                                GUILayout.Label("�������ͣ�");
                                                if (GUILayout.Button("����", GUILayout.Width(40)))
                                                {
                                                    List<string> tmp = new List<string>(assetRuleDraw.mAssetRule.filters);
                                                    tmp.Add(string.Empty);
                                                    assetRuleDraw.mAssetRule.filters = tmp.ToArray();
                                                }
                                                if (GUILayout.Button("����", GUILayout.Width(40)))
                                                {
                                                    List<string> tmp = new List<string>();
                                                    for (int j = 0; j < assetRuleDraw.mAssetRule.filters.Length; j++)
                                                    {
                                                        if (string.IsNullOrEmpty(assetRuleDraw.mAssetRule.filters[j]))
                                                            continue;
                                                        tmp.Add(assetRuleDraw.mAssetRule.filters[j]);
                                                    }
                                                    assetRuleDraw.mAssetRule.filters = tmp.ToArray();
                                                }
                                            }

                                            for (int j = 0; j < assetRuleDraw.mAssetRule.filters.Length; j++)
                                            {
                                                assetRuleDraw.mAssetRule.filters[j] = EditorGUILayout.TextField(assetRuleDraw.mAssetRule.filters[j]);
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }
    }
}
