using UnityEngine;

namespace CustomTools.ObjectPooling
{
    [System.Serializable]
    public class PoolObjects
    {
        public PooledObjectType Tag;
        public GameObject Prefab;
        public int Size;
    }
}