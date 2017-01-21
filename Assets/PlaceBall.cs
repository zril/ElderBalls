using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBall : MonoBehaviour {

    //public float speed = 2;
    public float startSpeed = 0;

    //public float friction = 10f;
    //public float frictionBase = 0.5f;
    public float triggerTime = 0.5f;
    public float ballBounceModifier = 2.0f;
    public float wallBounceModifier = 2.0f;
    public float goalBounceModifier = 2.0f;

    public int playerNumber = 1;


    private float triggerTimer;
    private bool trigger = false;

    // Use this for initialization
    void Start () {
        triggerTimer = 0;
        transform.GetComponent<Rigidbody2D>().velocity = transform.up * startSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        /*
        transform.localPosition += transform.up * Time.deltaTime * startSpeed;
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
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            var playerScript = player.GetComponent<Player>();
            if (player.GetComponent<Player>().playerNumber == playerNumber)
            {
                playerScript.AddPlaceBall();
            }
        }
        Instantiate(Resources.Load("ShockWave"), transform.position, Quaternion.identity);
        GameObject.Destroy(gameObject);
        var fx = Instantiate(Resources.Load("DetonateEffect"), transform.position, Quaternion.identity);
        Destroy(fx, 0.05f);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "PlaceBall")
        {
            if (GetComponent<Rigidbody2D>().velocity.magnitude < other.rigidbody.velocity.magnitude)
            {
                GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + GetComponent<Rigidbody2D>().velocity.normalized * ballBounceModifier;
            }
        }
        else if (other.gameObject.tag == "Wall")
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + GetComponent<Rigidbody2D>().velocity.normalized * wallBounceModifier;
        }
        else if (other.gameObject.tag == "Goal")
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + GetComponent<Rigidbody2D>().velocity.normalized * goalBounceModifier;
        }
    }
}
