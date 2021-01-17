using CustomTools.ObjectPooling.Scripts.ObjectPool;
using UnityEngine;
using Zenject;

public class DemoSceneInstaller : MonoInstaller
{
    [SerializeField] private GameObject _objectPool;

    public override void InstallBindings()
    {
        Container.Bind<ObjectPooler>().FromComponentOn(_objectPool).AsSingle().NonLazy();
    }
}