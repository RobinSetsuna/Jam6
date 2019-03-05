using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public static Player Singleton { get; private set; }

    [Header("Stats")]
    [SerializeField] private int maxNumLives = 7;
    [SerializeField] private int initialNumLives = 3;
    [SerializeField] private Weapon[] weapons = new Weapon[2];
    [SerializeField] private int maxHp = 0;
    [SerializeField] private int maxEnergy = 10000;

    [Header("References")]
    [SerializeField] private GameObject shieldFx;

    [Header("Item Setting")]
    public float speedUpDuration = 3.0f;
    public float smallDuration = 3.0f;
    public float shieldDuration = 3.0f;

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

    public bool IsProtected { get; private set; }

    public float FireIntervalFactor { get; private set; }

    public int ApplyDamage(int rawDamage)
    {
        int absorbedDamage = Mathf.Min(rawDamage, energy / 10);

        Energy -= absorbedDamage * 10;
        rawDamage -= absorbedDamage;
        hp -= rawDamage;

        if (rawDamage > 0 && hp <= 0)
        {
            NumLives = Mathf.Max(0, numLives - 1);
            StartCoroutine(ArmWithShield(3));
        }

        return rawDamage;
    }

    public int GainEnergy(int rawEnergy)
    {
        int sp0 = energy;

        Energy = Mathf.Min(maxEnergy, energy + rawEnergy);

        return energy - sp0;
    }

    private IEnumerator SpeedUpFireInterval(float duration)
    {
        FireIntervalFactor = 0.5f;

        yield return new WaitForSeconds(duration);

        FireIntervalFactor = 1f;
    }

    private IEnumerator BecomeSmaller(float duration)
    {
        transform.localScale = transform.localScale * 0.5f;

        yield return new WaitForSeconds(duration);

        transform.localScale = transform.localScale * 2.0f;
    }

    private IEnumerator ArmWithShield(float duration)
    {
        IsProtected = true;
        shieldFx.SetActive(true);

        yield return new WaitForSeconds(duration);

        IsProtected = false;
        shieldFx.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("HealObj"))
        {
            NumLives = Mathf.Min(maxNumLives, numLives + 1);
            other.GetComponent<Recyclable>().Die();
        }
        else if (other.tag.Equals("SpeedUpObj"))
        {
            StartCoroutine(SpeedUpFireInterval(speedUpDuration));
            other.GetComponent<Recyclable>().Die();
        }
        else if (other.tag.Equals("SmallObj"))
        {
            StartCoroutine(BecomeSmaller(smallDuration));
            other.GetComponent<Recyclable>().Die();
        }
        else if (other.tag.Equals("ShieldObj"))
        {
            StartCoroutine(ArmWithShield(shieldDuration));
            other.GetComponent<Recyclable>().Die();
        }
        else if (other.tag.Equals("StraightFireWeapon"))
        {
            MainWeapon = weapons[0];
            other.GetComponent<Recyclable>().Die();
        }
        else if (other.tag.Equals("SplitFireWeapon"))
        {
            MainWeapon = weapons[1];
            other.GetComponent<Recyclable>().Die();
        }
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
        MainWeapon = weapons[0];

        IsProtected = false;
        FireIntervalFactor = 1f;
    }

    private void OnDestroy()
    {
        if (this == Singleton)
            Singleton = null;
    }
}
