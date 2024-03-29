# Runtime-extendible Object-pooler within unity

This pool creates objects that you defined on start and creates additionally if more spawn request comes to the pool than pools capacity and manages newly added pool objects too ! .

## Pre-requisit(s)

- Zenject IOC (Find [HERE](https://github.com/modesttree/Zenject)  )

## Import

1. Go to [release](https://github.com/ertanturan/UnityObjectPooling/releases) page.
2. Download the lates release of the package.
3. Import it to your unity project.

## Usage

1. Add ObjectPooler component to an object
2. Set a size for the pool.
3. Create prefabs to pool 
4. Edit PooledObjectType.cs class according to your prefabs which you want to pool
5. Go to the scene object with ObjectPooler component and assign prefabs and enums and set size for each pooled object.
6. Ready to go !

### To Spawn and Despawn

#### Inject the pooler to your script
(This part assumes you've set-up your zenject installer and contexts)

```csharp 
[Inject]
private ObjectPooler _pooler;
```
#### Spawn
```csharp  
_pooler.SpawnFromPool(PooledObjectType.YourTypeComesHere , YourVector3PositionComesHere, YourQuaternionRotationComesHere,Optional_YourParentTransformComesHere,Optional_PooledObjectInitializationArgsComesHere);  
```
#### Despawn

```csharp 
_pooler.Despawn(gameObject); 
```


## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

# Credits

- [Zenject](https://github.com/modesttree/Zenject) - used for dependency injection

I would like to extend a special thank you to the creators and maintainers of Zenject for their excellent work on this dependency injection framework. Their contributions have greatly improved the functionality and usability of this project.


# ENJOY !
