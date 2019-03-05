using UnityEngine;

public class ObjMovement : MonoBehaviour
{
    public float speed = 3.0f;
    public Vector3 orientation;
    public float xConstraint = 4.38f;
    
    private void OnEnable()
    {
        orientation = orientation.normalized;
    }

    // Update is called once per frame
    private void Update()
    {
        if (transform.position.x > xConstraint || transform.position.x < -xConstraint)
            orientation.x = -orientation.x;

        transform.position += orientation * Time.deltaTime * speed;
    }
}
