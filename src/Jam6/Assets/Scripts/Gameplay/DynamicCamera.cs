using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour {
    private Camera cam;
    private Vector3 velocity;
    public float moveFactor;
    public float smoothTimeY;
	// Use this for initialization
	void Start () {
        cam = gameObject.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        float incremental = Time.unscaledTime * moveFactor;
        float y = Mathf.SmoothDamp(cam.transform.position.y, incremental, ref velocity.y, smoothTimeY);
        cam.transform.position = new Vector3(cam.transform.position.x, y, cam.transform.position.z);
	}
}
