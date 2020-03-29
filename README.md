# Runtime-extendible Object-pooler within unity

This pool creates objects that you defined on start and creates additionally if more spawn request comes to the pool than pools capacity and manages newly added pool objects too ! .


## Import

1. Go to [release](https://github.com/ertanturan/UnityObjectPooling/releases) page.
2. Download the lates release of the package.
3. Import it to your unity project.

## Usage

1. Head to your `hierarchy >> Right Click >> Create Empty`
2. Add Component >> Object Pooler >> Set size that you wish to add to your project.
3. Create Script which inherits from PooledObject Class 

`` public Class ExampleClass : PooledObject``

4. Add a type to `enum PooledObjectType` (Can be found under `Assets/Scripts/ObjectPool/PoolderObjectType.cs`)
5. Retrun to the game object you created on hierarchy  drag and drop your prefab to Prefab property on objectpooler's pool . Set the tag you wrote on step 4. set size (How many object you want to create on beginning).


### To Spawn and Despawn

`` ObjectPooler.Instance.SpawnFromPool(PooledObjectType.YourTypeComesHere , transform.position, Random.rotation);  ``

Attention ! : Despawn should be called from the pooled object .
`` ObjectPooler.Instance.Despawn(Type,gameObject); ``


## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.


# ENJOY !
