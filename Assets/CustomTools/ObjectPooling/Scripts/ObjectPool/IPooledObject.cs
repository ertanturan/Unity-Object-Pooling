public interface IPooledObject
{
    PooledObjectType PoolType { get; set; }
    void Init();
    void OnObjectSpawn();
    void OnObjectDespawn();
    void Despawn();
}
