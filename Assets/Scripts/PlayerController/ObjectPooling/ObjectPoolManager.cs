using UnityEngine;
using System.Collections.Generic;


public class ObjectPoolManager
{
    //The variable is declared to be volatile to ensure that 
    //assignment to the instance variable completes before the 
    //instance variable can be accessed
    private static volatile ObjectPoolManager instance;

    private Dictionary<string, ObjectPool> ObjectPools;

    //Object for locking
    private static object syncRoot = new System.Object();

    private ObjectPoolManager()
    {
        ObjectPools = new Dictionary<string, ObjectPool>();
    }

    public static ObjectPoolManager Instance
    {
        get
        {
            //check to see if it doesnt exist
            if (instance == null)
            {
                //lock access, if it is already locked, wait
                lock (syncRoot)
                {
                    //the instance could have been made between
                    //checking and waiting for a lock to release.
                    if (instance == null)
                    {
                        //create a new instance
                        instance = new ObjectPoolManager();
                    }
                }
            }
            //return either the new instance or the already built one.
            return instance;
        }
    }

    public bool CreatePool(string objectClass, GameObject sampleObject, int poolSize)
    {
        //Check to see if the pool already exists.
        if (Instance.ObjectPools.ContainsKey(objectClass))
        {
            //Pool already exists
            return false;
        }
        else
        {
            //Create a new pool using the properties,
            //add the pool to the dictionary of pools to manage
            //using the name as the key and the pool as the value
            Instance.ObjectPools.Add(objectClass, new ObjectPool(sampleObject, poolSize));
            return true;
        }
    }

    public GameObject Instantiate (string objectClass)
    {
        return Instance.ObjectPools[objectClass].Instantiate();
    }

    public bool Destroy (string objectClass, GameObject pooledObject)
    {
        IPoolable PoolComponent = pooledObject.GetComponent(typeof(IPoolable)) as IPoolable;
        if (PoolComponent == null)
        {
            throw new System.ArgumentException("Passed object does not contain component that implements IPoolable!", "sampleObject");
        }
        ObjectPool ObjectPool;
        if (Instance.ObjectPools.TryGetValue(objectClass, out ObjectPool))
        {
            return ObjectPool.Destroy(pooledObject);
        }
        else
        {
            return false;
        }
    }
}
