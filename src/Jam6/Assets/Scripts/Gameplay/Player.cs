using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public static Player Singleton { get; private set; }

    [SerializeField] private int maxHp = 0;
    [SerializeField] private int maxSp = 10000;

    private int hp;
    private int sp;

    public int ApplyDamage(int rawDamage)
    {
        int absorbedDamage = Mathf.Min(rawDamage, sp / 10);

        sp -= absorbedDamage * 10;
        rawDamage -= absorbedDamage;
        hp -= rawDamage;

        if (rawDamage > 0 && hp <= 0)
        {

        }

        return rawDamage;
    }

    public int GainEnergy(int rawEnergy)
    {
        int sp0 = sp;

        sp = Mathf.Min(maxSp, sp + rawEnergy);

        return sp - sp0;
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
