using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2.0f;
    public ItemDataManager dataManager;
    public float fireInterval = 2.0f;
    public float speedUpTime = 3.0f;
    public float smallInterval = 3.0f;
    public float shieldInterval = 3.0f;

    [SerializeField] private Transform barrel;
    [SerializeField] private int bulletID = 0;

    private float t = 0;

    private bool isSmall = false;

    // Use this for initialization
    void OnEnable()
    {
        t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        LinearMovement bullet;
        while (t <= 0)
        {
            bullet = ObjectRecycler.Singleton.GetObject<LinearMovement>(bulletID);

            bullet.initialPosition = barrel.position;
            bullet.spawnTime = Time.time + t;

            bullet.gameObject.SetActive(true);

            t += fireInterval;
        }

        t -= Time.deltaTime;
    }
    void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += Vector3.up * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += Vector3.down * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += Vector3.right * speed * Time.deltaTime;
        }
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
