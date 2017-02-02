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


    public bool magnetic = false;
    public float magnetRadius = 2.0f;
    public float magnetAttract = 0.2f;
    public float magnetTriggerMaxTimer = 3.0f;



    public int playerNumber = 1;

    private bool alive = true;
    private float magnetTimer;
    private float triggerTimer;
    private bool trigger = false;
    private AudioClip collideClip;
    private AudioClip detonateClip;
    private AudioClip magneticClip;

    private float powerFactor = 1;

    // Use this for initialization
    void Start () {
        triggerTimer = 0;
        transform.GetComponent<Rigidbody2D>().velocity = transform.up * startSpeed;
        collideClip = Resources.Load<AudioClip>("Sounds/BallCollision");
        detonateClip = Resources.Load<AudioClip>("Sounds/BoomBomb");
        if (magnetic)
        {
            GetComponent<Rigidbody2D>().mass = 100.0f;
            GetComponent<Animator>().runtimeAnimatorController = Resources.Load("PlaceBall/magnet_0") as RuntimeAnimatorController;
        }
        magneticClip = Resources.Load<AudioClip>("Sounds/SuperMagnet");
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

        transform.Rotate(new Vector3(0, 0, GetComponent<Rigidbody2D>().velocity.magnitude * 400 * Time.deltaTime));

        if (trigger)
        {
            triggerTimer += Time.deltaTime;
        }
        if (triggerTimer > triggerTime)
        {
            Detonate();
        }

        //Super Ballz
        if(magnetic && alive)
        {
            if (!GetComponents<AudioSource>()[1].isPlaying)
            {
                GetComponents<AudioSource>()[1].clip = magneticClip;
                GetComponents<AudioSource>()[1].Play();
            }
            //attract
            var balls = GameObject.FindGameObjectsWithTag("PlaceBall");
            foreach (GameObject ball in balls)
            {
                var p1 = new Vector2(ball.transform.position.x, ball.transform.position.y);
                var p2 = new Vector2(transform.position.x, transform.position.y);
                if (Vector3.Distance(p1, p2) < magnetRadius && Vector3.Distance(p1, p2) > 0)
                {
                    ball.GetComponent<Rigidbody2D>().velocity += (p2 - p1).normalized * magnetAttract;
                }
            }

            magnetTimer += Time.deltaTime;
            if(magnetTimer >= magnetTriggerMaxTimer )
            {
                Trigger();
            }
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
            if(magnetic && GetComponents<AudioSource>()[1].isPlaying)
            {
                GetComponents<AudioSource>()[1].Stop();
            }
            GetComponents<AudioSource>()[0].clip = detonateClip;
            GetComponents<AudioSource>()[0].pitch += (Random.value - 0.5f) * audioPitchRange * 2;
            GetComponents<AudioSource>()[0].Play();

            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                var playerScript = player.GetComponent<Player>();
                if (player.GetComponent<Player>().playerNumber == playerNumber)
                {
                    playerScript.AddPlaceBall();
                }
            }
            var wave = Instantiate(Resources.Load("ShockWave"), transform.position, Quaternion.identity) as GameObject;
            wave.GetComponent<Shockwave>().SetPowerFactor(powerFactor);
            Vector3 cratpos = transform.position;
            cratpos.z++;
            var crat = Instantiate(Resources.Load("Cratere"), cratpos, Quaternion.identity);
            var fx = Instantiate(Resources.Load("PlaceBall/DetonateEffect"), transform.position, Quaternion.identity);
            Destroy(fx, 0.05f);
            Destroy(GetComponent<CircleCollider2D>());
            Destroy(GetComponent<SpriteRenderer>());
        }
        else
        {
            if (!GetComponents<AudioSource>()[0].isPlaying)
            {
                GameObject.Destroy(gameObject);
                
            }
        }

    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "PlaceBall")
        {
            if(magnetic)
            {
                other.rigidbody.velocity = new Vector2();
            }
            else if (GetComponent<Rigidbody2D>().velocity.magnitude < other.rigidbody.velocity.magnitude)
            {
                GetComponents<AudioSource>()[0].clip = collideClip;
                GetComponents<AudioSource>()[0].Play();
                GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + GetComponent<Rigidbody2D>().velocity.normalized * ballBounceModifier;
            }
        }
        else if (other.gameObject.tag == "Wall")
        {
            GetComponents<AudioSource>()[0].clip = collideClip;
            GetComponents<AudioSource>()[0].Play();
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + GetComponent<Rigidbody2D>().velocity.normalized * wallBounceModifier;
        }
        else if (other.gameObject.tag == "Goal")
        {
            GetComponents<AudioSource>()[0].clip = collideClip;
            GetComponents<AudioSource>()[0].Play();
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

    public void SetPowerFactor(float factor)
    {
        powerFactor = factor;
    }
    
}
