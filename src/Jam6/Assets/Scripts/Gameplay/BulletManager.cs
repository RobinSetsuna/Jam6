using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Singleton { get; private set; }

    [SerializeField] private Bullet[] prefabs;

    private Stack<Bullet>[] inactiveBullets;

    private BulletManager() { }

    public Bullet GetBullet(int id)
    {
        Bullet bullet;

        if (inactiveBullets[id].Count > 0)
            bullet = inactiveBullets[id].Pop();
        else
            bullet = Instantiate(prefabs[id], transform);

        bullet.id = id;

        return bullet;
    }

    public void Recycle(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);

        inactiveBullets[bullet.id].Push(bullet);
    }

    private void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);

            inactiveBullets = new Stack<Bullet>[prefabs.Length];

            for (int id = 0; id < inactiveBullets.Length; id++)
                inactiveBullets[id] = new Stack<Bullet>();
        }
        else if (this != Singleton)
            Destroy(gameObject);
    }
}
