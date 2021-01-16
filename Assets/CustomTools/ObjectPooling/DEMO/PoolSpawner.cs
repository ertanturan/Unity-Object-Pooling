using System.Collections;
using System.Collections.Generic;
using CustomTools.ObjectPooling.Scripts.ObjectPool;
using UnityEngine;
using Zenject;

public class PoolSpawner : MonoBehaviour
{

    private List<IPooledObject> _cubes = new List<IPooledObject>();
    private ObjectPooler _pooler;

    [SerializeField] private Transform _spawnPosition;
    
    [Inject]
    private void Construct(ObjectPooler pooler)
    {
        _pooler = pooler;
    }
    
    
    public void SpawnCube()
    {
        GameObject obj = _pooler.SpawnFromPool(PooledObjectType.Cube,
            _spawnPosition.position, Quaternion.identity);
        
        _cubes.Add(obj.GetComponent<IPooledObject>());
    }

    public void DespawnAnyCube()
    {
        if (_cubes.Count>0)
        {

            _cubes[0].Despawn();
            _cubes.RemoveAt(0);
        }
    }
    
}
