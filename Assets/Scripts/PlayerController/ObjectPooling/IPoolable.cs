using System.Collections.Generic;

public interface IPoolable
{
    int PoolKey { get; set; } //integer key that points to current object in UsedObjects dictionary of ObjectPool
    void Initialize ();
    void Reset ();
}
