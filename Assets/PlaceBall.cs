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
    public float maxSpeed = 20.0f;
    public float audioPitchRange = 0.2f;
    


    public int playerNumber = 1;

    private bool alive = true;
    private float triggerTimer;
    private bool trigger = false;
    private AudioClip collideClip;
    private AudioClip detonateClip;

    // Use this for initialization
    void Start () {
        triggerTimer = 0;
        transform.GetComponent<Rigidbody2D>().velocity = transform.up * startSpeed;
        collideClip = Resources.Load<AudioClip>("Sounds/BallCollision");
        detonateClip = Resources.Load<AudioClip>("Sounds/Boom1 - Dark");
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
        if(GetComponent<Rigidbody2D>().velocity.magnitude > maxSpeed)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * maxSpeed;
        }

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

        gameObject.GetComponent<Animator>().SetBool("detonate", true);
    }

    private void Detonate()
    {
        if (alive)
        {
            alive = false;
            GetComponent<AudioSource>().pitch += (Random.value - 0.5f) * audioPitchRange * 2;
            GetComponent<AudioSource>().PlayOneShot(detonateClip);
            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                var playerScript = player.GetComponent<Player>();
                if (player.GetComponent<Player>().playerNumber == playerNumber)
                {
                    playerScript.AddPlaceBall();
                }
            }
            Instantiate(Resources.Load("ShockWave"), transform.position, Quaternion.identity);
            Vector3 cratpos = transform.position;
            cratpos.z--;
            var crat = Instantiate(Resources.Load("Cratere"), cratpos, Quaternion.identity);
            Destroy(crat, 5f);
            var fx = Instantiate(Resources.Load("PlaceBall/DetonateEffect"), transform.position, Quaternion.identity);
            Destroy(fx, 0.05f);
            Destroy(GetComponent<CircleCollider2D>());
            Destroy(GetComponent<SpriteRenderer>());
        }
        else
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GameObject.Destroy(gameObject);
                
            }
        }

    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "PlaceBall")
        {
            if (GetComponent<Rigidbody2D>().velocity.magnitude < other.rigidbody.velocity.magnitude)
            {
                GetComponent<AudioSource>().PlayOneShot(collideClip);
                GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + GetComponent<Rigidbody2D>().velocity.normalized * ballBounceModifier;
            }
        }
        else if (other.gameObject.tag == "Wall")
        {
            GetComponent<AudioSource>().PlayOneShot(collideClip);
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + GetComponent<Rigidbody2D>().velocity.normalized * wallBounceModifier;
        }
        else if (other.gameObject.tag == "Goal")
        {
            GetComponent<AudioSource>().PlayOneShot(collideClip);
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + GetComponent<Rigidbody2D>().velocity.normalized * goalBounceModifier;
        }    
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "GameZone")
        {
            Detonate();
        }
    }

    
}
