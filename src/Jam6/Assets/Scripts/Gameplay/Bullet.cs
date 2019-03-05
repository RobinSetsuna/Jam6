using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Bullet : Recyclable
{
    public bool isFriendly = false;
    public int numHits = 1;
    public int rawDamage = 100;

    private int numHitsRemaining;
    private bool hasEnergy;

    protected override void OnEnable()
    {
        base.OnEnable();

        numHitsRemaining = numHits;
        hasEnergy = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isFriendly)
        {
            if (other.tag == "Enemy")
            {
                other.GetComponent<IDamageable>().ApplyDamage(rawDamage);
                if (--numHitsRemaining == 0)
                    Die();
            }
        }
        else if (other.tag == "Player")
        {
            if (Player.Singleton.IsProtected)
                Die();
            else
            {
                other.GetComponent<IDamageable>().ApplyDamage(rawDamage);
                if (--numHitsRemaining == 0)
                    Die();
            }
        }
        else if (other.tag == "Wing" && hasEnergy)
        {
            other.transform.parent.GetComponent<Player>().GainEnergy(rawDamage);
            hasEnergy = false;
        }
        else if (other.tag == "Shield" && numHitsRemaining > 0)
            Die();
    }
}
