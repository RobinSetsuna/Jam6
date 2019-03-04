using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public static Player Singleton { get; private set; }

    public float ApplyDamage(float rawDamage)
    {
        return rawDamage;
    }

    private void Awake()
    {
        if (!Singleton)
            Singleton = this;
        else if (this != Singleton)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (this == Singleton)
            Singleton = null;
    }
}
