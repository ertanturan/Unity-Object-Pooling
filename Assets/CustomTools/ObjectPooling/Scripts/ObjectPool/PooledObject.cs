using CustomTools.ObjectPooling.Scripts.ObjectPool;
using UnityEngine;
using Zenject;

public class PooledObject : MonoBehaviour, IPooledObject
{
    private ObjectPooler _objectPooler;


    public PooledObjectType PoolType { get; set; }
    public ObjectPooler ObjectPooler { get; set; }


    public virtual void Construct(ObjectPooler pooler)
    {
        _objectPooler = pooler;
    }
    
    public virtual void OnObjectSpawn()
    {

    }

    public virtual void OnObjectDespawn()
    {

    }

    // [Inject]
    

    public void Despawn()
    {
        _objectPooler.Despawn(gameObject);
    }


}
