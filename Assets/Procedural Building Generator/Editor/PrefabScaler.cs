using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabScaler : EditorWindow
{
    private float scale;

    [MenuItem("Window / Prefab Scaler")]
    public static void ShowWindow()
    {
        GetWindow<PrefabScaler>();
    }

    private void OnGUI()
    {
        GUILayout.Label("Scal Selected Prefabs", EditorStyles.boldLabel);
        scale = EditorGUILayout.FloatField("Scale", scale);

        if (GUILayout.Button("Apply"))
        {
            ScaleSelectedPrefabs();
        }

        EditorGUILayout.HelpBox("Select the prefabs you want to scale then click Apply", MessageType.Info);
    }

    private void ScaleSelectedPrefabs()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            if (PrefabUtility.IsPartOfAnyPrefab(obj))
            {
                GameObject prefabRoot = PrefabUtility.GetNearestPrefabInstanceRoot(obj);
                if (prefabRoot != null)
                {
                    prefabRoot.transform.localScale *= scale;
                    PrefabUtility.ApplyPrefabInstance(prefabRoot, InteractionMode.UserAction);
                }
            }
        }
        Debug.Log($"All selected prefabs have been scaled by {scale} times.");
    }
}
