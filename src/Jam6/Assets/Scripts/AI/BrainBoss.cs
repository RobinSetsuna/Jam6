﻿using System.Collections;
using UnityEngine;

public class BrainBoss : Enemy
{
    [SerializeField] private GameObject leftLaser;
    [SerializeField] private GameObject rightLaser;
    [SerializeField] private float fireInterval = 2f;
    [SerializeField] private int burstThreshold = 10000;

    private Vector3 orientation;

    private int currentBurstThreshold;
    private float t;
    private float tf1;
    private float tf2;
    private float tf3;

    private int of1;

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
                    case 1:
                        damageFactor = 1;
                        break;


                    case 3:
                        StartCoroutine(TurnOffLaser(2));
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
                        orientation = Vector3.down;
                        damageFactor = 0;
                        break;


                    case 2:
                        if (transform.position.x < -1.5f)
                            orientation = Vector3.right;
                        else if (transform.position.x > 1.5f)
                            orientation = Vector3.left;
                        else
                            orientation = Random.Range(0, 100) < 50 ? Vector3.left : Vector3.right;
                        of1 = Random.Range(0, 360);
                        t = 0;
                        break;


                    case 3:
                        t = 0;
                        leftLaser.SetActive(true);
                        rightLaser.SetActive(true);
                        break;


                    case 5:
                        t = 0;
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

        currentBurstThreshold = maxHp - burstThreshold;
    }

    private IEnumerator TurnOffLaser(float t)
    {
        yield return new WaitForSeconds(t);

        leftLaser.SetActive(false);
        rightLaser.SetActive(false);
    }

    private IEnumerator Burst(int r)
    {
        int R1 = 360 + r;
        int R2 = 365 + r;

        Recyclable bullet;
        LinearMovement bulletMovement;
        for (int o = r; o < R1; o += 10)
        {
            bullet = ObjectRecycler.Singleton.GetObject(2);
            bullet.lifeSpan = 20;
            bulletMovement = bullet.GetComponent<LinearMovement>();
            bulletMovement.initialPosition = transform.position;
            bulletMovement.orientation = MathUtility.GetOrientation(o);
            bulletMovement.speed = 1;
            bullet.gameObject.SetActive(true);
        }

        for (int o = r; o < R1; o += 10)
        {
            bullet = ObjectRecycler.Singleton.GetObject(2);
            bullet.lifeSpan = 10;
            bulletMovement = bullet.GetComponent<LinearMovement>();
            bulletMovement.initialPosition = transform.position;
            bulletMovement.orientation = MathUtility.GetOrientation(o);
            bulletMovement.speed = 2;
            bulletMovement.gameObject.SetActive(true);
        }

        for (int o = r; o < R1; o += 10)
        {
            bullet = ObjectRecycler.Singleton.GetObject(2);
            bullet.lifeSpan = 7;
            bulletMovement = bullet.GetComponent<LinearMovement>();
            bulletMovement.initialPosition = transform.position;
            bulletMovement.orientation = MathUtility.GetOrientation(o);
            bulletMovement.speed = 3;
            bulletMovement.gameObject.SetActive(true);
        }

        yield return null;

        for (int o = r + 5; o < R2; o += 10)
        {
            bullet = ObjectRecycler.Singleton.GetObject(2);
            bullet.lifeSpan = 14;
            bulletMovement = bullet.GetComponent<LinearMovement>();
            bulletMovement.initialPosition = transform.position;
            bulletMovement.orientation = MathUtility.GetOrientation(o);
            bulletMovement.speed = 1.5f;
            bulletMovement.gameObject.SetActive(true);
        }

        for (int o = r + 5; o < R2; o += 10)
        {
            bullet = ObjectRecycler.Singleton.GetObject(2);
            bullet.lifeSpan = 8;
            bulletMovement = bullet.GetComponent<LinearMovement>();
            bulletMovement.initialPosition = transform.position;
            bulletMovement.orientation = MathUtility.GetOrientation(o);
            bulletMovement.speed = 2.5f;
            bulletMovement.gameObject.SetActive(true);
        }

        yield break;
    }

    private void Update()
    {
        t += Time.deltaTime;

        if (transform.position.x < -5f)
            orientation = Vector3.right;
        else if (transform.position.x > 5f)
            orientation = Vector3.left;

        Recyclable bullet;
        LinearMovement bulletMovement;

        while (tf1 <= 0)
        {
            int r = Random.Range(0, 30);
            for (int o = r; o < 360 + r; o += 30)
            {
                bullet = ObjectRecycler.Singleton.GetObject(2);
                bullet.lifeSpan = 3;
                bulletMovement = bullet.GetComponent<LinearMovement>();
                bulletMovement.initialPosition = transform.position;
                bulletMovement.orientation = MathUtility.GetOrientation(o);
                bulletMovement.speed = 7;
                bulletMovement.spawnTime = Time.time + tf1;
                bullet.gameObject.SetActive(true);
            }

            tf1 += fireInterval;
        }

        tf1 -= Time.deltaTime;

        if (hp < currentBurstThreshold)
        {
            CurrentState = 6;
            currentBurstThreshold -= 10000;
        }

        switch (currentState)
        {
            case 1:
                transform.position += orientation * speed * Time.deltaTime;
                if (transform.position.y - 6f < 0.1f)
                    CurrentState = 2;
                break;


            case 2:
                if (Time.time - tf2 > 0.02f)
                {
                    bullet = ObjectRecycler.Singleton.GetObject(2);
                    bullet.lifeSpan = 4;
                    bulletMovement = bullet.GetComponent<LinearMovement>();
                    bulletMovement.initialPosition = transform.position;
                    bulletMovement.orientation = MathUtility.GetOrientation(of1);
                    bulletMovement.speed = 5;
                    bullet.gameObject.SetActive(true);

                    of1 += 10;
                    tf2 = Time.time;
                }

                transform.Translate(orientation * speed * Time.deltaTime);

                if (t > 5)
                    CurrentState = 4;
                break;


            //case 3:
            //    {
            //        float dx = Player.Singleton.transform.position.x - transform.position.x;

            //        Vector3 v;
            //        if (dx < -3f)
            //            v = Vector3.left;
            //        else if (dx > 3f)
            //            v = Vector3.right;
            //        else
            //            v = orientation;

            //        transform.Translate(v * speed / 2 * Time.deltaTime);
            //    }

            //    if (t > 3)
            //        CurrentState = Random.Range(1, 4) * 2;
            //    break;


            case 4:
                if (transform.position.x > 0.1f)
                    transform.Translate(Vector3.left * speed / 2 * Time.deltaTime);
                else if (transform.position.x < -0.1f)
                    transform.Translate(Vector3.right * speed / 2 * Time.deltaTime);
                else
                    CurrentState = 5;
                break;


            case 5:
                if (t < 8 && Time.time - tf3 > 1.5f)
                {
                    int r = 180 + Random.Range(23, 37) * 3;

                    for (int o = 180; o <= 360; o += 2)
                        if (Mathf.Abs(o - r) > 4)
                        {
                            bullet = ObjectRecycler.Singleton.GetObject(2);
                            bullet.lifeSpan = 10;
                            bulletMovement = bullet.GetComponent<LinearMovement>();
                            bulletMovement.initialPosition = transform.position;
                            bulletMovement.orientation = MathUtility.GetOrientation(o);
                            bulletMovement.speed = 2;
                            bullet.gameObject.SetActive(true);
                        }

                    tf3 = Time.time;
                }

                if (t > 10)
                    CurrentState = 2;
                break;


            case 6:
                if (transform.position.x > 0.1f)
                    transform.Translate(Vector3.left * speed / 2 * Time.deltaTime);
                else if (transform.position.x < -0.1f)
                    transform.Translate(Vector3.right * speed / 2 * Time.deltaTime);
                else
                    CurrentState = 7;
                break;


            case 7:
                StartCoroutine(Burst(Random.Range(0, 5)));
                CurrentState = 2;
                break;
        }
    }
}

