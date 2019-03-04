using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Movement), typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    public int id = -1;
    public bool isFriendly = false;
    public float lifeSpan = 3;
    public float rawDamage = 1;

    private float t0 = -1;

    private void OnTriggerEnter(Collider other)
    {
        if (isFriendly)
        {
            if (other.tag == "Enemy")
                other.GetComponent<Enemy>().ApplyDamage(rawDamage);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(RecycleAfter(lifeSpan));
    }

    private IEnumerator RecycleAfter(float t)
    {
        yield return new WaitForSeconds(t);

        BulletManager.Singleton.Recycle(this);
    }
}
