using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Maumer
{
    public static class PrefabDatabase
    {
        public static TreeNode<string> rootCategory
        {
            get
            {
                if (_rootCategory == null)
                    _rootCategory = new TreeNode<string>();
                return _rootCategory;
            }
        }
        public static TreeNode<string> _rootCategory;
        public static Dictionary<string, string> prefabPaths;
        public static char splitSymbol = '_';
        public static int numPrefabs;
        public static string[] folders;

        public static void RefreshPrefabDatabase()
        {
            var prefabs = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Prefabs" });
            numPrefabs = prefabs.Length;


            // TODO: Reset root category. Maybe there is a cleaner/ memory efficient way?
            _rootCategory = null;

            if (prefabPaths == null)
                prefabPaths = new Dictionary<string, string>();
            else
                prefabPaths.Clear();

            foreach (string prefab in prefabs)
            {
                //get the prefab's actual filname
                string assetPath = AssetDatabase.GUIDToAssetPath(prefab);

                var assetName = assetPath.Substring(assetPath.LastIndexOf("/") + 1);
                // remove the extension text
                var assetNameWithoutExtension = assetName.Replace(".prefab", "");


                // split the asset name with our underscore split symbol
                var splitCategories = assetNameWithoutExtension.Split(splitSymbol);
                var currentCategory = rootCategory;

                //construct the category tree
                for (int i = 0; i < splitCategories.Length; i++)
                {
                    var category = splitCategories[i];
                    if (currentCategory.children == null || !currentCategory.children.Any(c => c.value == category))
                    {
                        // if there are no exisitng child of this category we add it as a child
                        currentCategory = currentCategory.AddChild(category);
                    }
                    else
                    {
                        // if there's already a child of the same category we then ignore it and go on to the next child
                        currentCategory = currentCategory.children.FirstOrDefault(c => c.value == category);
                    }
                    if (i == splitCategories.Length - 1)
                    {
                        // when we reach the end of the file name we save the key/value pair
                        // for use when we want to instantiate this object elsewhere
                        if (!prefabPaths.ContainsKey(assetNameWithoutExtension))
                        {
                            prefabPaths.Add(assetNameWithoutExtension, assetPath);
                        }
                    }
                }
            }
        }
    }
}