using UnityEngine;
using System.Collections.Generic;


public partial class ObjectPoolManager
{
    private static readonly ObjectPoolManager instance = new ObjectPoolManager();
    private Dictionary<string, ObjectPool> ObjectPools = new Dictionary<string, ObjectPool>();

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static ObjectPoolManager ()
    {

    }

    private ObjectPoolManager ()
    {

    }

    public static ObjectPoolManager Instance
    {
        get
        {
            return instance;
        }
    }

    /*
    private static ObjectPoolManager instance;
    //Object for locking
    private static readonly object syncRoot = new object();
    private Dictionary<string, ObjectPool> ObjectPools;

    private ObjectPoolManager()
    {

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
                        instance.ObjectPools = new Dictionary<string, ObjectPool>();
                    }
                }
            }
            //return either the new instance or the already built one.
            return instance;
        }
    }
    */

    public bool CreatePool(string objectClassName, GameObject poolObject, int poolSize)
    {
        CheckObjectClassName(objectClassName);
        CheckPoolSize(poolSize);
        //Check to see if the pool already exists.
        if (Instance.ObjectPools.ContainsKey(objectClassName))
        {
            return false;
        }
        else
        {
            //Create a new pool using the properties,
            //add the pool to the dictionary of pools to manage
            //using GameObject class name as the key and the pool as the value
            Instance.ObjectPools.Add(objectClassName, new ObjectPool(objectClassName, poolObject, CheckPoolObject(poolObject), poolSize));
            return true;
        }
    }

    public bool DestroyPool(string objectClassName)
    {
        CheckObjectClassName(objectClassName);
        CheckPoolExists(objectClassName).Dispose();
        return Instance.ObjectPools.Remove(objectClassName);
    }

    public GameObject Instantiate (string objectClassName)
    {
        CheckObjectClassName(objectClassName);
        return CheckPoolExists(objectClassName).Instantiate();
    }

    public bool Destroy (GameObject poolObject)
    {
        IPoolable PoolComponent = CheckPoolObject(poolObject);
        ObjectPool ObjectPool;
        if (Instance.ObjectPools.TryGetValue(PoolComponent.ClassName, out ObjectPool))
        {
            return ObjectPool.Destroy(poolObject, PoolComponent);
        }
        else
        {
            return false;
        }
    }

    private void CheckObjectClassName (string objectClassName)
    {
        if (objectClassName == null)
        {
            throw new System.ArgumentException("Passed object class name is null!", "objectClassName");
        }
        else if (objectClassName.Length == 0)
        {
            throw new System.ArgumentException("Passed object class name is empty!", "objectClassName");
        }
    }

    private IPoolable CheckPoolObject (GameObject poolObject)
    {
        if (poolObject == null)
        {
            throw new System.ArgumentException("Passed object does not contain component that implements IPoolable!", "poolObject");
        }
        IPoolable PoolComponent = poolObject.GetComponent(typeof(IPoolable)) as IPoolable;
        if (PoolComponent == null)
        {
            throw new System.ArgumentException("Passed sample object does not contain component that implements IPoolable!", "poolObject");
        }
        return PoolComponent;
    }

    private void CheckPoolSize (int poolSize)
    {
        if (poolSize < 0)
        {
            throw new System.ArgumentException("Specified pool size is less than zero!", "poolObject");
        }
    }

    private ObjectPool CheckPoolExists (string objectClassName)
    {
        ObjectPool ObjectPool;
        if (!Instance.ObjectPools.TryGetValue(objectClassName, out ObjectPool))
        {
            throw new System.ArgumentException("Pool for specified object class name does not exist!", "objectClassName");
        }
        return ObjectPool;
    }
}
