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
        GUILayout.Label("���y QFX ��Ƨ��ò��� ASE Shader UI", EditorStyles.boldLabel);

        if (GUILayout.Button("�}�l�״_ QFX �̪� Shader"))
        {
            FixShadersInQFX();
        }
    }

    private static void FixShadersInQFX()
    {
        string[] shaderGUIDs = AssetDatabase.FindAssets("t:Shader", new[] { "Assets/QFX" });

        int fixCount = 0;

        // �Ȧs�n�ק諸�ɮ�
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

        // �Τ@�g�J�]�קK���~��s�^
        foreach (var entry in modifiedFiles)
        {
            File.WriteAllLines(entry.Key, entry.Value);
            Debug.Log($" �ץ� Shader: {entry.Key}");
        }

        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("����", $"�w�ץ� {fixCount} �� Shader�]�ȭ� QFX�^�I", "OK");
    }
}
