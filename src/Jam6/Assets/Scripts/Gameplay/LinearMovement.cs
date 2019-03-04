using UnityEngine;

public class LinearMovement : Movement
{
    public float speed;
    public Vector3 orientation;
    public Vector3 initialPosition;

    private float t0;

    private void OnEnable()
    {
        orientation = orientation.normalized;

        t0 = Time.time;
    }

    private void Update()
    {
		transform.position = initialPosition + (Time.time - t0) * orientation * speed;
	}
}
