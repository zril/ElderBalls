﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBall : MonoBehaviour {

    public float triggerRadius = 1f;
    public float throwTime = 1f;
    
    private float throwTimer;
    private Vector3 originPos;
    private Vector3 targetPos;

    // Use this for initialization
    void Start () {
        originPos = transform.position;

        throwTimer = throwTime;
    }
	
	// Update is called once per frame
	void Update () {
        if (throwTimer >= 0)
        {
            throwTimer -= Time.deltaTime;
            transform.position = Vector3.Lerp(targetPos, originPos, throwTimer / throwTime);

            var scale = 1 + 5 * (0.25f - Mathf.Pow((1 / throwTime) * (throwTimer - throwTime / 2), 2));
            transform.localScale = new Vector3(scale, scale, 1);

            transform.position += Vector3.up * (scale - 1) * 0.5f;
        }
        else
        {
            transform.localScale = Vector3.one;
            transform.position = targetPos;

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

    public void SetTarget(Vector3 target)
    {
        targetPos = target;
    }
}