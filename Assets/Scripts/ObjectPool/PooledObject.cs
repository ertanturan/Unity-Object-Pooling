using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour, IPooledObject
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
        AddRandomForce();
    }

    public virtual void AddRandomForce()
    {
        int value = 300;
        Vector3 random =
            new Vector3(Random.Range(-value, value),
                Random.Range(-value, value),
                Random.Range(-value, value));

        _rb.AddForce(random);
    }

 }
