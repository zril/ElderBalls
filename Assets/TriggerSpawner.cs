using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpawner : MonoBehaviour {


    private int triggers = 0;
    private float triggersTimer = 0;
    private float triggersPeriod = 0.15f;

    // Use this for initialization
    void Start () {
        triggers = 20;
    }
	
	// Update is called once per frame
	void Update () {
        if (triggers > 0)
        {
            triggersTimer -= Time.deltaTime;
            if (triggersTimer < 0)
            {
                trigger();
                triggersTimer += triggersPeriod;
                triggers--;
            }
        }
    }

    private void trigger()
    {
        var pos = new Vector3(-6 + Random.value * 12, -3 + Random.value * 6, 0);
        var ball = Instantiate(Resources.Load("TriggerBall"), pos, Quaternion.identity) as GameObject;
        var ballscript = ball.GetComponent<TriggerBall>();
        ballscript.playerNumber = 0;
        ballscript.SetThrowTimer(0.5f);
        ballscript.SetTarget(pos);
    }
}
