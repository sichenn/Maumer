using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace Maumer
{
    public class PrefabMenuEditorWindow : EditorWindow
    {
        private static string testString;
        private Vector2 scrollPos;
        private static PrefabMenuEditorWindow window;
        public string[] test;

        [MenuItem("Window/Maumer/PrefabMenu")]
        static void Init()
        {
            window = (PrefabMenuEditorWindow)EditorWindow.GetWindow(typeof(PrefabMenuEditorWindow));
            window.titleContent = new GUIContent("Prefab Data");
            window.Show();
        }

        private void OnGUI()
        {
            displayDatabaseInfo();
            if (GUILayout.Button("Refresh Prefab Database"))
            {
                PrefabDatabase.RefreshPrefabDatabase();
            }
        }

        private static void displayDatabaseInfo()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Number of prefabs: " + PrefabDatabase.numPrefabs);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField("Options below are still being worked on");
            EditorGUILayout.TextField(new GUIContent("Split Symbol", "devs will use this symbol to categorize prefabs"),"");
            // this should be an array but we don't have a custom string array editor method yet
            EditorGUILayout.TextField(new GUIContent("Folders", "Prefabs will be retrieved from these folders"), "");
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();
        }

       
    }

}
