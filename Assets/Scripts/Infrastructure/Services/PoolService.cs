using System.Collections.Generic;
using Client.Data.Core;
using UnityEngine;
using Object = UnityEngine.Object;

public class PoolService
{
    private readonly SharedData _data;
    private PrefabFactory _prefabFactory;

    private Dictionary<GameObject, Queue<GameObject>> _pools = new Dictionary<GameObject, Queue<GameObject>>();
    private Dictionary<GameObject, Transform> _poolParents = new Dictionary<GameObject, Transform>();

    public PoolService(SharedData data)
    {
        _data = data;
        InitPools();
    }

    private void InitPools()
    {
        foreach (var prefab in _data.StaticData.PrefabData.PoolPrefabData)
        {
            GameObject poolParent = new GameObject();
            poolParent.name = $"Pool - [{prefab.Key.name}]";

            _pools.Add(prefab.Key, new Queue<GameObject>());
            _poolParents.Add(prefab.Key, poolParent.transform);

            for (var i = 0; i < prefab.Value; i++)
            {
                GameObject poolObject = Object.Instantiate(prefab.Key, Vector3.one * 100.0f, Quaternion.identity, _poolParents[prefab.Key]);
                poolObject.AddComponent<PoolObject>().PrefabRef = prefab.Key;
                poolObject.SetActive(false);
                _pools[prefab.Key].Enqueue(poolObject);
            }
        }
    }

    public bool IsPrefabHavePool(GameObject neededPrefab)
    {
        return _pools.ContainsKey(neededPrefab);
    }

    public bool IsGameObjectHavePool(GameObject go)
    {
        if (go != null && go.TryGetComponent(out PoolObject poolObject))
            return _pools.ContainsKey(poolObject.PrefabRef);

        return false;
    }

    private bool IsPrefabHaveFreeObjectsInPool(GameObject neededPrefab)
    {
        return _pools[neededPrefab].Count > 0;
    }

    public GameObject GetGameObjectFromPool(GameObject neededPrefab)
    {
        foreach (var prefab in _data.StaticData.PrefabData.PoolPrefabData)
        {
            if (neededPrefab != prefab.Key) // in this pool check
                continue;

            if (!IsPrefabHaveFreeObjectsInPool(neededPrefab))
                CreateNewObjectInPool(neededPrefab);

            GameObject go = _pools[neededPrefab].Dequeue();
            go.transform.SetParent(null);
            go.SetActive(true);

            return go;
        }

        return null;
    }

    public void PutGameObjectToPool(GameObject storingGo)
    {
        if (storingGo.TryGetComponent(out PoolObject poolObject))
        {
            storingGo.transform.SetParent(_poolParents[poolObject.PrefabRef]);
            _pools[poolObject.PrefabRef].Enqueue(storingGo);
        }
    }

    private void CreateNewObjectInPool(GameObject neededPrefab)
    {
        GameObject poolObject = Object.Instantiate(neededPrefab, Vector3.one * 100.0f, Quaternion.identity, _poolParents[neededPrefab]);
        poolObject.AddComponent<PoolObject>().PrefabRef = neededPrefab;
        _pools[neededPrefab].Enqueue(poolObject);
    }
}

internal class PoolObject : MonoBehaviour
{
    public GameObject PrefabRef;
}