﻿using UnityEngine;
using System.Collections;

public class enemyFollowController : MonoBehaviour {

    public float enemySpeed;

    //facing variables
    public GameObject enemyGraphic;
    bool canFlip = true;
    bool facingRight = true;
    float flipTime = 5f;
    float nextFlipChance = 0f;

    //charging variables
    public float chargeTime;
    float startChargeTime;
    bool isCharging;
    Rigidbody2D enemyRB;

	//Sound Delaying Variables ((Created by Tyler for experimentation))
	private bool soundCanPlay = true;
	private float soundCanPlayTime;
	private float soundDelayTime = 5f;

	// Use this for initialization
	void Start ()
    {
        enemyRB = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Time.time > soundCanPlayTime) //Allowing the sound that was delayed to play again if the conditions are met.
		{
			soundCanPlay = true;
		}

	    if (Time.time > nextFlipChance)
        {
            if (Random.Range(0, 10) >= 5)
            {
                flipFacing();
            }
            nextFlipChance = Time.time + flipTime;
        }
        
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
			if(soundCanPlay)//Plays the sound if 'soundCanPlay' thinks it's ok. He knows best. ((This is to prevent sound spamming))
			{
				SoundManager.instance.playSoundEffect (5);
				soundCanPlay = false;
				soundCanPlayTime = Time.time + soundDelayTime;
			}

            if (facingRight && other.transform.position.x < transform.position.x)
            {
                flipFacing();
            }
            else if (!facingRight && other.transform.position.x > transform.position.x)
            {
                flipFacing();
            }
            canFlip = false;
            isCharging = true;
            startChargeTime = Time.time + chargeTime;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(startChargeTime < Time.time)
            {
                if (!facingRight)
                {
                    enemyRB.AddForce(new Vector2(-1, 0) * enemySpeed);
                }
                else
                {
                    enemyRB.AddForce(new Vector2(1, 0) * enemySpeed);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canFlip = true;
            isCharging = false;
            enemyRB.velocity = new Vector2(0f, 0f);
        }
    }

    void flipFacing()
    {
        if (!canFlip) return;
        float facingX = enemyGraphic.transform.localScale.x;
        facingX *= -1f;
        enemyGraphic.transform.localScale = new Vector3(facingX, enemyGraphic.transform.localScale.y, enemyGraphic.transform.localScale.z);
        facingRight = !facingRight;
    }
}
