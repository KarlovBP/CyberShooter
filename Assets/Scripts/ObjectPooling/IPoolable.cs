public interface IPoolable
{
    int PoolKey { get; set; } //integer key that points to current object in UsedObjects dictionary of ObjectPool
    string ClassName { get; set; } //string that contains specified name of class of current GameObject
    void Initialize ();
    void Reset ();
}
