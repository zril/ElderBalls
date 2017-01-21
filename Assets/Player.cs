﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    public int playerNumber = 1;
    public float moveSpeed = 2;
    public float ballSpeedBase = 2;
    public float ballSpeedFactor = 2;
    public float ballSpeedMax = 10;
    public float ballDistBase = 2;
    public float ballDistFactor = 2;
    public float ballDistMax = 10;

    string xAxis;
    string yAxis;
    string placeButton;
    string triggerButton;
    string superButton;


    private GameObject directionElement;
    private float placeChargeTimer = 0;
    private float triggerChargeTimer = 0;
    private Vector3 currentAngle;

    // Use this for initialization
    void Start()
    {
        Vector3 directionPosition;
        if (playerNumber == 1)
        {
            xAxis = "P1_Horizontal";
            yAxis = "P1_Vertical";
            placeButton = "P1_Place";
            triggerButton = "P1_Trigger";
            superButton = "P1_Super";
            currentAngle = new Vector3(1, 0);
        }
        else
        {
            xAxis = "P2_Horizontal";
            yAxis = "P2_Vertical";
            placeButton = "P2_Place";
            triggerButton = "P2_Trigger";
            superButton = "P2_Super";
            currentAngle = new Vector3(-1, 0);
        }

        directionElement = Instantiate(Resources.Load("Direction"), transform.position + currentAngle, Quaternion.identity) as GameObject;

    }

    // Update is called once per frame
    void Update()
    {


        float horizontal = Input.GetAxis(xAxis);
        float vertical = Input.GetAxis(yAxis);
        bool IsPlaceButton = Input.GetButton(placeButton);
        bool placeUp = Input.GetButtonUp(placeButton);
        bool IsTriggerButton = Input.GetButton(triggerButton);
        bool triggerUp = Input.GetButtonUp(triggerButton);
        bool super = Input.GetButtonDown(superButton);
        

        Vector3 movement = new Vector3(horizontal, vertical, 0);
        if (movement.magnitude > 1) movement.Normalize();

        if (!IsPlaceButton && !IsTriggerButton)
        {
            transform.localPosition += movement * moveSpeed * Time.deltaTime;
        }

        if (horizontal != 0 || vertical != 0)
        {
            currentAngle = new Vector3(horizontal, vertical, 0).normalized;
        }
        
        directionElement.transform.position = transform.localPosition + (currentAngle / 2.0f);

        Debug.Log(currentAngle);
        var rad = Mathf.Atan2(currentAngle.y, currentAngle.x);
        Debug.Log(rad * 180 / Mathf.PI);

        if (placeUp)
        {
            Debug.Log(placeChargeTimer);
            var ball = Instantiate(Resources.Load("PlaceBall"), transform.position, Quaternion.Euler(0, 0, -90 + rad * 180 / Mathf.PI)) as GameObject;
            var ballscript = ball.GetComponent<PlaceBall>();
            ballscript.startSpeed = Mathf.Min(ballSpeedBase + ballSpeedFactor * placeChargeTimer,ballSpeedMax);
        }
        if (triggerUp)
        {
            Debug.Log(triggerChargeTimer);
            var target = currentAngle.normalized * (Mathf.Min(ballDistBase + ballDistFactor * triggerChargeTimer,ballDistMax));
            var ball = Instantiate(Resources.Load("TriggerBall"), transform.position + target, Quaternion.identity) as GameObject;
        }


        if (IsPlaceButton && triggerChargeTimer == 0)
        {
            placeChargeTimer += Time.deltaTime;
        }
        else
        {
            placeChargeTimer = 0;
        }

        if (IsTriggerButton && placeChargeTimer == 0)
        {
            triggerChargeTimer += Time.deltaTime;
        }
        else
        {
            triggerChargeTimer = 0;
        }

        
    }
}