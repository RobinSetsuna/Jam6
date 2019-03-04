using UnityEngine;

public abstract class Hover : Enemy
{
    public Vector3 initialPosition;
    public Vector3 targetPosition;

    private Vector3 orientation;
    private float t0;

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
                Debug.Log(LogUtility.MakeLogStringFormat("Hover", "Reset {0}.", value));
#endif

                //switch (currentState)
                //{
                //}
            }
            else
            {
                // Before leaving the previous state
                //switch (currentState)
                //{
                //}

#if UNITY_EDITOR
                Debug.Log(LogUtility.MakeLogStringFormat("Hover", "Made a transition to state {0}.", value));
#endif

                currentState = value;

                // After entering the new state
                switch (currentState)
                {
                    case 1:
                        orientation = (targetPosition - initialPosition).normalized;
                        transform.up = orientation;
                        t0 = Time.time;
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
                transform.position = initialPosition + orientation * speed * (Time.time - t0);
                if ((transform.position - targetPosition).sqrMagnitude < 0.1f)
                    CurrentState = 2;
                break;


            case 2:
                transform.up = Player.Singleton.transform.position - transform.position;
                Shoot();
                break;
        }
    }
}
