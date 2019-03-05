using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    [SerializeField] protected int bulletID;
    [SerializeField] protected float fireInterval;

    public abstract void Fire(Transform[] barrels, int energy, float spawnTime);

    public void Fire(ref float t, Transform[] barrels, int energy)
    {
        while (t <= 0)
        {
            Fire(barrels, energy, Time.time + t);
            t += fireInterval;
        }
    }
}
