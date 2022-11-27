using System;

namespace CustomTools.ObjectPooling
{
    public interface IObjectPoolInitializable
    {
        void Init(object sender, PooledObjectInitializationArgs args);
        event EventHandler<PooledObjectInitializationArgs> OnPooledObjectInitializedEvent;
    }
}