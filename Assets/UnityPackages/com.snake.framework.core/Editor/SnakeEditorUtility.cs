using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace com.snake.framework 
{
    namespace editor 
    {
        public class SnakeEditorUtility
        {
            static public string GetPath(string _scriptName)
            {
                string[] path = UnityEditor.AssetDatabase.FindAssets(_scriptName);
                if (path.Length > 1)
                {
                    Debug.LogError("��ͬ���ļ�" + _scriptName + "��ȡ·��ʧ��");
                    return null;
                }
                //���ַ����еýű����ֺͺ�׺ͳͳȥ����
                string _path = AssetDatabase.GUIDToAssetPath(path[0]).Replace((@"/" + _scriptName + ".cs"), "");
                return _path;
            }
        }
    }
}