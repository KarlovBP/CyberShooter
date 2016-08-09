using UnityEngine;

public class PoolObject : MonoBehaviour, IPoolable
{
    // Use this for initialization
    protected virtual void Start ()
    {
	    
	}

    // Update is called once per frame
    protected virtual void Update()
    {

	}

    int IPoolable.PoolKey
    {
        get; set;
    }
    
    void IPoolable.Initialize ()
    {
        Initialize();
    }

    protected virtual void Initialize ()
    {
        gameObject.SetActive(true);
    }

    void IPoolable.Reset ()
    {
        Reset();
    }

    protected virtual void Reset ()
    {

    }
}
