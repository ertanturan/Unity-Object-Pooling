using CustomTools.ObjectPooling.Scripts.ObjectPool;
using UnityEngine;
using Zenject;

public class PooledObject : MonoBehaviour, IPooledObject
{

    public PooledObjectType PoolType { get; set; }
    public ObjectPooler ObjectPooler;

    [Inject]
    public virtual void Construct(ObjectPooler pooler)
    {
        Debug.Log("construct");
        
        Debug.Log(pooler);
        ObjectPooler = pooler;
    }
    
    public virtual void OnObjectSpawn()
    {

    }

    public virtual void OnObjectDespawn()
    {

    }

    public void Despawn()
    {
        ObjectPooler.Despawn(gameObject);
    }


}
