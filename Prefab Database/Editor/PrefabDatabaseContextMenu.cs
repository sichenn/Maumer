using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Maumer
{
    // Author: Sichen Liu. 2018
    // Usage:
    // By naming prefab names such as ObjectType_MapLevel_Name (ex. Chr_Common_Player) this tool
    // can automatically sort them and help you instantiate them in the scene
    /// <summary>
    /// Map editor tool which displays a scene view context menu to instantiate prefabs
    /// </summary>
    [InitializeOnLoad]
    public class PrefabDatabaseContextMenu : Editor
    {

        // TODO: This should be configurable in the future. 
        // Only prefabs in this folder (or even multiple folders) will be searched
        private static string prefabFolder = "Assets/Prefabs/";
        private static string selectedAssetPath;
        private static Vector2 lastMousePosInSceneView2D;

        static PrefabDatabaseContextMenu()
        {
            PrefabDatabase.RefreshPrefabDatabase();
            SceneView.onSceneGUIDelegate += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            // if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
            // {
            //     // executed on right click up
            //     GenericMenu menu = new GenericMenu();
            //     addPrefabContextMenu(menu);
            //     // we record the mouse position here because it's only retrievable in OnGUI
            //     lastMousePosInSceneView2D = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
            //     menu.ShowAsContext();
            // }
        }

        /// <summary>
        /// Takes a generic menu and add all prefabs from the database to the context menu
        /// </summary>
        /// <param name="menu"></param>
        private static void addPrefabContextMenu(GenericMenu menu)
        {
            // goes through the tree structure recorded in the editor window
            var paths = findPrefabsRecursively(PrefabDatabase.rootCategory);
            int index = 1;
            foreach (var prefabFormattedName in paths.Split(new char[] { '|' }))
            {
                menu.AddItem(new GUIContent(prefabFormattedName), false, recordPrefabAssetName, prefabFormattedName);
                index++;
            }
        }

        private static string findPrefabsRecursively(TreeNode<string> currentCategory)
        {
            if (currentCategory.children == null)
            {
                return getTreeNodePath(currentCategory) + "|";
            }
            else
            {
                var result = "";
                for (int i = 0; i < currentCategory.children.Count; i++)
                {
                    result += findPrefabsRecursively(currentCategory.children[i]);
                }

                return result;
            }
        }

        /// <summary>
        /// Temporary. This should be moved to TreeNode later
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        private static string getTreeNodePath<T>(TreeNode<T> node)
        {
            string path = "";
            Stack<TreeNode<T>> pathStack = new Stack<TreeNode<T>>();
            TreeNode<T> currentNode = node;
            while (currentNode != null && currentNode.value != null)
            {
                pathStack.Push(currentNode);
                currentNode = currentNode.Parent;
            }
            while (pathStack.Count != 0)
            {
                var pathAddition = pathStack.Pop().value;
                path += pathAddition;
                if (pathStack.Count != 0)
                    path += "/";
            }
            return path;
        }

        private static void recordPrefabAssetName(object obj)
        {
            string path;
            string assetName = obj.ToString().Replace('/', PrefabDatabase.splitSymbol);
            if (PrefabDatabase.prefabPaths.TryGetValue(assetName, out path))
            {
                // path found in database
                selectedAssetPath = path;
            }
            else
            {
                // path not found in database
                Debug.LogWarning("Could not find asset path for asset named: " + assetName);
            }

            var gameObject = instantiatePrefabFromPath(path);
            if(gameObject)
            {
                gameObject.transform.position = lastMousePosInSceneView2D;
            }

        }

        private static GameObject instantiatePrefabFromPath(string path)
        {
            // find prefab in asset database
            var prefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            if (prefab)
            {
                // convert to instance
                prefab = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                EditorGUIUtility.PingObject(prefab);
                Undo.RecordObject(prefab, "instantiated new prefab");
                SceneView.lastActiveSceneView.FrameSelected();

                // mark scene dirty
                var activeScene = SceneManager.GetActiveScene();
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(activeScene);
                Selection.activeGameObject = prefab;
            }
            else
            {
                Debug.LogError("could not instantiate prefab from path: " + path + ". Check if it still exists");
            }
            return prefab;
              
        }
    }

}
