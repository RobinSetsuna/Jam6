using UnityEngine;

public abstract class Turrent : Enemy
{
    [SerializeField] private float fireInterval = 5f;

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
                //switch (currentState)
                //{
                //}

#if UNITY_EDITOR
                //Debug.Log(LogUtility.MakeLogStringFormat("Hover", "Made a transition to state {0}.", value));
#endif

                currentState = value;

                // After entering the new state
                switch (currentState)
                {
                    case 1:
                        transform.up = Vector3.down;
                        break;

                    case 2:
                        t = fireInterval;
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
                if (transform.position.y < 10)
                    CurrentState = 2;
                break;


            case 2:
                t -= Time.deltaTime;

                transform.up = Player.Singleton.transform.position - transform.position;

                while (t <= 0)
                {
                    Shoot();
                    t += fireInterval;
                }

                if (transform.position.y < -10)
                    Die();
                break;
        }
    }
}
