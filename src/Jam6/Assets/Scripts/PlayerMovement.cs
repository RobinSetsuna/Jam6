﻿using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2.0f;
    public float rotationSpeed = 250.0f;
    public ItemDataManager dataManager;
    public float speedUpTime = 3.0f;
    public float smallInterval = 3.0f;
    public float shieldInterval = 3.0f;

    [SerializeField] private Transform[] barrels = new Transform[1];

    public bool screenWrap = false;

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

        Player.Singleton.MainWeapon.Fire(ref t, barrels, Player.Singleton.Energy);

        t -= Time.deltaTime;
    }

    void Move()
    {
        GetComponent<Animator>().SetInteger("state", PLAYER_IDLE);

        Vector3 upVector = transform.up;
        Vector3 rightVector = transform.right;

        if (Input.GetKey(KeyCode.W))
            transform.position += upVector * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.S))
            transform.position -= upVector * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Animator>().SetInteger("state", PLAYER_LEFT);
            transform.position -= rightVector * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Animator>().SetInteger("state", PLAYER_RIGHT);
            transform.position += rightVector * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));

        if(Input.GetKey(KeyCode.E))
            transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime));
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
        yield return new WaitForSeconds(speedUpTime);
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
        for (int i = 0; i < barrels.Length; i++)
            if (barrels[i])
                Gizmos.DrawSphere(barrels[i].position, 0.1f);
    }
#endif
}
