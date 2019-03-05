using System.Collections;
using UnityEngine;

public class ChasingEnemyM1 : ChasingEnemy
{
    public int bulletID = 5;
    public float fireInterval = 0.3f;

    protected override void Shoot()
    {
        StartCoroutine(Fire(3));
    }

    private IEnumerator Fire(int N)
    {
        LinearMovement bullet;
        for (int n = 0; n < N; n++)
        {
            for (float dx = -1f; dx <= 1f; dx += 0.5f)
            {
                bullet = ObjectRecycler.Singleton.GetObject<LinearMovement>(bulletID);
                bullet.initialPosition = transform.position + new Vector3(dx, Mathf.Abs(dx) * 0.5f - 1.5f, 0);

                bullet.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(fireInterval);
        }

        yield break;
    }
}
