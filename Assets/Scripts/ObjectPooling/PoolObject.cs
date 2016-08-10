using UnityEngine;

public class PoolObject : MonoBehaviour, IPoolable
{
    // Use this for initialization
    void Start ()
    {
        Reset();
    }

    // Update is called once per frame
    void Update ()
    {
        
	}

    void OnBecameInvisible ()
    {
        ObjectPoolManager.Instance.Destroy(gameObject);
    }

    private int PoolKey = -1;

    int IPoolable.PoolKey {
        get
        {
            return PoolKey;
        }
        set
        {
            if (PoolKey == -1)
            {
                PoolKey = value;
            }
        }
    }

    private string ClassName;

    string IPoolable.ClassName {
        get
        {
            return ClassName;
        }
        set
        {
            if (ClassName == null)
            {
                ClassName = value;
            }
        }
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
        gameObject.SetActive(false);
    }
}
