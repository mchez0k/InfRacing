using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Utils
{
    public class ResourceLoader
    {
        private static readonly Dictionary<string, Object> _resourcesCache = new Dictionary<string, Object>(System.StringComparer.Ordinal);
        private static readonly Dictionary<string, Object> _addressablesCache = new Dictionary<string, Object>(System.StringComparer.Ordinal);
        public static readonly UnityEvent<string> OnAssetBundleLoaded = new();

        public static void LoadAssetsWithLabel(AssetLabelReference label)
        {
            Addressables.LoadAssetsAsync<GameObject>(label, null).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    foreach (var prefab in handle.Result)
                    {
                        _addressablesCache.TryAdd(prefab.name, prefab);
                    }
                }
                else
                {
                    Debug.LogError($"Failed to load assets");
                }
            };
        }

        public static void LoadAddressableResource<T>(string assetName, System.Action<T> onComplete) where T : Object
        {
            if (_addressablesCache.TryGetValue(assetName, out var cachedAsset))
            {
                onComplete?.Invoke(cachedAsset as T);
                return;
            }

            Addressables.LoadAssetAsync<T>(assetName).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    _addressablesCache[assetName] = handle.Result;
                    onComplete?.Invoke(handle.Result);
                }
                else
                {
                    onComplete?.Invoke(null);
                    Debug.LogError($"Failed to load {assetName}");
                }
            };
        }

        public void UnloadAsset(string assetKey)
        {
            if (_addressablesCache.ContainsKey(assetKey))
            {
                Addressables.Release(_addressablesCache[assetKey]);
                _addressablesCache.Remove(assetKey);
                Debug.Log($"Unloaded {assetKey}");
            }
            else
            {
                Debug.LogWarning($"Asset {assetKey} not loaded or unloaded already");
            }
        }
    }
}