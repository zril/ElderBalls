using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBall : MonoBehaviour {

    public float triggerRadius = 1f;
    private float triggerTimer;

	// Use this for initialization
	void Start () {
        triggerTimer = 1f;
    }
	
	// Update is called once per frame
	void Update () {
        triggerTimer -= Time.deltaTime;
        if (triggerTimer < 0)
        {
            GameObject.Destroy(gameObject);
            Detonate();
        }
    }

    private void Detonate()
    {
        var balls = GameObject.FindGameObjectsWithTag("PlaceBall");

        foreach(GameObject ball in balls)
        {
            if (Vector3.Distance(ball.transform.position, transform.position) < triggerRadius)
            {
                ball.GetComponent<PlaceBall>().Trigger();
            }
        }
    }
}
