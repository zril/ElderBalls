using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBall : MonoBehaviour {

    public float speed = 2;
    public float startSpeed = 0;

    //public float friction = 10f;
    //public float frictionBase = 0.5f;
    public float triggerTime = 0.5f;


    private float triggerTimer;
    private bool trigger = false;

    // Use this for initialization
    void Start () {
        triggerTimer = 0;
        transform.GetComponent<Rigidbody2D>().velocity = transform.up * startSpeed * speed;
    }
	
	// Update is called once per frame
	void Update () {
        /*
        transform.localPosition += transform.up * Time.deltaTime * speed;
        speed -= (frictionBase + friction * speed) * Time.deltaTime;
        if (startSpeed < 0)
        {
            startSpeed = 0;
        }
        */


        if (trigger)
        {
            triggerTimer += Time.deltaTime;
        }
        if (triggerTimer > triggerTime)
        {
            Detonate();
        }
    }

    public void Trigger()
    {
        trigger = true;
    }

    private void Detonate()
    {
        Instantiate(Resources.Load("ShockWave"), transform.position, Quaternion.identity);
        GameObject.Destroy(gameObject);
    }
}
