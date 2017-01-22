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
        Debug.Log("Super Bomb !");
    }

    protected override void superKnife()
    {
        Vector2 direction;
        if(playerNumber == 1)
        {
            direction = Vector2.left;
        }
        else
        {
            direction = Vector2.right;
        }

        Debug.Log("SuperKnife Active ");
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
        Debug.Log("Super Potion !");
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

	}
}
