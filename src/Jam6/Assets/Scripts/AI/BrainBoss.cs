using UnityEngine;

public class BrainBoss : Enemy
{
    [SerializeField] GameObject leftLaser;
    [SerializeField] GameObject rightLaser;

    private int currentState = 0;
    private Vector3 orientation;
    private float t;

    public int CurrentState
    {
        get
        {
            return currentState;
        }

        private set
        {
            // Reset the current state
            if (value == currentState)
            {
#if UNITY_EDITOR
                Debug.Log(LogUtility.MakeLogStringFormat("BrainBoss", "Reset {0}.", value));
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
                    case 3:
                        leftLaser.SetActive(false);
                        rightLaser.SetActive(false);
                        break;
                }

#if UNITY_EDITOR
                Debug.Log(LogUtility.MakeLogStringFormat("BrainBoss", "Made a transition to state {0}.", value));
#endif

                //int previousGameState = CurrentGameState;
                currentState = value;

                // After entering the new state
                switch (currentState)
                {
                    case 1:
                        CurrentState = 2;
                        break;


                    case 2:
                        if (transform.position.x < -1.5f)
                            orientation = Vector3.right;
                        else if (transform.position.x > 1.5f)
                            orientation = Vector3.left;
                        else
                            orientation = Random.Range(0, 100) < 50 ? Vector3.left : Vector3.right;

                        t = 0;
                        break;


                    case 3:
                        t = 0;
                        leftLaser.SetActive(true);
                        rightLaser.SetActive(true);
                        break;


                    case 4:
                        CurrentState = 2;
                        break;
                }

                //switch (currentState)
                //{
                //}
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        currentState = 0;
        CurrentState = 1;
    }

    private void Update()
    {
        t += Time.deltaTime;

        if (transform.position.x < -2f)
            orientation = Vector3.right;
        else if (transform.position.x > 2f)
            orientation = Vector3.left;

        switch (currentState)
        {
            case 2:
                transform.Translate(orientation * speed * Time.deltaTime);

                if (t > 5)
                    CurrentState = 3;
                break;


            case 3:
                {
                    float dx = Player.Singleton.transform.position.x - transform.position.x;

                    Vector3 v;
                    if (dx < -1.2)
                        v = Vector3.left;
                    else if (dx > 1.2)
                        v = Vector3.right;
                    else
                        v = orientation;

                    transform.Translate(v * speed / 2 * Time.deltaTime);
                }

                if (t > 3)
                    CurrentState = 4;
                break;
        }
    }
}
