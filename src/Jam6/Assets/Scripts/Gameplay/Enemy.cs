using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Enemy : Recyclable, IDamageable
{
    [SerializeField] protected int maxHp = 1;
    [SerializeField] protected float speed = 3;
    [SerializeField] private int lootChance = 10;

    protected int hp;
    protected int damageFactor;

    protected int currentState;

    protected abstract int CurrentState { get; set; }

    public int ApplyDamage(int rawDamage)
    {
        hp -= rawDamage * damageFactor;

        if (hp <= 0)
        {
            if (Random.Range(0, 100) < 10)
                ItemDataManager.Singleton.InstantiateWeapon(20 + Random.Range(0, 2), transform.position);

            Die();
        }

        return rawDamage;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        hp = maxHp;
        damageFactor = 1;

        currentState = 0;
        CurrentState = 1;
    }
}
