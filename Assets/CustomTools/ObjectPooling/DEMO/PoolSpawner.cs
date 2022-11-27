using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CustomTools.ObjectPooling.DEMO
{
    public class PoolSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPosition;

        private readonly List<IPooledObject> _cubes = new();
        private ObjectPooler _pooler;

        [Inject]
        private void Construct(ObjectPooler pooler)
        {
            _pooler = pooler;
        }


        public void SpawnCube()
        {
            var obj = _pooler.SpawnFromPool(PooledObjectType.Cube,
                _spawnPosition.position, Quaternion.identity);

            _cubes.Add(obj.GetComponent<IPooledObject>());
        }

        public void DespawnAnyCube()
        {
            if (_cubes.Count > 0)
            {
                _cubes[0].Despawn();
                _cubes.RemoveAt(0);
            }
        }
    }
}