using UnityEngine;
using UnityEditor;
using System.IO;

public class RemoveASECustomEditor : EditorWindow
{
    [MenuItem("Tools/Fix ASE Shader UI Error")]
    public static void ShowWindow()
    {
        GetWindow<RemoveASECustomEditor>("Fix ASE Shader UI");
    }

    private void OnGUI()
    {
        GUILayout.Label("���� ASE Shader CustomEditor", EditorStyles.boldLabel);

        if (GUILayout.Button("���y�ò��� ASEMaterialInspector"))
        {
            FixAllShaders();
        }
    }

    private static void FixAllShaders()
    {
        string[] shaderGUIDs = AssetDatabase.FindAssets("t:Shader");

        int fixCount = 0;

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
                File.WriteAllLines(path, lines);
                Debug.Log($" �ץ� Shader: {path}");
            }
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("����", $"�w�ץ� {fixCount} �� Shader�I", "OK");
    }
}