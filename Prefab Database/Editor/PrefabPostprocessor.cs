using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Maumer
{
    public class PrefabPostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] aImportedAssets, string[] aDeletedAssets, string[] aMovedAssets, string[] aMovedFromAssetPaths)
        {
            PrefabDatabase.RefreshPrefabDatabase();
        }
    }

}
