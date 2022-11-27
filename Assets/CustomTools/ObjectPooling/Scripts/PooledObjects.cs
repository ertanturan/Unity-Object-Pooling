using System;
using UnityEngine;

namespace CustomTools.ObjectPooling
{
    [Serializable]
    public class PoolObjects
    {
        public PooledObjectType Tag;
        public GameObject Prefab;
        public int Size;
    }
}