namespace CustomTools.ObjectPooling
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class ObjectPooler : MonoBehaviour
    {
        [SerializeField] private List<PoolObjects> _pool;

        private readonly Dictionary<PooledObjectType, int> _poolIndexes = new();
        private readonly Dictionary<PooledObjectType, Transform> _poolMasters = new();

        private Dictionary<PooledObjectType, List<GameObject>> _allPooledObjects;
        private Dictionary<PooledObjectType, Queue<GameObject>> _poolQueue;

        private void Start()
        {
            _poolQueue = new Dictionary<PooledObjectType, Queue<GameObject>>();

            GameObject master = new("Pool");

            for (int j = 0; j < _pool.Count; j++)
            {
                GameObject poolSpecifiMaster = new(_pool[j].Tag.ToString());
                poolSpecifiMaster.transform.parent = master.transform;

                Queue<GameObject> objectPool = new();
                _poolIndexes.Add(_pool[j].Tag, j);

                _poolMasters.Add(_pool[j].Tag, poolSpecifiMaster.transform);
                _poolQueue.Add(_pool[j].Tag, objectPool);

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
        /// <param name="targetPosition"></param>
        /// <param name="targetRotation"></param>
        /// <param name="targetParent"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public GameObject SpawnFromPool(PooledObjectType pooledObjectType, Vector3 targetPosition,
            Quaternion targetRotation,
            Transform targetParent = null, PooledObjectInitializationArgs args = null)
        {
            if (!_poolQueue.ContainsKey(pooledObjectType))
            {
                Debug.LogWarning(string.Concat("PoolObjects with Tag ", pooledObjectType, " doesn't exist .."));
                return null;
            }

            GameObject objToSpawn;

            if (_poolQueue[pooledObjectType].Count != 0)
            {
                objToSpawn = _poolQueue[pooledObjectType].Peek();
                objToSpawn.SetActive(true);

                if (targetParent)
                {
                    objToSpawn.transform.SetParent(targetParent);
                }

                RectTransform rect = objToSpawn.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.anchoredPosition = targetPosition;
                }
                else
                {
                    objToSpawn.transform.position = targetPosition;
                    objToSpawn.transform.rotation = targetRotation;
                }

                IPooledObject iPooledObj = objToSpawn.GetComponent<IPooledObject>();

                IObjectPoolInitializable initializable = objToSpawn.GetComponent<IObjectPoolInitializable>();

                if (args != null && initializable != null)
                {
                    initializable.Init(this, args);
                }


                iPooledObj.OnObjectSpawn();

                _poolQueue[pooledObjectType].Dequeue();
            }
            else
            {
                int index = _poolIndexes[pooledObjectType];

                //if object is set as expandable then expand the pool.
                if (_pool[index].IsExpandable)
                {
                    return ExpandAndSpawnFromPool(pooledObjectType, targetPosition, targetRotation, targetParent);
                }

                //if pool is not expandable then de-queue one from the scene and spawn it to the location where user wants it. 
                // so this way we can keep the size of the pool for that type unchanged.
                Despawn(_poolQueue[pooledObjectType].First().gameObject);
                return SpawnFromPool(pooledObjectType, targetPosition, targetRotation, targetParent, args);
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
            int index = _poolIndexes[pooledObjectType];

            AddNewInstanceToObjectPool(pooledObjectType);


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

            bool isThereAnyQueuedObjectByTheTag = _poolQueue.ContainsKey(pooledObjectType);

            bool isObjectAlreadyDespawned = _poolQueue[pooledObjectType].Contains(gameObject);


            if (isThereAnyQueuedObjectByTheTag && !isObjectAlreadyDespawned)
            {
                _poolQueue[pooledObjectType].Enqueue(obj);

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
        ///     Creates  a new game object and registers it to the pool
        /// </summary>
        /// <param name="pooledObjectType"></param>
        private void AddNewInstanceToObjectPool(PooledObjectType pooledObjectType)
        {
            Queue<GameObject> objectPool = _poolQueue[pooledObjectType];

            Transform poolSpecifiMaster = _poolMasters[pooledObjectType];
            int index = _poolIndexes[pooledObjectType];
            PoolObjects pooledObject = _pool[index];
            GameObject prefab = pooledObject.Prefab;

            GameObject freshGameObject = Instantiate(prefab, poolSpecifiMaster, true);
            IPooledObject iPool = freshGameObject.GetComponent<IPooledObject>();
            if (iPool == null)
            {
                PooledObject temp = freshGameObject.AddComponent<PooledObject>();
                iPool = temp;
            }

            iPool.PoolType = pooledObjectType;

            if (!_allPooledObjects.ContainsKey(pooledObjectType))
            {
                _allPooledObjects.Add(pooledObjectType, new List<GameObject>(pooledObject.Size));
            }

            _allPooledObjects[pooledObjectType].Add(freshGameObject);

            freshGameObject.SetActive(false);
            objectPool.Enqueue(freshGameObject);
        }
    }
}