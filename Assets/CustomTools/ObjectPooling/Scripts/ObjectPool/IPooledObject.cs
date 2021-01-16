using CustomTools.ObjectPooling.Scripts.ObjectPool;
using UnityEngine;

public interface IPooledObject
{
    PooledObjectType PoolType { get; set; }


    void Construct(ObjectPooler pooler);
    void OnObjectSpawn();
    void OnObjectDespawn();
    void Despawn();
}
