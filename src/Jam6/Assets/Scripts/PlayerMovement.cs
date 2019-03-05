using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Setting")]
    public float speed = 2.0f;
    public float rotationSpeed = 250.0f;
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -10f;
    public float maxY = 10f;
    public bool enableTurning = false;
    public bool screenWrap = false;

    [Header("References")]
    [SerializeField] private ItemDataManager dataManager;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform[] barrels = new Transform[1];

    private float t = 0;

    private Camera mainCamera;

    private int animationState = 0;
    private const int PLAYER_IDLE = 0;
    private const int PLAYER_LEFT = 1;
    private const int PLAYER_RIGHT = 2;

    private bool isSmall = false;

    // Use this for initialization
    void OnEnable()
    {
        t = 0;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //Bounds();

        Player.Singleton.MainWeapon.Fire(ref t, transform.up, barrels, Player.Singleton.Energy, Player.Singleton.FireIntervalFactor);

        t -= Time.deltaTime;
    }

    void Move()
    {
        animator.SetInteger("state", PLAYER_IDLE);

        Vector3 upVector = transform.up;
        Vector3 rightVector = transform.right;

        if (Input.GetKey(KeyCode.W))
            transform.position = Vector3.Min(new Vector3(float.MaxValue, maxY, float.MaxValue), transform.position + upVector * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.S))
            transform.position = Vector3.Max(new Vector3(float.MinValue, minY, float.MinValue), transform.position - upVector * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
        {
            animator.SetInteger("state", PLAYER_LEFT);
            transform.position = Vector3.Max(new Vector3(minX, float.MinValue, float.MinValue), transform.position - rightVector * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            animator.SetInteger("state", PLAYER_RIGHT);
            transform.position = Vector3.Min(new Vector3(maxX, float.MaxValue, float.MaxValue), transform.position + rightVector * speed * Time.deltaTime);
        }

        if (enableTurning)
        {
            if (Input.GetKey(KeyCode.Q))
                transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));

            if(Input.GetKey(KeyCode.E))
                transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime));
        }
    }

    private void Bounds()
    {
        Vector3 positionOnScreen = mainCamera.WorldToScreenPoint(transform.position);

        if (positionOnScreen.y < 0)
        {
            positionOnScreen.y = 0;
        }
        else if (positionOnScreen.y > Screen.height)
        {
            positionOnScreen.y = Screen.height;
        }

        if (screenWrap)
        {
            if (positionOnScreen.x < 0)
            {
                positionOnScreen.x = Screen.width - 1;
            }
            else if (positionOnScreen.x > Screen.width)
            {
                positionOnScreen.x = 1;
            }
        }
        else
        {
            if (positionOnScreen.x < 0)
            {
                positionOnScreen.x = 0;
            }
            else if (positionOnScreen.x > Screen.width)
            {
                positionOnScreen.x = Screen.width;
            }
        }

        transform.position = mainCamera.ScreenToWorldPoint(positionOnScreen);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < barrels.Length; i++)
            if (barrels[i])
                Gizmos.DrawSphere(barrels[i].position, 0.1f);
    }
#endif
}
