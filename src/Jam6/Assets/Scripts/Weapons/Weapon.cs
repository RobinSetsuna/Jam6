using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    [SerializeField] protected int bulletID;
    [SerializeField] protected float fireInterval;

    public abstract void Fire(Vector3 orientation, Transform[] barrels, int energy, float spawnTime);

    public void Fire(ref float t, Vector3 orientation, Transform[] barrels, int energy, float fireIntervalFactor)
    {
        while (t <= 0)
        {
            Fire(orientation, barrels, energy, Time.time + t);
            t += fireInterval * fireIntervalFactor;
        }
    }
}
