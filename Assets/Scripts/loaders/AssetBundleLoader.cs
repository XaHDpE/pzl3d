using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace loaders
{
    public class AssetBundleLoader
    {
        
        private IDictionary<string, AssetBundle> loadedBundles;
        private IDictionary<string, GameObject> loadedAssets;

        public AssetBundleLoader()
        {
            loadedBundles = new Dictionary<string, AssetBundle>();
            loadedAssets = new Dictionary<string, GameObject>();
        }

        private AssetBundle LoadAssetBundle(string path)
        {
            var fullPath = Path.Combine(Application.streamingAssetsPath, path);

            if (loadedBundles.ContainsKey(fullPath))
                return loadedBundles[fullPath]; 
            
            var assetBundleLoaded = AssetBundle.LoadFromFile(fullPath);
            if (assetBundleLoaded != null)
            {
                loadedBundles.Add(fullPath, assetBundleLoaded);
                return assetBundleLoaded;
            }
            
            Debug.Log("Failed to load AssetBundle!");
            return null;

        }

        public GameObject GetAssetByName(string bundlePath, string assetName)
        {
            if (loadedAssets.ContainsKey(assetName))
                return loadedAssets[assetName];

            var result = LoadAssetBundle(bundlePath).LoadAsset<GameObject>(assetName);
            loadedAssets.Add(assetName, result);
            return result;
        }
        
    }
}