using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Enemy : Recyclable, IDamageable
{
    [SerializeField] protected int maxHp = 1;
    [SerializeField] protected float speed = 3;

    protected int hp;

    protected int currentState;

    protected abstract int CurrentState { get; set; }

    public int ApplyDamage(int rawDamage)
    {
        hp -= rawDamage;

        if (hp <= 0)
            Die();

        return rawDamage;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        hp = maxHp;

        currentState = 0;
        CurrentState = 1;
    }
}
