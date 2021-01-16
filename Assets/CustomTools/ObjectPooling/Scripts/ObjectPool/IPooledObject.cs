using CustomTools.ObjectPooling.Scripts.ObjectPool;
using UnityEngine;
using Zenject;

public interface IPooledObject
{
    PooledObjectType PoolType { get; set; }

     ObjectPooler Pooler { get; }
     
     [Inject]
    void Construct(ObjectPooler pooler);
    void OnObjectSpawn();
    void OnObjectDespawn();
    void Despawn();
}
