using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2.0f;
    public float rotationSpeed = 250.0f;
    public ItemDataManager dataManager;
    public float fireInterval = 2.0f;
    public float speedUpTime = 3.0f;
    public float smallInterval = 3.0f;
    public float shieldInterval = 3.0f;

    public bool screenWrap = false;

    [SerializeField] private Transform barrel;
    [SerializeField] private int bulletID = 0;

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
        Bounds();

        LinearMovement bullet;
        while (t <= 0)
        {
            bullet = ObjectRecycler.Singleton.GetObject<LinearMovement>(bulletID);

            bullet.initialPosition = barrel.position;
            bullet.spawnTime = Time.time + t;
            bullet.orientation = this.transform.up;

            bullet.gameObject.SetActive(true);

            t += fireInterval;
        }

        t -= Time.deltaTime;
    }
    void Move()
    {
        this.GetComponent<Animator>().SetInteger("state", PLAYER_IDLE);
        Vector3 upVector = this.transform.up;
        Vector3 rightVector = this.transform.right;
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += upVector * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position -= upVector * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.GetComponent<Animator>().SetInteger("state", PLAYER_LEFT);
            this.transform.position -= rightVector * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.GetComponent<Animator>().SetInteger("state", PLAYER_RIGHT);
            this.transform.position += rightVector * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Vector3 currentRotation = this.transform.rotation.eulerAngles;
            currentRotation.z += rotationSpeed * Time.deltaTime;
            this.transform.rotation = Quaternion.Euler(currentRotation);
        }
        if(Input.GetKey(KeyCode.E))
        {
            Vector3 currentRotation = this.transform.rotation.eulerAngles;
            currentRotation.z -= rotationSpeed * Time.deltaTime;
            this.transform.rotation = Quaternion.Euler(currentRotation);
        }
    }

    private void Bounds()
    {
        Vector3 positionOnScreen = mainCamera.WorldToScreenPoint(this.transform.position);
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
        this.transform.position = mainCamera.ScreenToWorldPoint(positionOnScreen);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detect if collision of items happen
        //Debug.Log("Collision:" + collision.gameObject.tag.ToString());

        if (collision.gameObject.tag.Equals("HealObj"))
        {
            Debug.Log("Collision:" + collision.gameObject.tag.ToString());
            dataManager.AddHealth(collision);
        }
        else if (collision.gameObject.tag.Equals("SpeedUpObj"))
        {
            Debug.Log("Collision:" + collision.gameObject.tag.ToString());
            dataManager.SpeedUp(collision);
            StartCoroutine(SpeedUpFireInterval());

        }
        else if (collision.gameObject.tag.Equals("SmallObj"))
        {
            Debug.Log("Collision:" + collision.gameObject.tag.ToString());
            dataManager.BecomeSmall(collision);
            StartCoroutine(BecomeSmaller());
        }
        else if (collision.gameObject.tag.Equals("ShieldObj"))
        {

            Debug.Log("Collision:" + collision.gameObject.tag.ToString());
            dataManager.WithShield(collision);
            StartCoroutine(ArmWithShield());
        }
    }

    IEnumerator SpeedUpFireInterval()
    {
        fireInterval = 0.3f;
        yield return new WaitForSeconds(speedUpTime);
        fireInterval = 2.0f;
    }
    IEnumerator BecomeSmaller()
    {
        if (!isSmall)
        {
            isSmall = true;
            this.transform.localScale = this.transform.localScale * 0.5f;
            yield return new WaitForSeconds(smallInterval);
            this.transform.localScale = this.transform.localScale * 2.0f;
            isSmall = false;
        }

    }
    IEnumerator ArmWithShield()
    {
        //Bool -> isShield with isShield = flase, the enememy can damage player
        Debug.Log("Shield");
        yield return new WaitForSeconds(shieldInterval);
        Debug.Log("No Shield");
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (barrel)
            Gizmos.DrawSphere(barrel.position, 0.1f);
    }
#endif
}
