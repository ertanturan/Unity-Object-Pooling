using UnityEngine;

public class PooledObject : MonoBehaviour, IPooledObject
{


    public PooledObjectType PoolType { get; set; }



    public virtual void OnObjectSpawn()
    {

    }

    public virtual void OnObjectDespawn()
    {

    }

    public virtual void Init()
    {

    }

    public void Despawn()
    {
        ObjectPooler.Instance.Despawn(gameObject);
    }


}
