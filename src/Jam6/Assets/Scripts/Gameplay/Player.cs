using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public static Player Singleton { get; private set; }

    [SerializeField] private int maxNumLives = 7;
    [SerializeField] private int initialNumLives = 3;
    [SerializeField] private Weapon initialWeapon;
    [SerializeField] private int maxHp = 0;
    [SerializeField] private int maxEnergy = 10000;

    public EventOnDataUpdate<int> OnNumLivesChange { get; private set; }
    public EventOnDataUpdate<int> OnEnergyChange { get; private set; }

    private int numLives;
    private int hp;
    private int energy;

    public int NumLives
    {
        get
        {
            return numLives;
        }

        private set
        {
            if (value != numLives)
            {
                numLives = value;
                OnNumLivesChange.Invoke(value);
            }
        }
    }

    public int MaxEnergy
    {
        get
        {
            return maxEnergy;
        }
    }

    public int Energy
    {
        get
        {
            return energy;
        }

        private set
        {
            if (value != energy)
            {
                energy = value;
                OnEnergyChange.Invoke(value);
            }
        }
    }

    public Weapon MainWeapon { get; private set; }

    public int ApplyDamage(int rawDamage)
    {
        int absorbedDamage = Mathf.Min(rawDamage, energy / 10);

        Energy -= absorbedDamage * 10;
        rawDamage -= absorbedDamage;
        hp -= rawDamage;

        if (rawDamage > 0 && hp <= 0)
            NumLives--;

        return rawDamage;
    }

    public int GainEnergy(int rawEnergy)
    {
        int sp0 = energy;

        Energy = Mathf.Min(maxEnergy, energy + rawEnergy);

        return energy - sp0;
    }

    private void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;

            OnNumLivesChange = new EventOnDataUpdate<int>();
            OnEnergyChange = new EventOnDataUpdate<int>();
        }
        else if (this != Singleton)
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        numLives = initialNumLives;
        MainWeapon = initialWeapon;
    }

    private void OnDestroy()
    {
        if (this == Singleton)
            Singleton = null;
    }
}
