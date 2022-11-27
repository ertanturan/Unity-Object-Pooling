using System.Collections.Generic;
using UnityEngine;

namespace CustomTools.ObjectPooling
{
    public class ObjectPooler : MonoBehaviour
    {
        [SerializeField] private List<PoolObjects> _pool;

        private readonly Dictionary<PooledObjectType, int> _poolIndexes = new();
        private readonly Dictionary<PooledObjectType, Transform> _poolMasters = new();
        private Dictionary<PooledObjectType, Queue<GameObject>> _poolDictionary;

        private void Start()
        {
            _poolDictionary = new Dictionary<PooledObjectType, Queue<GameObject>>();

            var master = new GameObject("Pool");

            for (var j = 0; j < _pool.Count; j++)
            {
                var poolSpecificMaster = new GameObject(_pool[j].Tag.ToString());
                poolSpecificMaster.transform.parent = master.transform;

                var objectPool = new Queue<GameObject>();
                _poolIndexes.Add(_pool[j].Tag, j);

                _poolMasters.Add(_pool[j].Tag, poolSpecificMaster.transform);

                for (var i = 0; i < _pool[j].Size; i++)
                {
                    var obj = Instantiate(_pool[j].Prefab, poolSpecificMaster.transform, true);
                    var iPool = obj.GetComponent<IPooledObject>();
                    if (iPool == null)
                    {
                        var temp = obj.AddComponent<PooledObject>();
                        iPool = temp;
                    }

                    iPool.PoolType = _pool[j].Tag;


                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                _poolDictionary.Add(_pool[j].Tag, objectPool);
            }
        }

        public GameObject SpawnFromPool(PooledObjectType pooledObjectType, Vector3 pos, Quaternion rot,
            GameObject parent = null, PooledObjectInitializationEventArgs initializationEventArgs = null)
        {
            if (!_poolDictionary.ContainsKey(pooledObjectType))
            {
                Debug.LogWarning(string.Concat("PoolObjects with Tag ", pooledObjectType, " doesn't exist .."));
                return null;
            }

            GameObject objToSpawn;

            if (_poolDictionary[pooledObjectType].Count != 0)
            {
                objToSpawn = _poolDictionary[pooledObjectType].Peek();
                objToSpawn.SetActive(true);
                objToSpawn.transform.position = pos;
                objToSpawn.transform.rotation = rot;

                var iPooledObj = objToSpawn.GetComponent<IPooledObject>();
                var iInitializable = objToSpawn.GetComponent<IObjectPoolInitializable>();

                iInitializable?.Init(this, initializationEventArgs);

                // iPooledObj.Init(iPooledObj.ObjectPooler);
                iPooledObj.OnObjectSpawn();

                _poolDictionary[pooledObjectType].Dequeue();
            }
            else
                objToSpawn = ExpandPool(pooledObjectType, pos, rot);

            if (parent) objToSpawn.transform.SetParent(parent.transform);

            return objToSpawn;
        }

        public void Despawn(GameObject obj)
        {
            var pooledObjectType = obj.GetComponent<IPooledObject>().PoolType;

            var isThereAnyQueuedObjectByTheTag = _poolDictionary.ContainsKey(pooledObjectType);

            var isObjectAlreadyDespawned = _poolDictionary[pooledObjectType].Contains(gameObject);


            if (isThereAnyQueuedObjectByTheTag && !isObjectAlreadyDespawned)
            {
                _poolDictionary[pooledObjectType].Enqueue(obj);

                var iPooledObj = obj.GetComponent<IPooledObject>();
                if (iPooledObj != null) iPooledObj.OnObjectDespawn();

                obj.transform.SetParent(_poolMasters[pooledObjectType]);
                obj.SetActive(false);
            }
            else
                Debug.LogError("Trying to despawn object which is not pooled or object is already despawned !");
        }

        private GameObject ExpandPool(PooledObjectType pooledObjectType, Vector3 pos, Quaternion rot)
        {
            var index = _poolIndexes[pooledObjectType];
            var objToAdd = Instantiate(_pool[index].Prefab, _poolMasters[pooledObjectType], true);
            objToAdd.SetActive(true);

            objToAdd.transform.position = pos;
            objToAdd.transform.rotation = rot;

            var iPool = objToAdd.GetComponent<IPooledObject>();
            if (iPool == null)
            {
                var tempPool = objToAdd.AddComponent<PooledObject>();
                iPool = tempPool;
            }

            iPool.PoolType = pooledObjectType;

            var iPooledObj = objToAdd.GetComponent<IPooledObject>();
            var iInitializable = objToAdd.GetComponent<IObjectPoolInitializable>();

            iInitializable?.Init();

            iPooledObj.OnObjectSpawn();


            _poolDictionary[pooledObjectType].Enqueue(objToAdd);
            _poolDictionary[pooledObjectType].Dequeue();

            _pool[index].Size++;

            return objToAdd;
        }
    }
}