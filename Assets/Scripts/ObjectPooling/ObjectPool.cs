using UnityEngine;
using System.Collections.Generic;

public partial class ObjectPoolManager
{
    private class ObjectPool : System.IDisposable
    {
        bool disposed = false;
        Stack<GameObject> UnusedObjects;
        Dictionary<int, GameObject> UsedObjects;

        public ObjectPool(string objectClass, GameObject sampleObject, IPoolable poolComponent, int poolSize)
        {
            //instantiate a new list of game objects to store our pooled objects in.
            UnusedObjects = new Stack<GameObject>(poolSize);
            UsedObjects = new Dictionary<int, GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                poolComponent.PoolKey = GenerateKeyForDictionary();
                poolComponent.ClassName = objectClass;
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
            else if (UsedObjects.Count > 0)
            {
                //We're out of unused objects, so we will reuse random one already in use
                ReusedObject = System.Linq.Enumerable.ToList(UsedObjects.Values)[Random.Range(0, UsedObjects.Count - 1)];
            }
            else
            {
                return null;
            }
            PoolComponent = ReusedObject.GetComponent(typeof(IPoolable)) as IPoolable;
            UsedObjects[PoolComponent.PoolKey] = ReusedObject;
            PoolComponent.Initialize();
            return ReusedObject;
        }

        public bool Destroy(GameObject pooledObject, IPoolable poolComponent)
        {
            if (!UsedObjects.Remove(poolComponent.PoolKey))
            {
                return false;
            }
            UnusedObjects.Push(pooledObject);
            poolComponent.Reset();
            return true;
        }

        private int GenerateKeyForDictionary()
        {
            return (int)(Random.value * int.MaxValue);
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                // Free any managed objects here.
            }

            // Free any unmanaged objects here.
            foreach (KeyValuePair<int, GameObject> pair in UsedObjects)
            {
                Object.Destroy(pair.Value);
            }
            UsedObjects = null;
            while (UnusedObjects.Count > 0)
            {
                Object.Destroy(UnusedObjects.Pop());
            }
            UnusedObjects = null;
            disposed = true;
        }
    }
}
