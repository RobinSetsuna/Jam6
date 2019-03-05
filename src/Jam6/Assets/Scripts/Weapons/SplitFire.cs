using UnityEngine;

public class SplitFire : Weapon
{
    [SerializeField] private int[] fireAngles = new int[9];

    public override void Fire(Vector3 orientation, Transform[] barrels, int energy, float spawnTime)
    {
        LinearMovement bullet;
        for (int i = 0; i < Mathf.Min(barrels.Length, 1 + (energy / 1000) * 2); i++)
        {
            bullet = ObjectRecycler.Singleton.GetObject<LinearMovement>(bulletID);
            bullet.initialPosition = barrels[i].position;
            bullet.orientation = Quaternion.Euler(0, 0, fireAngles[i]) * orientation;
            bullet.spawnTime = spawnTime;
            bullet.gameObject.SetActive(true);
        }
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Assets/Create/Weapons/SplitFire")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<SplitFire>();
    }
#endif
}
