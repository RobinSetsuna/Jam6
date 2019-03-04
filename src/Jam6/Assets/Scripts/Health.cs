using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour {

    public int health;
    public int numOfHealth;


    public Image[] myhealthbars;
    public Sprite full;
    public Sprite empty;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (health > numOfHealth)
        {
            health = numOfHealth;
        }

        for (int i = 0; i < myhealthbars.Length; i++)
        {
            if (i < health)
            {
                myhealthbars[i].sprite = full;
            }
            else
            {
                myhealthbars[i].sprite = empty;
            }

            if(i<numOfHealth)
            {
                myhealthbars[i].enabled = true;
            }
            else
            {
                myhealthbars[i].enabled = false;
            }
        }
		
	}
}
