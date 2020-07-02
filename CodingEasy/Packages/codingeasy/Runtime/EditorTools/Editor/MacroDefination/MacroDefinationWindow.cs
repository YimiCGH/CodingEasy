using UnityEngine;
using UnityEditor;
using System.IO;
public class MacroDefinationWindow : EditorWindow
{
    const string saveDataPath = "Assets/Editor/PlatformMacroDefines.asset";

    [MenuItem("Tools/Macro")]
    public static void Open()
    {
        GetWindow<MacroDefinationWindow>();
    }

    public PlatformMacroDefines platformMacroDefines;

    private void OnEnable()
    {
        platformMacroDefines = AssetDatabase.LoadAssetAtPath<PlatformMacroDefines>(saveDataPath);

        var dir = new DirectoryInfo(Application.dataPath + "/Editor");
        if (!dir.Exists)
        {
            dir.Create();
        }

        if (platformMacroDefines == null)
        {
            platformMacroDefines = ScriptableObject.CreateInstance<PlatformMacroDefines>();
            platformMacroDefines.Init();
            AssetDatabase.CreateAsset(platformMacroDefines, saveDataPath);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    private void OnGUI()
    {
        if (platformMacroDefines == null)
        {
            return;
        }
        platformMacroDefines.OnDraw();
    }
}