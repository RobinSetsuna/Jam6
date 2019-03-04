using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Enemy : Recyclable, IDamageable
{
    [SerializeField] protected float maxHp;
    [SerializeField] protected float speed = 3;

    protected float hp;

    protected int currentState;

    protected abstract int CurrentState { get; set; }

    public float ApplyDamage(float rawDamage)
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
