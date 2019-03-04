using UnityEngine;

public class HoverEnemyS1 : Hover
{
    [SerializeField] private float fireInterval = 0.5f;
    [SerializeField] private int bulletID = 3;

    private float lastFireTime;

    protected override void Shoot()
    {
        if (Time.time - lastFireTime >= fireInterval)
        {
            LinearMovement bulletMovement = ObjectRecycler.Singleton.GetObject<LinearMovement>(bulletID);
            bulletMovement.initialPosition = transform.position + transform.up;
            bulletMovement.orientation = transform.up;
            bulletMovement.gameObject.SetActive(true);

            lastFireTime = Time.time;
        }
    }
}
