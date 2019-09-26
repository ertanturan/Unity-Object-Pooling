using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour,IPooledObject
{
    private Rigidbody _rb;
    public virtual void OnObjectSpawn()
    {
        _rb.velocity = Vector3.zero;
    }

    public virtual void OnObjectDespawn()
    {

    }

    public virtual void Init()
    {
        _rb = GetComponent<Rigidbody>();
    }
}
