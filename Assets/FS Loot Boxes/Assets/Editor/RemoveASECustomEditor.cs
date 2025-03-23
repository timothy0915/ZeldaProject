using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class RemoveASECustomEditor : EditorWindow
{
    [MenuItem("Tools/Fix ASE Shader UI Error")]
    public static void ShowWindow()
    {
        GetWindow<RemoveASECustomEditor>("Fix ASE Shader UI");
    }

    private void OnGUI()
    {
        GUILayout.Label("掃描 QFX 資料夾並移除 ASE Shader UI", EditorStyles.boldLabel);

        if (GUILayout.Button("開始修復 QFX 裡的 Shader"))
        {
            FixShadersInQFX();
        }
    }

    private static void FixShadersInQFX()
    {
        string[] shaderGUIDs = AssetDatabase.FindAssets("t:Shader", new[] { "Assets/QFX" });

        int fixCount = 0;

        // 暫存要修改的檔案
        Dictionary<string, string[]> modifiedFiles = new Dictionary<string, string[]>();

        foreach (string guid in shaderGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string[] lines = File.ReadAllLines(path);

            bool modified = false;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("CustomEditor") && lines[i].Contains("ASEMaterialInspector"))
                {
                    lines[i] = "// " + lines[i] + "  // Removed by tool";
                    modified = true;
                    fixCount++;
                }
            }

            if (modified)
            {
                modifiedFiles.Add(path, lines);
            }
        }

        // 統一寫入（避免中途刷新）
        foreach (var entry in modifiedFiles)
        {
            File.WriteAllLines(entry.Key, entry.Value);
            Debug.Log($" 修正 Shader: {entry.Key}");
        }

        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("完成", $"已修正 {fixCount} 個 Shader（僅限 QFX）！", "OK");
    }
}
