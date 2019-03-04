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
    public GameObject bullet;
    private float previousTime;
    private bool isSmall = false;


    // Use this for initialization
    void Start()
    {
        previousTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if ((Time.time - previousTime) > fireInterval)
        {
            Shooting();
            previousTime = Time.time;
        }
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

    void Shooting()
    {
        if (ObjectRecycler.Singleton)
        {
            Recyclable bullet = ObjectRecycler.Singleton.GetObject(0);
            bullet.GetComponent<LinearMovement>().initialPosition = transform.position + Vector3.up * 0.8f;

            bullet.gameObject.SetActive(true);
        }
        else
            ObjectPool.GetInstance().GetObj("Bullet", this.transform.position + Vector3.up * 2.0f);
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
}
