using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.ResourceManagement.ResourceLocations;

public class ResourceManager
{
    private readonly Dictionary<string, UnityEngine.Object> _prefabCache = new();

    private readonly Dictionary<string, AsyncOperationHandle> _handleCache = new();

    private readonly Dictionary<string, Queue<GameObject>> _pool = new();

    public async UniTask<T> LoadAsync<T>(string key) where T : UnityEngine.Object
    {
        if (_prefabCache.TryGetValue(key, out var cached))
        {
            return cached as T;
        }

        var handle = Addressables.LoadAssetAsync<T>(key);

        await handle.Task;

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"Failed to load asset at {key}");
        }

        _handleCache[key] = handle;
        _prefabCache[key] = handle.Result;

        return handle.Result;
    }



    public async UniTask<GameObject> InstantiateAsync(string key, Transform parent = null)
    {
        if (!_pool.TryGetValue(key, out var queue))
        {
            queue = new Queue<GameObject>();
            _pool[key] = queue;
        }

        if (queue.Count > 0)
        {
            var pooled = queue.Dequeue();
            if (parent != null)
                pooled.transform.SetParent(parent, false);

            pooled.SetActive(true);
            return pooled;
        }

        var prefab = await LoadAsync<GameObject>(key);
        if (prefab == null)
        {
            Debug.LogError($"Prefab at {key} is null");
            return null;
        }

        var go = UnityEngine.Object.Instantiate(prefab, parent, false);
        go.name = prefab.name;

        return go;

    }


    public void Release(string key, GameObject go)
    {
        if (go == null) return;

        if (!_pool.TryGetValue(key, out var queue))
        {
            queue = new Queue<GameObject>();
            _pool[key] = queue;
        }

        go.SetActive(false);
        queue.Enqueue(go);
    }

    public async UniTask PreloadByLabelAsync(string label, Action<float> onProgress = null)
    {
        var locationHandle = Addressables.LoadResourceLocationsAsync(label);
        await locationHandle.Task;

        if (locationHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"[ResourceManager] Failed to load locations for label: {label}");
            return;
        }

        IList<IResourceLocation> locations = locationHandle.Result;
        int totalCount = locations.Count;

        if (totalCount == 0)
        {
            onProgress?.Invoke(1f);
            Addressables.Release(locationHandle);
            return;
        }

        for (int i = 0; i < totalCount; i++)
        {
            var loc = locations[i];

            // 이미 캐시에 있는 키면 스킵 (중복 로드 방지)
            string key = loc.PrimaryKey; // 보통 Addressables의 "주소" 문자열
            if (_prefabCache.ContainsKey(key))
            {
                float alreadyLoadedProgress = (float)(i + 1) / totalCount;
                onProgress?.Invoke(alreadyLoadedProgress);
                continue;
            }

            var handle = Addressables.LoadAssetAsync<UnityEngine.Object>(loc);

            // 각 에셋 로드 중에도 PercentComplete로 부드럽게 진행률 반영
            while (!handle.IsDone)
            {
                float baseProgress = (float)i / totalCount;
                float current = baseProgress + (handle.PercentComplete / totalCount);
                onProgress?.Invoke(current);
                await UniTask.Yield();
            }

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _handleCache[key] = handle;
                _prefabCache[key] = handle.Result;
            }
            else
            {
                Debug.LogError($"[ResourceManager] Failed to preload asset: {key}");
            }

            // 마지막 자원까지 끝났으면 거의 1.0에 근접
            float progress = (float)(i + 1) / totalCount;
            onProgress?.Invoke(progress);
        }

        // 다 끝나면 1.0으로 보정
        onProgress?.Invoke(1f);

        // locations 핸들은 해제해도 됨 (자원 자체는 _handleCache에 있는 handle들이 들고 있음)
        Addressables.Release(locationHandle);
    }

    public void Clear()
    {
        // 풀에 있는 오브젝트 삭제
        foreach (var pair in _pool)
        {
            foreach (var obj in pair.Value)
            {
                if (obj != null)
                    UnityEngine.Object.Destroy(obj);
            }
        }
        _pool.Clear();

        // Addressables 핸들 해제
        foreach (var handle in _handleCache.Values)
        {
            Addressables.Release(handle);
        }
        _handleCache.Clear();
        _prefabCache.Clear();
    }
}
