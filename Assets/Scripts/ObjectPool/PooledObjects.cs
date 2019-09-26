using UnityEngine;

[System.Serializable]
public struct PoolObjects
{
    public PooledObjectType Tag;
    public GameObject Prefab;
    public int Size;
}
