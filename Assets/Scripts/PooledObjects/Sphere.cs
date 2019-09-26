using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : PooledObject
{
    public PooledObjectType Type;

    public override void Init()
    {
        base.Init();
    }

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
    }

    public override void OnObjectDespawn()
    {
        base.OnObjectDespawn();
    }

    public override void AddRandomForce()
    {
        base.AddRandomForce();
    }
}
