using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    /// <summary>
    /// The unique instance
    /// </summary>
    public static ItemDataManager Singleton { get; private set; }

    [SerializeField] private float playerHealth = 10.0f;
    public float maxHealth = 15.0f;
    public float intervalOfHealOBj = 5.0f;
    public float intervalOfSpeedUpObj = 5.0f;
    public float intervalOfSmallObj = 5.0f;
    public float intervalOfShieldObj = 5.0f;
    //public GameObject healObjPrefab;

    public float xConstraint = 8f;
    private float yConstraint = 8f;
    private float previousTimeHeal;
    private float previousSpeedUp;
    private float previousSmall;
    private float previousShield;

    public void AddHealth(Collider2D collision)
    {
        if (playerHealth < maxHealth)
        {
            playerHealth++;
            ObjectPool.GetInstance().RecycleObj(collision.gameObject);
        }
    }
    public void SpeedUp(Collider2D collision)
    {
        ObjectPool.GetInstance().RecycleObj(collision.gameObject);
    }
    public void BecomeSmall(Collider2D collision)
    {
        ObjectPool.GetInstance().RecycleObj(collision.gameObject);

    }
    public void WithShield(Collider2D collision)
    {
        ObjectPool.GetInstance().RecycleObj(collision.gameObject);
    }

    public void InstantiateHealObj()
    {
        InstantiateItem(16);
    }
    public void InstantiateSpeedUpObj()
    {
        InstantiateItem(17);
    }
    public void InstantiateSmallObj()
    {
        InstantiateItem(18);
    }
    public void InstantiateShieldObj()
    {
        InstantiateItem(19);
    }

    public void InstantiateItem(int id)
    {
        ObjMovement item = ObjectRecycler.Singleton.GetObject<ObjMovement>(id);
        item.orientation = MathUtility.GetOrientation(Random.Range(225, 315));
        item.transform.position = new Vector3(Random.Range(-xConstraint, xConstraint), yConstraint + 3, 0f);
        item.gameObject.SetActive(true);
    }

    public void InstantiateWeapon(int id, Vector3 position)
    {
        //GameObject HealObjPrefab = ObjectPool.GetInstance().GetObj("SheildObj", new Vector3(Random.Range((-1.0f) * xConstraint, xConstraint), yConstraint, 0.0f));
        ObjMovement item = ObjectRecycler.Singleton.GetObject<ObjMovement>(id);
        item.orientation = MathUtility.GetOrientation(Random.Range(225, 315));
        item.transform.position = position;
        item.gameObject.SetActive(true);
    }

    private void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (this != Singleton)
            Destroy(gameObject);
    }

    private void Start()
    {
        //
        //GameObject HealObj = ObjectPool.GetInstance().GetObj("HealObj", new Vector3(Random.Range((-1.0f) * xConstraint, xConstraint), yConstraint, 0.0f));

        
    }
	
	// Update is called once per frame
	private void Update()
    {
		if((Time.time - previousTimeHeal)> intervalOfHealOBj)
        {
            InstantiateHealObj();
            previousTimeHeal = Time.time;
        }
        if ((Time.time - previousSpeedUp) > intervalOfSpeedUpObj)
        {
            InstantiateSpeedUpObj();
            previousSpeedUp = Time.time;
        }
        if ((Time.time - previousSmall) > intervalOfSmallObj)
        {
            InstantiateSmallObj();
            previousSmall = Time.time;
        }
        if ((Time.time - previousShield) > intervalOfShieldObj)
        {
            InstantiateShieldObj();
            previousShield = Time.time;
        }
    }
}
