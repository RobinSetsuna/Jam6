using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMovement : MonoBehaviour {

    public float ySpeed = -1.0f;
    public float xSpeed = 1.0f;
    public float xDistance = 3.0f;
    public float xConstraint = 4.38f;
    //private float totalTime = 0.0f;
    //private Vector3 originalPosition;

    private float direction = -1.0f;
	// Use this for initialization
	void Start () {
       //originalPosition = this.transform.position;
	}

    // Update is called once per frame
    void Update()
    {
        Move();


    }
    private void Move()
    {


        if (this.transform.position.x > xConstraint)
        {
            direction = -1.0f;
        }
        if (this.transform.position.x < direction*xConstraint)
        {
            direction = 1.0f;
        }
      
        this.transform.position += Vector3.right * xSpeed * direction * Time.deltaTime + Vector3.up * ySpeed * Time.deltaTime;
    }





}
