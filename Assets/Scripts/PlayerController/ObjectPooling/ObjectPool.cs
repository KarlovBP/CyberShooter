using UnityEngine;
using System.Collections.Generic;

public class ObjectPool
{
    private int PoolSize;
    private GameObject SampleObject;
    Stack<GameObject> UnusedObjects;
    Dictionary<int, GameObject> UsedObjects;

    public ObjectPool(GameObject sampleObject, int poolSize)
    {
        IPoolable PoolComponent = sampleObject.GetComponent(typeof(IPoolable)) as IPoolable;
        if (PoolComponent == null)
        {
            throw new System.ArgumentException("Passed sample object does not contain component that implements IPoolable!", "sampleObject");
        }
        //instantiate a new list of game objects to store our pooled objects in.
        PoolSize = poolSize;
        SampleObject = sampleObject;
        UnusedObjects = new Stack<GameObject>(poolSize);
        UsedObjects = new Dictionary<int, GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            PoolComponent.PoolKey = GenerateKeyForDictionary();
            UnusedObjects.Push(Object.Instantiate(sampleObject) as GameObject);
            Object.DontDestroyOnLoad(UnusedObjects.Peek());
        }
    }

    public GameObject Instantiate()
    {
        GameObject ReusedObject;
        IPoolable PoolComponent;
        if (UnusedObjects.Count > 0)
        {
            ReusedObject = UnusedObjects.Pop();
        }
        else
        {
            //We're out of unused objects, so we will reuse random one already in use
            ReusedObject = System.Linq.Enumerable.ToList(UsedObjects.Values)[Random.Range(0, UsedObjects.Count)];
        }
        PoolComponent = ReusedObject.GetComponent(typeof(IPoolable)) as IPoolable;
        UsedObjects[PoolComponent.PoolKey] = ReusedObject;
        ReusedObject.SetActive(true);
        PoolComponent.Initialize();
        return ReusedObject;
    }

    public bool Destroy (GameObject pooledObject)
    {
        IPoolable PoolComponent = pooledObject.GetComponent(typeof(IPoolable)) as IPoolable;
        if (!UsedObjects.Remove(PoolComponent.PoolKey))
        {
            return false;
        }
        UnusedObjects.Push(pooledObject);
        PoolComponent.Reset();
        pooledObject.SetActive(false);
        return true;
    }

    private int GenerateKeyForDictionary()
    {
        return (int)(Random.value * int.MaxValue);
    }
}
