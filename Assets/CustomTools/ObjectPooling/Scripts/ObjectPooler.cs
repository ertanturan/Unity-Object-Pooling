namespace CustomTools.ObjectPooling
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ObjectPooler : MonoBehaviour
    {
        [SerializeField] private List<PoolObjects> _pool;

        private readonly Dictionary<PooledObjectType, int> _poolIndexes = new();
        private readonly Dictionary<PooledObjectType, Transform> _poolMasters = new();
        private Dictionary<PooledObjectType, Queue<GameObject>> _poolDictionary;

        private void Start()
        {
            _poolDictionary = new Dictionary<PooledObjectType, Queue<GameObject>>();

            GameObject master = new("Pool");

            for (int j = 0; j < _pool.Count; j++)
            {
                GameObject poolSpecifiMaster = new(_pool[j].Tag.ToString());
                poolSpecifiMaster.transform.parent = master.transform;

                Queue<GameObject> objectPool = new();
                _poolIndexes.Add(_pool[j].Tag, j);

                _poolMasters.Add(_pool[j].Tag, poolSpecifiMaster.transform);
                _poolDictionary.Add(_pool[j].Tag, objectPool);

                for (int i = 0; i < _pool[j].Size; i++)
                {
                    AddNewInstanceToObjectPool(_pool[j].Tag);
                }
            }
        }

        /// <summary>
        ///     Spawns a gameobject from pool if there's any. If not, it'll expand the pool size and then spawn.
        /// </summary>
        /// <param name="pooledObjectType"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public GameObject SpawnFromPool(PooledObjectType pooledObjectType, Vector3 position, Quaternion rotation,
            Transform parent = null, PooledObjectInitializationArgs args = null)
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
                objToSpawn.transform.position = position;
                objToSpawn.transform.rotation = rotation;

                IPooledObject iPooledObj = objToSpawn.GetComponent<IPooledObject>();

                IObjectPoolInitializable iInitializable = objToSpawn.GetComponent<IObjectPoolInitializable>();

                if (args != null && iInitializable != null)
                {
                    iInitializable.Init(this, args);
                }


                iPooledObj.OnObjectSpawn();

                _poolDictionary[pooledObjectType].Dequeue();
            }
            else
            {
                return ExpandAndSpawnFromPool(pooledObjectType, position, rotation, parent);
            }

            if (parent)
            {
                objToSpawn.transform.SetParent(parent);
            }

            return objToSpawn;
        }

        /// <summary>
        ///     If pool size is not catching up to the spawn calls then this function will be called by "SpawnFromPool" to expand
        ///     the pool
        /// </summary>
        /// <param name="pooledObjectType"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private GameObject ExpandAndSpawnFromPool(PooledObjectType pooledObjectType, Vector3 position,
            Quaternion rotation, Transform parent = null)
        {
            AddNewInstanceToObjectPool(pooledObjectType);

            int index = _poolIndexes[pooledObjectType];

            _pool[index].Size++;
            return SpawnFromPool(pooledObjectType, position, rotation, parent);
        }

        /// <summary>
        ///     Returns the pooled object back to pool.
        /// </summary>
        /// <param name="obj"></param>
        public void Despawn(GameObject obj)
        {
            PooledObjectType pooledObjectType = obj.GetComponent<IPooledObject>().PoolType;

            bool isThereAnyQueuedObjectByTheTag = _poolDictionary.ContainsKey(pooledObjectType);

            bool isObjectAlreadyDespawned = _poolDictionary[pooledObjectType].Contains(gameObject);


            if (isThereAnyQueuedObjectByTheTag && !isObjectAlreadyDespawned)
            {
                _poolDictionary[pooledObjectType].Enqueue(obj);

                IPooledObject iPooledObj = obj.GetComponent<IPooledObject>();
                if (iPooledObj != null)
                {
                    iPooledObj.OnObjectDespawn();
                }

                obj.transform.SetParent(_poolMasters[pooledObjectType]);
                obj.SetActive(false);
            }
            else
            {
                Debug.LogError("Trying to despawn object which is not pooled or object is already despawned !");
            }
        }


        /// <summary>
        ///     Creates  a new gameobject and registers it to the pool
        /// </summary>
        /// <param name="pooledObjectType"></param>
        private void AddNewInstanceToObjectPool(PooledObjectType pooledObjectType)
        {
            Queue<GameObject> objectPool = _poolDictionary[pooledObjectType];

            Transform poolSpecifiMaster = _poolMasters[pooledObjectType];
            int index = _poolIndexes[pooledObjectType];
            GameObject prefab = _pool[index].Prefab;

            GameObject freshGameObject = Instantiate(prefab, poolSpecifiMaster, true);
            IPooledObject iPool = freshGameObject.GetComponent<IPooledObject>();
            if (iPool == null)
            {
                PooledObject temp = freshGameObject.AddComponent<PooledObject>();
                iPool = temp;
            }

            iPool.PoolType = pooledObjectType;


            freshGameObject.SetActive(false);
            objectPool.Enqueue(freshGameObject);
        }
    }
}