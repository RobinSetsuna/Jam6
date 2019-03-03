using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRecycle : MonoBehaviour {
    public float lifespan = 10.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator AuRecycle()
    {
        yield return new WaitForSeconds(lifespan);

        ObjectPool.GetInstance().RecycleObj(this.gameObject);
    }

    private void OnEnable()
    {
        StartCoroutine(AuRecycle());
    }
}
