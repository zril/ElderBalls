using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMagnet : SuperBase {

    public float windFactor = 3.5f;
    public float windTime = 5.0f;

    private float windTimer = 0.0f;

    protected override void superBomb()
    {

        var rad = Mathf.Atan2(angle.y, angle.x);
        var ball = Instantiate(Resources.Load("PlaceBall/PlaceBall"), Vector3.forward + transform.position + angle.normalized * 0.4f, Quaternion.Euler(0, 0, -90 + rad * 180 / Mathf.PI)) as GameObject;
        var ballscript = ball.GetComponent<PlaceBall>();
        ballscript.startSpeed = startSpeed ;
        ballscript.playerNumber = playerNumber;
        ballscript.magnetic = true;
    }

    protected override void superKnife()
    {

        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sounds/SuperWind");
            GetComponent<AudioSource>().Play();
        }
        Vector2 direction;
        if(playerNumber == 1)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }

        Debug.Log("SuperKnife Active");
        var balls = GameObject.FindGameObjectsWithTag("PlaceBall");
        foreach (GameObject ball in balls)
        {
            ball.GetComponent<Rigidbody2D>().velocity += direction * windFactor * Time.deltaTime;
        }
        windTimer += Time.deltaTime;
        if(windTimer > windTime)
        {
            GameObject.Destroy(gameObject);
        }

    }

    protected override void superPotion()
    {
        var target = angle.normalized * startSpeed;
        var ball = Instantiate(Resources.Load("TriggerBall"), transform.position, Quaternion.identity) as GameObject;
        var ballscript = ball.GetComponent<TriggerBall>();
        ballscript.SetTarget(transform.position + target);
        ballscript.playerNumber = playerNumber;
        ballscript.isBlackHole = true;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    	if(knife)
        {
            superKnife();
        }

        if(bomb)
        {
            superBomb();
            GameObject.Destroy(gameObject);
        }

        if(potion)
        {
            superPotion();
            GameObject.Destroy(gameObject);
        }

	}
}
