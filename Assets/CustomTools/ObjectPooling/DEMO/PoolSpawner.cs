using System.Collections.Generic;
using CustomTools.ObjectPooling.Scripts.ObjectPool;
using UnityEngine;

public class PoolSpawner : MonoBehaviour
{

    private List<IPooledObject> _cubes = new List<IPooledObject>();

    [SerializeField] private Transform _spawnPosition;
    
    public void SpawnCube()
    {
        GameObject obj = ObjectPooler.Instance.SpawnFromPool(PooledObjectType.Cube,
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
