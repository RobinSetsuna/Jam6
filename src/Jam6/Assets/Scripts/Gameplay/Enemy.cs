using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Enemy : Recyclable, IDamageable
{
    [SerializeField] private float maxHp;
    [SerializeField] protected float speed = 3;

    private float hp;

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
    }
}
