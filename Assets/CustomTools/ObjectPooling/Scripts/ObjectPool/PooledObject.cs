using CustomTools.ObjectPooling.Scripts.ObjectPool;
using UnityEngine;
using Zenject;

public class PooledObject : MonoBehaviour, IPooledObject
{

    public PooledObjectType PoolType { get; set; }
    public ObjectPooler Pooler { get; private set; }

    [Inject]
    public virtual void Construct(ObjectPooler pooler)
    {
        Debug.Log("construct");
        
        Debug.Log(pooler);
        Pooler = pooler;
    }
    
    public virtual void OnObjectSpawn()
    {

    }

    public virtual void OnObjectDespawn()
    {

    }

    public void Despawn()
    {
        Pooler.Despawn(gameObject);
    }


}
