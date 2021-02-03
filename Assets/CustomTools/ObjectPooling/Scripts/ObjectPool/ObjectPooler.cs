using System.Collections.Generic;
using UnityEngine;

namespace CustomTools.ObjectPooling.Scripts.ObjectPool
{
    public class ObjectPooler : MonoBehaviour
    {
        private Dictionary<PooledObjectType, Queue<GameObject>> _poolDictionary;
        [SerializeField] private List<PoolObjects> _pool;

        private Dictionary<PooledObjectType, int> _poolIndexes = new Dictionary<PooledObjectType, int>();
        private Dictionary<PooledObjectType, Transform> _poolMasters = new Dictionary<PooledObjectType, Transform>();

        private void Start()
        {
            _poolDictionary = new Dictionary<PooledObjectType, Queue<GameObject>>();

            GameObject master = new GameObject("Pool");

            for (int j = 0; j < _pool.Count; j++)
            {
                GameObject poolSpecifiMaster = new GameObject(_pool[j].Tag.ToString());
                poolSpecifiMaster.transform.parent = master.transform;

                Queue<GameObject> objectPool = new Queue<GameObject>();
                _poolIndexes.Add(_pool[j].Tag, j);

                _poolMasters.Add(_pool[j].Tag, poolSpecifiMaster.transform);

                for (int i = 0; i < _pool[j].Size; i++)
                {
                    GameObject obj = Instantiate(_pool[j].Prefab, poolSpecifiMaster.transform, true);
                    IPooledObject iPool = obj.GetComponent<IPooledObject>();
                    if (iPool == null)
                    {
                        PooledObject temp = obj.AddComponent<PooledObject>();
                        iPool = temp;
                    }
                    iPool.PoolType = _pool[j].Tag;


                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                _poolDictionary.Add(_pool[j].Tag, objectPool);
            }

        }

        public GameObject SpawnFromPool(PooledObjectType pooledObjectType, Vector3 pos, Quaternion rot, GameObject parent = null)
        {

            if (!_poolDictionary.ContainsKey(pooledObjectType))
            {
                Debug.LogWarning("PoolObjects with Tag " + pooledObjectType + " doesn't exist ..");
                return null;
            }

            GameObject objToSpawn;

            if (_poolDictionary[pooledObjectType].Count != 0)
            {
                objToSpawn = _poolDictionary[pooledObjectType].Peek();
                objToSpawn.SetActive(true);
                objToSpawn.transform.position = pos;
                objToSpawn.transform.rotation = rot;

                IPooledObject iPooledObj = objToSpawn.GetComponent<IPooledObject>();
                IObjectPoolInitializable iInitializable = objToSpawn.GetComponent<IObjectPoolInitializable>();

                iInitializable?.Init();

                // iPooledObj.Init(iPooledObj.ObjectPooler);
                iPooledObj.OnObjectSpawn();

                _poolDictionary[pooledObjectType].Dequeue();
            }
            else
            {
                objToSpawn = ExpandPool(pooledObjectType, pos, rot);
            }

            if (parent)
            {
                objToSpawn.transform.SetParent(parent.transform);
            }

            return objToSpawn;
        }

        public void Despawn(GameObject obj)
        {
            PooledObjectType pooledObjectType = obj.GetComponent<IPooledObject>().PoolType;

            if (_poolDictionary.ContainsKey(pooledObjectType) &&  // check if there's a queued objects by that tag.
                _poolDictionary[pooledObjectType].Contains(gameObject)) // check if `obj` is already despawned
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

        private GameObject ExpandPool(PooledObjectType pooledObjectType, Vector3 pos, Quaternion rot)
        {
            int index = _poolIndexes[pooledObjectType];
            GameObject objToAdd = Instantiate(_pool[index].Prefab, _poolMasters[pooledObjectType], true);
            objToAdd.SetActive(true);

            objToAdd.transform.position = pos;
            objToAdd.transform.rotation = rot;

            IPooledObject iPool = objToAdd.GetComponent<IPooledObject>();
            if (iPool == null)
            {
                PooledObject tempPool = objToAdd.AddComponent<PooledObject>();
                iPool = tempPool;
            }

            iPool.PoolType = pooledObjectType;

            IPooledObject iPooledObj = objToAdd.GetComponent<IPooledObject>();
            IObjectPoolInitializable iInitializable = objToAdd.GetComponent<IObjectPoolInitializable>();

            iInitializable?.Init();
            
            iPooledObj.OnObjectSpawn();


            _poolDictionary[pooledObjectType].Enqueue(objToAdd);
            _poolDictionary[pooledObjectType].Dequeue();

            _pool[index].Size++;

            return objToAdd;
        }

    }
}