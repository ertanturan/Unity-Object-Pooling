namespace CustomTools.ObjectPooling
{
    using System;
    using UnityEngine;

    [Serializable]
    public class PoolObjects
    {
        public PooledObjectType Tag;
        public GameObject Prefab;
        public int Size;
        public bool IsExpandable = true;
    }
}