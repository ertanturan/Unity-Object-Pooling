using System;

namespace CustomTools.ObjectPooling
{
    public interface IObjectPoolInitializable
    {
        void Init(object sender, PooledObjectInitializationEventArgs args);
        event EventHandler<PooledObjectInitializationEventArgs> OnInitialisedEvent;
    }
}