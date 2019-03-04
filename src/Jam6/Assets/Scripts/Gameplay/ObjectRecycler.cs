using System.Collections.Generic;
using UnityEngine;

public class ObjectRecycler : MonoBehaviour
{
    public static ObjectRecycler Singleton { get; private set; }

    [SerializeField] private Recyclable[] prefabs;

    private Stack<Recyclable>[] inactiveBullets;

    private ObjectRecycler() { }

    public Recyclable GetObject(int id)
    {
        Recyclable recyclable;

        if (inactiveBullets[id].Count > 0)
            recyclable = inactiveBullets[id].Pop();
        else
            recyclable = Instantiate(prefabs[id], transform);

        recyclable.id = id;

        return recyclable;
    }

    public void Recycle(Recyclable recyclable)
    {
        if (recyclable.id >= 0)
            inactiveBullets[recyclable.id].Push(recyclable);
    }

    private void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);

            inactiveBullets = new Stack<Recyclable>[prefabs.Length];

            for (int id = 0; id < inactiveBullets.Length; id++)
                inactiveBullets[id] = new Stack<Recyclable>();
        }
        else if (this != Singleton)
            Destroy(gameObject);
    }
}
