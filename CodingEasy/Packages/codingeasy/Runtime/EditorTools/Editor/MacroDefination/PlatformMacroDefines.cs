using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

[System.Serializable]
[CreateAssetMenu(fileName = "PlatformMacroDefines.asset",menuName ="宏定义")]

public class PlatformMacroDefines : ScriptableObject
{
    public MacroDefination[] Defines;

    public GUIContent[] gUIContents;

    private MacroDefination.MacroItem[] copyData;

    public void Init() {
        Defines = new MacroDefination[3];
        gUIContents = new GUIContent[Defines.Length];

        Defines[0] = new MacroDefination { targetGroup = BuildTargetGroup.Standalone};
        Defines[1] = new MacroDefination { targetGroup = BuildTargetGroup.iOS };
        Defines[2] = new MacroDefination { targetGroup = BuildTargetGroup.Android };

        for (int i = 0; i < gUIContents.Length; i++)
        {
            gUIContents[i] = new GUIContent(Defines[i].targetGroup.ToString());
        }
    }

    [OnOpenAsset]
    public static bool Open(int instanceID, int line) {
       
        var obj = EditorUtility.InstanceIDToObject(instanceID);
        Debug.Log($"click on {obj.name}");
        if (obj is PlatformMacroDefines) {
            MacroDefinationWindow.Open();
            return true;
        }
        return false;
    }

    int selected;
    public void OnDraw() {

        selected = GUILayout.Toolbar(selected, gUIContents);
        Defines[selected].OnDraw();
        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Copy"))
            {                
                copyData = Defines[selected].macroItems.ToArray();
            }
            if (GUILayout.Button("Paste"))
            {
                Defines[selected].macroItems.Clear();
                Defines[selected].macroItems.AddRange(copyData);
                Save();
            }
        }
        EditorGUILayout.EndHorizontal();

        if (!EditorUserBuildSettings.activeBuildTarget.ToString().Contains(Defines[selected].targetGroup.ToString())) {
            return;
        }

        if (GUILayout.Button("应用")) {
            //更新当前平台的宏
            Defines[selected].Apply();
        }
    }

    void Save() {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}