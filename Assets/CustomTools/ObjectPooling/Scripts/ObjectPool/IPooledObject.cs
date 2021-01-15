using CustomTools.ObjectPooling.Scripts.ObjectPool;

public interface IPooledObject
{
    PooledObjectType PoolType { get; set; }
     // ObjectPooler ObjectPooler { get;  set; }
    void Construct(ObjectPooler pooler);
    void OnObjectSpawn();
    void OnObjectDespawn();
    void Despawn();
}
