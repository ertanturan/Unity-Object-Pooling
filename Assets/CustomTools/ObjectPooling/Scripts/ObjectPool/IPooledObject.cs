using Zenject;

namespace CustomTools.ObjectPooling.Scripts.ObjectPool
{
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
}
