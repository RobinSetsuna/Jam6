using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour {
    [SerializeField] private float playerHealth = 10.0f;
    public float maxHealth = 15.0f;
    public float intervalOfHealOBj = 5.0f;

    private float xConstraint = 4.38f;
    private float yConstraint = 5.5f;
    private float previousTime;

    public GameObject healObjPrefab;
    
	// Use this for initialization
	void Start () {
        //
        GameObject HealObj = ObjectPool.GetInstance().GetObj("HealObj", new Vector3(Random.Range((-1.0f) * xConstraint, xConstraint), yConstraint, 0.0f));

        
    }
	
	// Update is called once per frame
	void Update () {
		if((Time.time - previousTime)> intervalOfHealOBj)
        {
            InstantiateHealObj();
            previousTime = Time.time;
        }
    }
    public void AddHealth(Collision2D collision)
    {
        if(playerHealth < maxHealth) {
            playerHealth++;
            ObjectPool.GetInstance().RecycleObj(collision.gameObject);
        }
    }
    void InstantiateHealObj()
    {
        GameObject HealObjPrefab = ObjectPool.GetInstance().GetObj("HealObj", new Vector3(Random.Range((-1.0f) * xConstraint, xConstraint), yConstraint, 0.0f));
        //HealObjPrefab.transform.position = new Vector3(Random.Range((-1.0f) * xConstraint, xConstraint), yConstraint, 0.0f);
      
    }
}
