using UnityEngine;

public class StraightFire : Weapon
{
    public override void Fire(Vector3 orientation, Transform[] barrels, int energy, float spawnTime)
    {
        LinearMovement bullet;
        for (int i = 0; i < Mathf.Min(barrels.Length, 1 + (energy / 1000) * 2); i++)
        {
            bullet = ObjectRecycler.Singleton.GetObject<LinearMovement>(bulletID);
            bullet.initialPosition = barrels[i].position;
            bullet.orientation = orientation;
            bullet.spawnTime = spawnTime;
            bullet.gameObject.SetActive(true);
        }
    }
}
