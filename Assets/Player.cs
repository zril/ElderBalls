using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private GameObject placeChargeIndicator;
    private GameObject triggerChargeIndicator;
    private float placeChargeTimer = 0;
    private float triggerChargeTimer = 0;
    private Vector3 currentAngle;

    private int hp;
    private int placeBallCount;
    private int triggerBallCount;

    // Use this for initialization
    void Start()
    {
        hp = 51;
        placeBallCount = 5;
        triggerBallCount = 1;
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
        placeChargeIndicator = Instantiate(Resources.Load("charge"), transform.position, Quaternion.identity) as GameObject;
        triggerChargeIndicator = Instantiate(Resources.Load("charge"), transform.position, Quaternion.identity) as GameObject;

        if (playerNumber == 2)
        {
            transform.Rotate(new Vector3(0, 180, 0));
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
        

        Vector3 movement = new Vector3(horizontal, vertical, 0);
        if (movement.magnitude > 1) movement.Normalize();

        if (!IsPlaceButton && !IsTriggerButton)
        {
            float maxX = 0;
            float minX = -6;
            if (playerNumber == 2)
            {
                maxX = 6;
                minX = 0;
            }
            transform.localPosition += movement * moveSpeed * Time.deltaTime;
            var pos = transform.localPosition;
            if (transform.localPosition.x > maxX)
            {
                transform.localPosition = new Vector3(maxX, pos.y, pos.z);
            }
            if (transform.localPosition.x < minX)
            {
                transform.localPosition = new Vector3(minX, pos.y, pos.z);
            }
        }
        
        if (horizontal != 0 || vertical != 0)
        {
            currentAngle = new Vector3(horizontal, vertical, 0).normalized;
        }


        var rad = Mathf.Atan2(currentAngle.y, currentAngle.x);

        directionElement.transform.position = transform.localPosition + (currentAngle / 2.0f);
        placeChargeIndicator.transform.position = transform.localPosition;
        triggerChargeIndicator.transform.position = transform.localPosition;


        float placeChargePct = Mathf.Min(ballSpeedFactor * placeChargeTimer, ballSpeedMax - ballSpeedBase) / (ballSpeedMax - ballSpeedBase);
        placeChargeIndicator.transform.localScale = new Vector3(placeChargePct, placeChargePct, placeChargePct);
        placeChargeIndicator.transform.rotation = Quaternion.Euler(0, 0, rad * 180 / Mathf.PI);


        float triggerChargePct = Mathf.Min(ballDistFactor * triggerChargeTimer, ballDistMax - ballDistBase) / (ballDistMax - ballDistBase);
        triggerChargeIndicator.transform.localScale = new Vector3(triggerChargePct, triggerChargePct, triggerChargePct);
        triggerChargeIndicator.transform.rotation = Quaternion.Euler(0, 0, rad * 180 / Mathf.PI);



        if (placeUp && placeChargeTimer > 0)
        {
            var ball = Instantiate(Resources.Load("PlaceBall/PlaceBall"), Vector3.forward + transform.position + currentAngle.normalized * 0.4f, Quaternion.Euler(0, 0, -90 + rad * 180 / Mathf.PI)) as GameObject;
            var ballscript = ball.GetComponent<PlaceBall>();
            ballscript.startSpeed = Mathf.Min(ballSpeedBase + ballSpeedFactor * placeChargeTimer,ballSpeedMax);
            ballscript.playerNumber = playerNumber;
            RemovePlaceBall();
        }
        if (triggerUp && triggerChargeTimer > 0)
        {
            var target = currentAngle.normalized * (Mathf.Min(ballDistBase + ballDistFactor * triggerChargeTimer,ballDistMax));
            var ball = Instantiate(Resources.Load("TriggerBall"), transform.position, Quaternion.identity) as GameObject;
            var ballscript = ball.GetComponent<TriggerBall>();
            ballscript.SetTarget(transform.position + target);
            ballscript.playerNumber = playerNumber;
            RemoveTriggerBall();
        }


        if (IsPlaceButton && triggerChargeTimer == 0 && placeBallCount > 0)
        {
            placeChargeTimer += Time.deltaTime;
        }
        else
        {
            placeChargeTimer = 0;
        }

        if (IsTriggerButton && placeChargeTimer == 0 && triggerBallCount > 0)
        {
            triggerChargeTimer += Time.deltaTime;
        }
        else
        {
            triggerChargeTimer = 0;
        }

        
    }

    public void Damage()
    {
        hp--;
        updateUI();
    }

    public void AddPlaceBall()
    {
        placeBallCount++;
        updateUI();
    }

    public void RemovePlaceBall()
    {
        placeBallCount--;
        updateUI();
    }

    public void AddTriggerBall()
    {
        triggerBallCount++;
    }

    public void RemoveTriggerBall()
    {
        triggerBallCount--;
    }

    private void updateUI()
    {
        var canvas = GameObject.FindGameObjectWithTag("Canvas");
        var ui = canvas.transform.FindChild("Player" + playerNumber);
        var uihp = ui.transform.FindChild("Hp");
        var text = uihp.GetComponentInChildren<Text>();
        text.text = "" + hp;

        var uiballs = ui.transform.FindChild("Balls");
        for (int i = 0; i < 5; i++)
        {
            var ball = uiballs.FindChild("Ball" + (i + 1));
            ball.gameObject.SetActive(i < placeBallCount);
        }
    }
}