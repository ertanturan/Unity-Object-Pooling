using UnityEngine;
using Zenject;

namespace CustomTools.ObjectPooling.DEMO
{
    public class DemoSceneInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _objectPool;

        public override void InstallBindings()
        {
            Container.Bind<ObjectPooler>().FromComponentOn(_objectPool).AsSingle().NonLazy();
        }
    }
}