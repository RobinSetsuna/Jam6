using UnityEngine;

public abstract class ChasingEnemy : Enemy
{
    public Vector3 initialPosition;
    public Vector3 targetPosition;

    public float chaseSpeed = 10;
    public float chaseDuration = 1f;
    public float chaseInterval = 2f;

    private Vector3 orientation;
    private float t;

    protected override int CurrentState
    {
        get
        {
            return currentState;
        }

        set
        {
            // Reset the current state
            if (value == currentState)
            {
#if UNITY_EDITOR
                //Debug.Log(LogUtility.MakeLogStringFormat("Hover", "Reset {0}.", value));
#endif

                //switch (currentState)
                //{
                //}
            }
            else
            {
                // Before leaving the previous state
                switch (currentState)
                {
                    case 1:
                        t = 0;
                        break;
                }

#if UNITY_EDITOR
                //Debug.Log(LogUtility.MakeLogStringFormat("Hover", "Made a transition to state {0}.", value));
#endif

                currentState = value;

                // After entering the new state
                switch (currentState)
                {
                    case 1:
                        orientation = (targetPosition - initialPosition).normalized;
                        transform.up = orientation;
                        t = Time.time;
                        break;


                    case 2:
                        t += chaseDuration;
                        break;


                    case 3:
                        t += chaseInterval;
                        Shoot();
                        break;
                }

                //switch (currentState)
                //{
                //}
            }
        }
    }

    protected abstract void Shoot();

    private void Update()
    {
        switch (currentState)
        {
            case 1:
                transform.position = initialPosition + orientation * speed * (Time.time - t);
                if ((transform.position - targetPosition).sqrMagnitude < 0.1f)
                    CurrentState = 2;
                break;


            case 2:
                t -= Time.deltaTime;

                transform.up = Vector3.down;
                float dx = Player.Singleton.transform.position.x - transform.position.x;

                if (t <= 0)
                    CurrentState = 3;
                else
                {
                    if (dx < -0.1f)
                        transform.Translate(Vector3.left * chaseSpeed * Time.deltaTime);
                    else if (dx > 0.1f)
                        transform.Translate(Vector3.right * chaseSpeed * Time.deltaTime);
                    else
                        CurrentState = 3;
                }
                break;


            case 3:
                t -= Time.deltaTime;
                if (t <= 0)
                    CurrentState = 2;
                break;
        }
    }
}
