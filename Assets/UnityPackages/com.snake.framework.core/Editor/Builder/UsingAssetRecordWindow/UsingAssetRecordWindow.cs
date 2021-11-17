using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace com.snake.framework
{
    namespace editor
    {
        public class UsingAssetRecordWindow : EditorWindow
        {
            static private List<string> _usingAssetList;
            [MenuItem("SnakeTools/��Դ¼�Ʊ༭��")]
            static public void ShowBuilderEditorWindow()
            {
                UsingAssetRecordWindow wnd = GetWindow<UsingAssetRecordWindow>();
                wnd.titleContent = new GUIContent("��Դ¼�Ʊ༭��");
            }

            private void OnEnable()
            {
                reset();
            }

            private void reset()
            {
                _usingAssetList = new List<string>();
            }

            public void CreateGUI()
            {
                VisualElement root = rootVisualElement;
                var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(SnakeEditorUtility.GetPackagesPath() + "/Editor/Builder/UsingAssetRecordWindow/UsingAssetRecordWindow.uxml");
                if (visualTree == null)
                {
                    Debug.LogError("û���ҵ���Դuxml�ļ�.path:" + SnakeEditorUtility.GetPackagesPath() + "/Editor/Builder/UsingAssetRecordWindow/UsingAssetRecordWindow.uxml");
                    return;
                }
                VisualElement labelFromUXML = visualTree.Instantiate();
                labelFromUXML.Q<IMGUIContainer>().onGUIHandler = OnGUIWithAssetRoleView;
                root.Add(labelFromUXML);
            }

            private Vector2 _scrollPosition;
            private void OnGUIWithAssetRoleView()
            {
                using (EditorGUILayout.ScrollViewScope svs = new EditorGUILayout.ScrollViewScope(_scrollPosition, GUILayout.MaxWidth(360)))
                {
                    _scrollPosition = svs.scrollPosition;
                    for (int i = 0; i < _usingAssetList.Count; i++)
                    {
                        GUILayout.Label(_usingAssetList[i]);
                    }
                }
            }

            static public void AddUsingAsset(string assetPath)
            {
                _usingAssetList.Add(assetPath);
            }
        }
    }
}