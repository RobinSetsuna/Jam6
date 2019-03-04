using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHp;

    private float hp;

    public float ApplyDamage(float rawDamage)
    {
        hp -= rawDamage;
        return rawDamage;
    }

    private void OnEnable()
    {
        hp = maxHp;
    }
}
