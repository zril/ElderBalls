using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    public int playerNumber = 1;
    public float moveSpeed = 2;
    public float ballSpeedBase = 2;
    public float ballSpeedFactor = 2;
    public float ballDistBase = 2;
    public float ballDistFactor = 2;

    string xAxis;
    string yAxis;
    string placeButton;
    string triggerButton;
    string superButton;


    private float chargeTimer = 0;
    private float placeChargeTimer = 0;
    private float triggerChargeTimer = 0;

    // Use this for initialization
    void Start()
    {
        if (playerNumber == 1)
        {
            xAxis = "P1_Horizontal";
            yAxis = "P1_Vertical";
            placeButton = "P1_Place";
            triggerButton = "P1_Trigger";
            superButton = "P1_Super";
        }
        else
        {
            xAxis = "P2_Horizontal";
            yAxis = "P2_Vertical";
            placeButton = "P2_Place";
            triggerButton = "P2_Trigger";
            superButton = "P2_Super";
        }

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

        chargeTimer += Time.deltaTime;

        Vector3 movement = new Vector3(horizontal, vertical, 0);
        if (movement.magnitude > 1) movement.Normalize();

        transform.localPosition += movement * moveSpeed * Time.deltaTime;

        Vector3 angle = new Vector3(horizontal, vertical, 0).normalized;

        Debug.Log(angle);
        var rad = Mathf.Atan2(angle.y, angle.x);
        Debug.Log(rad * 180 / Mathf.PI);

        if (placeUp)
        {
            Debug.Log(placeChargeTimer);
            var ball = Instantiate(Resources.Load("PlaceBall"), transform.position, Quaternion.Euler(0, 0, -90 + rad * 180 / Mathf.PI)) as GameObject;
            var ballscript = ball.GetComponent<PlaceBall>();
            ballscript.speed = ballSpeedBase + ballSpeedFactor * placeChargeTimer;
        }
        if (triggerUp)
        {
            Debug.Log(triggerChargeTimer);
            var target = angle.normalized * (ballDistBase + ballDistFactor * triggerChargeTimer);
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