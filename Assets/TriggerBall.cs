using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBall : MonoBehaviour {

    public float triggerRadius = 1f;
    public float throwTime = 1f;
    public float triggerTime = 0.2f;

    public int playerNumber = 1;
    
    private float throwTimer;
    private Vector3 originPos;
    private Vector3 targetPos;
    private float baseScale = 0.125f;

    private float triggerTimer;

    // Use this for initialization
    void Start () {
        originPos = transform.position;
        triggerTimer = triggerTime;
        throwTimer = throwTime;
        transform.localScale = Vector3.one * baseScale;
    }
	
	// Update is called once per frame
	void Update () {
        if (throwTimer >= 0)
        {
            throwTimer -= Time.deltaTime;
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

            triggerTimer -= Time.deltaTime;
            if (triggerTimer < 0)
            {
                GameObject.Destroy(gameObject);
                Detonate();
            }
        }
    }

    private void Detonate()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            var playerScript = player.GetComponent<Player>();
            if (player.GetComponent<Player>().playerNumber == playerNumber)
            {
                playerScript.AddTriggerBall();
            }
        }

        var balls = GameObject.FindGameObjectsWithTag("PlaceBall");
        foreach(GameObject ball in balls)
        {
            var p1 = new Vector2(ball.transform.position.x, ball.transform.position.y);
            var p2 = new Vector2(transform.position.x, transform.position.y);
            if (Vector3.Distance(p1, p2) < triggerRadius)
            {
                ball.GetComponent<PlaceBall>().Trigger();
            }
        }

        var fx = Instantiate(Resources.Load("TriggerEffect"), transform.position, Quaternion.identity) as GameObject;
        fx.transform.localScale *= triggerRadius;
        Destroy(fx, 0.05f);
    }

    public void SetTarget(Vector3 target)
    {
        targetPos = target;
    }
}
