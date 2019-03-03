using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour {
    [SerializeField] private float playerHealth = 10.0f;
    public float maxHealth = 15.0f;
    public float intervalOfHealOBj = 5.0f;
    public float intervalOfSpeedUpObj = 5.0f;
    public float intervalOfSmallObj = 5.0f;
    public float intervalOfShieldObj = 5.0f;
    //public GameObject healObjPrefab;


    private float xConstraint = 12f;
    private float yConstraint = 6.7f;
    private float previousTimeHeal;
    private float previousSpeedUp;
    private float previousSmall;
    private float previousShield;

    // Use this for initialization
    void Start () {
        //
        GameObject HealObj = ObjectPool.GetInstance().GetObj("HealObj", new Vector3(Random.Range((-1.0f) * xConstraint, xConstraint), yConstraint, 0.0f));

        
    }
	
	// Update is called once per frame
	void Update () {
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
    public void AddHealth(Collider2D collision)
    {
        if(playerHealth < maxHealth) {
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

    void InstantiateHealObj()
    {
        GameObject HealObjPrefab = ObjectPool.GetInstance().GetObj("HealObj", new Vector3(Random.Range((-1.0f) * xConstraint, xConstraint), yConstraint, 0.0f));
        //HealObjPrefab.transform.position = new Vector3(Random.Range((-1.0f) * xConstraint, xConstraint), yConstraint, 0.0f);
      
    }
    void InstantiateSpeedUpObj()
    {
        GameObject HealObjPrefab = ObjectPool.GetInstance().GetObj("SpeedUpObj", new Vector3(Random.Range((-1.0f) * xConstraint, xConstraint), yConstraint, 0.0f));

    }
    void InstantiateSmallObj()
    {
        GameObject HealObjPrefab = ObjectPool.GetInstance().GetObj("SmallObj", new Vector3(Random.Range((-1.0f) * xConstraint, xConstraint), yConstraint, 0.0f));

    }
    void InstantiateShieldObj()
    {
        GameObject HealObjPrefab = ObjectPool.GetInstance().GetObj("SheildObj", new Vector3(Random.Range((-1.0f) * xConstraint, xConstraint), yConstraint, 0.0f));

    }





}
