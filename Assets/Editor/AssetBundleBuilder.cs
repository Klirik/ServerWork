using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class AssetBundleBuilder : MonoBehaviour
    {
        [MenuItem("AssetBundle/Build")]
        public static void CreateAssetBundle()
        {
            BuildPipeline.BuildAssetBundles("Assets/AssetBundle", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }
    }
}