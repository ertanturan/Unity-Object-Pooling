using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
  

    public Dictionary<string, Queue<GameObject>> PoolDictionary;
    public List<PoolObjects> Pool;

    #region Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        GameObject master = new GameObject("Pool");

        foreach (PoolObjects pool in Pool)
        {
            GameObject poolSpecifiMaster = new GameObject(pool.Tag.ToString());
            poolSpecifiMaster.transform.parent = master.transform;

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab);
                obj.transform.parent = poolSpecifiMaster.transform;

                if(obj.GetComponent<IPooledObject>()==null)
                {
                    obj.AddComponent<PooledObject>();
                }

                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            PoolDictionary.Add(pool.Tag.ToString(), objectPool);

        }
    }

    public GameObject SpawnFromPool(PooledObjectType tag, Vector3 pos, Quaternion rot)
    {
        if (!PoolDictionary.ContainsKey(tag.ToString())) { Debug.LogWarning("PoolObjects with Tag " + tag + " doesn't exist .."); return null; }

        GameObject objToSpawn = PoolDictionary[tag.ToString()].Dequeue();
        objToSpawn.SetActive(true);
        objToSpawn.transform.position = pos;
        objToSpawn.transform.rotation = rot;

        IPooledObject iPooledObj = objToSpawn.GetComponent<IPooledObject>();

        iPooledObj.OnObjectSpawn();

        PoolDictionary[tag.ToString()].Enqueue(objToSpawn);
        return objToSpawn;
    }

    public void Despawn(PooledObjectType tag)
    {
        GameObject objToDespawn = PoolDictionary[tag.ToString()].Dequeue();
        objToDespawn.SetActive(false);
        objToDespawn.transform.position = Vector3.zero;
        IPooledObject iPooledObj = objToDespawn.GetComponent<IPooledObject>();
        if (iPooledObj!=null) iPooledObj.OnObjectDespawn();
    
    }

}