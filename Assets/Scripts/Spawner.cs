using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public PooledObjectType Type;
    public float SpawnRate = 2f;

    private float _timer = 0;

    void Start()
    {
        _timer = SpawnRate;
    }

    void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            GameObject obj = ObjectPooler.Instance.SpawnFromPool(Type, transform.position, Random.rotation);
            obj.GetComponent<IPooledObject>().Init();
            obj.GetComponent<IPooledObject>().OnObjectSpawn();
            _timer = SpawnRate;
        }
    }
}
