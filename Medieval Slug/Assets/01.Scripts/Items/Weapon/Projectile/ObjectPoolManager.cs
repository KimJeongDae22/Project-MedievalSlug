using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    [SerializeField] private GameObject[] prefabs;
    private Dictionary<int, Queue<GameObject>> pools = new Dictionary<int, Queue<GameObject>>();

    void Awake()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            pools[i] = new Queue<GameObject>();
        }
    }

    public GameObject GetObject(int prefabIndex, Vector2 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(prefabIndex))
        {
            Debug.Log($"prefabIndex[{prefabIndex}]에 대한 Pool이 존재하지 않습니다.");
            return null;
        }

        GameObject obj;
        if (pools[prefabIndex].Count > 0)
        {
            obj = pools[prefabIndex].Dequeue();
        }
        else
        {
            obj = Instantiate(prefabs[prefabIndex]);
            obj.GetComponent<IPoolable>()?.Initialize(o => ReturnObject(prefabIndex, o));
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        obj.GetComponent<IPoolable>()?.OnSpawn();
        return obj;
    }

    public void ReturnObject(int prefabIndex, GameObject obj)
    {
        if (!pools.ContainsKey(prefabIndex))
        {
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        pools[prefabIndex].Enqueue(obj);
    }
}
