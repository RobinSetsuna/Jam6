using System.Collections.Generic;
using UnityEngine;

public class ObjectRecycler : MonoBehaviour
{
    public static ObjectRecycler Singleton { get; private set; }

    [SerializeField] private Recyclable[] prefabs;

    private Stack<Recyclable>[] recycledObjects;

    private ObjectRecycler() { }

    public Recyclable GetObject(int id)
    {
        if (recycledObjects[id].Count > 0)
            return recycledObjects[id].Pop();

        Recyclable recyclable = Instantiate(prefabs[id], transform);
        recyclable.id = id;

        return recyclable;
    }

    public T GetObject<T>(int id) where T : MonoBehaviour
    {
        if (recycledObjects[id].Count > 0)
            return recycledObjects[id].Pop().GetComponent<T>();

        T obj = Instantiate(prefabs[id].GetComponent<T>(), transform);

        if (obj)
            obj.GetComponent<Recyclable>().id = id;

        return obj;
    }

    public void Recycle(Recyclable recyclable)
    {
        if (recyclable.id >= 0)
            recycledObjects[recyclable.id].Push(recyclable);
    }

    private void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);

            recycledObjects = new Stack<Recyclable>[prefabs.Length];

            for (int id = 0; id < recycledObjects.Length; id++)
                recycledObjects[id] = new Stack<Recyclable>(256);
        }
        else if (this != Singleton)
            Destroy(gameObject);
    }
}
