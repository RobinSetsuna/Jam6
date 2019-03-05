using System.Collections;
using UnityEngine;

public class TurrentM1 : Turrent
{
    [SerializeField] private int bulletID = 6;
    [SerializeField] private float bulletInterval = 0.03f;
    [SerializeField] private int numBullets = 30;
    [SerializeField] private Transform[] barrels = new Transform[6];

    protected override void Shoot()
    {
        StartCoroutine(Fire(numBullets));
    }

    private IEnumerator Fire(int N)
    {
        float t = 0;
        float currentTime;

        LinearMovement bullet;
        for (int n = 0; n < N; n++)
        {
            currentTime = Time.time;

            while (t <= 0)
            {
                for (int i = 0; i < barrels.Length; i++)
                {
                    bullet = ObjectRecycler.Singleton.GetObject<LinearMovement>(bulletID);
                    bullet.initialPosition = barrels[i].position;
                    bullet.orientation = Player.Singleton.transform.position - barrels[i].position;
                    bullet.spawnTime = currentTime + t;

                    bullet.gameObject.SetActive(true);
                }

                t += bulletInterval;
            }

            yield return null;

            t -= Time.deltaTime;
        }

        yield break;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < barrels.Length; i++)
            if (barrels[i])
                Gizmos.DrawSphere(barrels[i].position, 0.1f);
    }
#endif
}
