using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBall : MonoBehaviour
{

    public float triggerRadius = 1f;
    public float throwTime = 1f;
    public float triggerMaxTimer;
    public int triggerSuperIncr = 1;

    public int playerNumber = 1;

    public bool isBlackHole = false;
    public bool blackHoleActive = false;
    public float blackHoleMaxTimer = 5.0f;
    public float blackHoleTriggerMaxTimer = 1.0f;
    public float blackHoleRadius = 7.0f;
    public float blackHoleMaxAttract = 1.0f;


    private float throwTimer;
    private Vector3 originPos;
    private Vector3 targetPos;
    private float baseScale = 0.125f;

    private float triggerTime;
    private float blackHoleTimer;
    private bool alive = true;

    private GameObject triggerTargetIndicator;

    private float timeOffset = 1;

    // Use this for initialization
    void Start()
    {
        originPos = transform.position;
        throwTimer = throwTime * timeOffset;
        transform.localScale = Vector3.one * baseScale;
        if (isBlackHole)
        {
            triggerMaxTimer = blackHoleTriggerMaxTimer;
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Trigger/blackhole");
        }

        triggerTargetIndicator = Instantiate(Resources.Load("TargetPrefab"), transform.position, Quaternion.identity) as GameObject;

    }

    // Update is called once per frame
    void Update()
    {
        triggerTargetIndicator.transform.localScale = IsOnGround() ? Vector3.zero : Vector3.one;
        triggerTargetIndicator.transform.position = targetPos;
        triggerTargetIndicator.transform.localScale = throwTimer / throwTime * Vector3.one;

        if (throwTimer >= 0)
        {
            throwTimer -= Time.deltaTime;
            if (throwTimer < 0)
            {
                GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/DetonatorLand"));
            }
            transform.position = Vector3.Lerp(targetPos, originPos, throwTimer / throwTime);

            var scale = 1 + 5 * (0.25f - Mathf.Pow((1 / throwTime) * (throwTimer - throwTime / 2), 2));
            transform.localScale = new Vector3(scale * baseScale, scale * baseScale, 1);
            transform.Rotate(Vector3.forward * 500 * Time.deltaTime);
            transform.position += Vector3.up * (scale - 1) * 0.5f;
        }
        else
        {
            transform.localScale = Vector3.one * baseScale;
            transform.position = targetPos;
            transform.Rotate(Vector3.forward * 1600 * Time.deltaTime);

            triggerTime += Time.deltaTime;
            if (triggerMaxTimer <= triggerTime)
            {
                if (isBlackHole)
                {
                    if (!blackHoleActive)
                    {
                        Detonate();
                    }

                    var balls = GameObject.FindGameObjectsWithTag("PlaceBall");
                    foreach (GameObject ball in balls)
                    {
                        var p1 = new Vector2(ball.transform.position.x, ball.transform.position.y);
                        var p2 = new Vector2(transform.position.x, transform.position.y);

                        if (Vector3.Distance(p1, p2) <= blackHoleRadius)
                        {
                            ball.GetComponent<Rigidbody2D>().velocity += (p2 - p1).normalized * blackHoleMaxAttract * (blackHoleRadius - Vector3.Distance(p1, p2));
                        }
                    }

                    blackHoleTimer += Time.deltaTime;
                    if (blackHoleTimer >= blackHoleMaxTimer)
                    {
                        GameObject.Destroy(gameObject);
                    }

                }
                else
                {
                    Detonate();

                }
            }
        }
    }

    private void Detonate()
    {
        if (alive)
        {
            alive = false;


            if (isBlackHole)
            {
                GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/BoomPotion"));
                GetComponent<AudioSource>().loop = true;
                GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sounds/SuperBlackHole");
                GetComponent<AudioSource>().Play();
                blackHoleActive = true;
            }
            else
            {
                if (!Global.konamiCodeActive)
                {
                    GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sounds/BoomPotion");
                    GetComponent<AudioSource>().Play();
                }
                var players = GameObject.FindGameObjectsWithTag("Player");
                Player targetPlayer = null;
                var currentPos = new Vector2(transform.position.x, transform.position.y);
                foreach (GameObject player in players)
                {
                    var playerScript = player.GetComponent<Player>();
                    if (player.GetComponent<Player>().playerNumber == playerNumber)
                    {
                        targetPlayer = playerScript;
                        playerScript.AddTriggerBall();
                    }

                    var p1 = new Vector2(player.transform.position.x, player.transform.position.y);
                    if(Vector3.Distance(p1,currentPos) < triggerRadius)
                    {
                        var pushDirection = p1 - currentPos;
                        playerScript.repel(pushDirection.normalized * (triggerRadius - Vector3.Distance(p1, currentPos)));
                    }

                }

                var balls = GameObject.FindGameObjectsWithTag("PlaceBall");
                foreach (GameObject ball in balls)
                {
                    var p1 = new Vector2(ball.transform.position.x, ball.transform.position.y);

                    if (Vector3.Distance(p1, currentPos) < triggerRadius)
                    {
                        if (targetPlayer != null)
                        {
                            targetPlayer.addSuper(triggerSuperIncr);
                        }
                        ball.GetComponent<PlaceBall>().Trigger();
                    }
                }

                var bonuses = GameObject.FindGameObjectsWithTag("Bonus");
                foreach (GameObject bonus in bonuses)
                {
                    var p1 = new Vector2(bonus.transform.position.x, bonus.transform.position.y);
                    if (Vector3.Distance(p1, currentPos) < triggerRadius)
                    {
                        bonus.GetComponent<Bonus>().Apply(playerNumber);
                    }
                }

                var events = GameObject.FindGameObjectsWithTag("Event");
                foreach (GameObject ev in events)
                {
                    var p1 = new Vector2(ev.transform.position.x, ev.transform.position.y);
                    var p2 = new Vector2(transform.position.x, transform.position.y);
                    if (Vector3.Distance(p1, p2) < triggerRadius)
                    {
                        ev.GetComponent<Event>().Activate();
                    }
                }

            }

            Destroy(GetComponent<SpriteRenderer>());
            var fx = Instantiate(Resources.Load("TriggerEffect"), transform.position, Quaternion.identity) as GameObject;
            fx.transform.localScale *= triggerRadius;
            if (isBlackHole)
            {
                fx.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Trigger/splash_6") as RuntimeAnimatorController;
                fx.transform.localScale *= 1.5f;
                fx.GetComponent<SpriteRenderer>().color = Color.black;
            }
            
            Destroy(fx, isBlackHole ? blackHoleMaxTimer : 0.25f);
        }
        else if (!GetComponent<AudioSource>().isPlaying)
        {
            GameObject.Destroy(triggerTargetIndicator);
            GameObject.Destroy(gameObject);
        }
    }

    public bool IsOnGround()
    {
        return throwTimer < 0;
    }

    public void SetTarget(Vector3 target)
    {
        targetPos = target;
    }

    public void Reset()
    {
        originPos = transform.position;
        triggerMaxTimer = triggerTime;
        throwTimer = throwTime;
        transform.localScale = Vector3.one * baseScale;
    }

    public void SetThrowTimer(float t)
    {
        timeOffset = t;
    }

    public bool isAlive()
    {
        return alive;
    }
}
