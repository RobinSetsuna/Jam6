using UnityEngine;

public class HoverEnemyS2 : Hover
{
    [SerializeField] private float fireInterval = 0.75f;
    [SerializeField] private int bulletID = 4;

    private float lastFireTime;

    protected override void Shoot()
    {
        if (Time.time - lastFireTime >= fireInterval)
        {
            LinearMovement bulletMovement;

            for (int r = -30; r <= 30; r += 15)
            {
                bulletMovement = ObjectRecycler.Singleton.GetObject<LinearMovement>(bulletID);
                bulletMovement.initialPosition = transform.position + transform.up;
                bulletMovement.orientation = Quaternion.Euler(0, 0, r) * transform.up;
                bulletMovement.gameObject.SetActive(true);
            }

            lastFireTime = Time.time;
        }
    }
}
