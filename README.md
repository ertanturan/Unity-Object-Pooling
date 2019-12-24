# Runtime-extendible Object-pooler within unity

You may have made games in the past that created and destroyed a lot of items (such as bullets or enemies).
What you may not have known is that the act of instantiating and destroying are inefficient and can slow your projects down.
This is where Object-pooling comes in . This project contains fully finished example of a object-pooler . 

This pool creates objects that you defined on start and creates additionally if more spawn request comes to the pool than pools capacity and manages newly added pool objects too ! .


## Import

1. Go to [release](https://github.com/ertanturan/UnityObjectPooling/releases) page.
2. Download the lates release of the package.
3. Import it to your unity project.

## Installation


## Usage

1. Head to your hierarchy >> Right Click >> Create Empty
2. Add Component >> Object Pooler >> Set size that you wish to add to your project.
3. Create Script which inherits from PooledObject Class 

`` public Class ExampleClass : PooledObject``

4. Add a type to 'enum PooledObjectType' (Can be found under Assets/Scripts/ObjectPool/PoolderObjectType.cs)
5. Retrun to the game object you created on hierarchy  drag and drop your prefab to Prefab property on objectpooler's pool . Set the tag you wrote on step 4. set size (How many object you want to create on beginning).


### To Spawn and Despawn

`` ObjectPooler.Instance.SpawnFromPool(PooledObjectType.YourTypeComesHere , transform.position, Random.rotation);  ``

`` ObjectPooler.Instance.Despawn(Type,gameObject); ``

## Demo Project

Demo project can be found under 'Assets/Scenes' folder named ' Main '

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.


# ENJOY !
