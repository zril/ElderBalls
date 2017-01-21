using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBall : MonoBehaviour {

    public float speed = 2;

    public float friction = 10f;
    public float frictionBase = 0.5f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition += transform.up * Time.deltaTime * speed;
        speed -= (frictionBase + friction * speed) * Time.deltaTime;
        if (speed < 0)
        {
            speed = 0;
        }
    }

    public void Trigger()
    {
        Instantiate(Resources.Load("ShockWave"), transform.position, Quaternion.identity);
    }
}
