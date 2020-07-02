using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;

[System.Serializable]
public class MacroDefination 
{
    public BuildTargetGroup targetGroup;


    [System.Serializable]
    public struct MacroItem {        
        public string Name;
        public string Describe;
        public bool isEnable;
    }

    public List<MacroItem> macroItems = new List<MacroItem>();
    public string macroDef;
    public Vector2 pos;
    public void OnDraw() {

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("宏名称");
            EditorGUILayout.LabelField("描述");
            EditorGUILayout.LabelField("启用",GUILayout.Width(100));


            if (GUILayout.Button("+"))
            {
                macroItems.Add(new MacroItem());
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.TextField(targetGroup.ToString(), macroDef);

        if (macroItems == null) {
            return;
        }
        

        EditorGUI.BeginChangeCheck();
        {
            pos = EditorGUILayout.BeginScrollView(pos);
            {
                
                for (int i = 0; i < macroItems.Count; i++)
                {
                    var item = macroItems[i];
                    EditorGUILayout.BeginHorizontal();
                    {
                        item.Name = EditorGUILayout.TextField(item.Name);
                        item.Describe = EditorGUILayout.TextField(item.Describe);
                        item.isEnable = EditorGUILayout.Toggle(item.isEnable, GUILayout.Width(100));

                        if (GUILayout.Button("x"))
                        {
                            if (EditorUtility.DisplayDialog("提示", "是否删除", "是", "取消"))
                            {
                                macroItems.RemoveAt(i);
                                return;
                            }

                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    macroItems[i] = item;
                }
            }
            EditorGUILayout.EndScrollView();
        }
        if (EditorGUI.EndChangeCheck()) {
            Update();
        }
    }

    public void Update() {
        List<string> defs = new List<string>();

        for (int i = 0; i < macroItems.Count; i++)
        {
            var item = macroItems[i];
            if (item.isEnable)
            {
                defs.Add(item.Name);
            }
        }
        macroDef = string.Join(";", defs.ToArray());
    }

    public void Apply() {
       

        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, macroDef);
    }
}