﻿using System.Collections;
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
    public float pitchModifier = 0.1f;
    public int maxSuper = 100;
    public int damageBlockSuperIncr = 2;

    string xAxis;
    string yAxis;
    string placeButton;
    string triggerButton;
    string pushButton;
    string superButton;


    private GameObject directionElement;
    private GameObject placeChargeIndicator;
    private GameObject triggerChargeIndicator;
    private float placeChargeTimer = 0;
    private float triggerChargeTimer = 0;
    private float pushChargeTimer = 0;
    private Vector3 currentAngle;
    private AudioClip placeClip;
    private AudioClip triggerClip;
    private AudioClip gameOverClip;

    private GameObject chargeFx;

    private int hp;
    private int super;
    private int placeBallCount;
    private int triggerBallCount;

    // Use this for initialization
    void Start()
    {
        hp = 51;
        super = 0;
        placeBallCount = 5;
        triggerBallCount = 1;
        if (playerNumber == 1)
        {
            xAxis = "P1_Horizontal";
            yAxis = "P1_Vertical";
            placeButton = "P1_Place";
            triggerButton = "P1_Trigger";
            pushButton = "P1_Trigger";
            superButton = "P1_Super";
            currentAngle = new Vector3(1, 0);
            GetComponent<AudioSource>().pitch -= pitchModifier;
        }
        else
        {
            xAxis = "P2_Horizontal";
            yAxis = "P2_Vertical";
            placeButton = "P2_Place";
            triggerButton = "P2_Trigger";
            pushButton = "P2_Push";
            superButton = "P2_Super";
            currentAngle = new Vector3(-1, 0);
            GetComponent<AudioSource>().pitch += pitchModifier;
        }
        placeClip = Resources.Load<AudioClip>("Sounds/Throw1 - Dark Short");
        triggerClip = Resources.Load<AudioClip>("Sounds/Throw2 - Dark Short");
        gameOverClip = Resources.Load<AudioClip>("Sounds/BALLS2BALLS - Dark");
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
        bool IsPushButton = Input.GetButton(pushButton);
        bool pushUp = Input.GetButtonUp(pushButton);
        bool super = Input.GetButtonDown(superButton);
        

        Vector3 movement = new Vector3(horizontal, vertical, 0);
        if (movement.magnitude > 1) movement.Normalize();

        if (!IsPlaceButton && !IsTriggerButton && !IsPushButton)
        {
            transform.localPosition += movement * moveSpeed * Time.deltaTime;
            var pos = transform.localPosition;

            var limits = GameObject.FindGameObjectsWithTag("MoveLimit");
            foreach (GameObject limit in limits)
            {
                if (limit.GetComponent<MoveLimit>().playerNumber == playerNumber)
                {
                    var limitx = limit.transform.position.x;
                    if (playerNumber == 1 && transform.localPosition.x > limitx)
                    {
                        transform.localPosition = new Vector3(limitx, pos.y, pos.z);
                    }

                    if (playerNumber == 2 && transform.localPosition.x < limitx)
                    {
                        transform.localPosition = new Vector3(limitx, pos.y, pos.z);
                    }
                }
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
        placeChargeIndicator.transform.localScale = new Vector3(placeChargePct * 2, placeChargePct * 2, placeChargePct);
        placeChargeIndicator.transform.rotation = Quaternion.Euler(0, 0, rad * 180 / Mathf.PI);


        float triggerChargePct = Mathf.Min(ballDistFactor * triggerChargeTimer, ballDistMax - ballDistBase) / (ballDistMax - ballDistBase);
        triggerChargeIndicator.transform.localScale = new Vector3(triggerChargePct * 2, triggerChargePct * 2, triggerChargePct);
        triggerChargeIndicator.transform.rotation = Quaternion.Euler(0, 0, rad * 180 / Mathf.PI);



        if (placeUp && placeChargeTimer > 0)
        {

            GetComponent<AudioSource>().PlayOneShot(placeClip);
            var ball = Instantiate(Resources.Load("PlaceBall/PlaceBall"), Vector3.forward + transform.position + currentAngle.normalized * 0.4f, Quaternion.Euler(0, 0, -90 + rad * 180 / Mathf.PI)) as GameObject;
            var ballscript = ball.GetComponent<PlaceBall>();
            ballscript.startSpeed = Mathf.Min(ballSpeedBase + ballSpeedFactor * placeChargeTimer,ballSpeedMax);
            ballscript.playerNumber = playerNumber;
            RemovePlaceBall();
            Destroy(chargeFx);
        }
        if (triggerUp && triggerChargeTimer > 0)
        {
            GetComponent<AudioSource>().PlayOneShot(triggerClip);
            var target = currentAngle.normalized * (Mathf.Min(ballDistBase + ballDistFactor * triggerChargeTimer,ballDistMax));
            var ball = Instantiate(Resources.Load("TriggerBall"), transform.position, Quaternion.identity) as GameObject;
            var ballscript = ball.GetComponent<TriggerBall>();
            ballscript.SetTarget(transform.position + target);
            ballscript.playerNumber = playerNumber;
            RemoveTriggerBall();
            Destroy(chargeFx);
        }

        if (pushUp && pushChargeTimer > 0)
        {
            //GetComponent<AudioSource>().PlayOneShot(triggerClip);
            var push = Instantiate(Resources.Load("Push"), transform.position, Quaternion.Euler(0, 0, -90 + rad * 180 / Mathf.PI)) as GameObject;
            push.GetComponent<Push>().SetChargeFactor(pushChargeTimer);
            Destroy(chargeFx);
        }


        if (IsPlaceButton && triggerChargeTimer == 0 && pushChargeTimer == 0 && placeBallCount > 0)
        {
            if (placeChargeTimer == 0)
            {
                chargeFx = Instantiate(Resources.Load("Charge/ChargeEffect"), transform.position - Vector3.back * 0.5f + Vector3.down * 0.15f, Quaternion.identity) as GameObject;
            }
            placeChargeTimer += Time.deltaTime;
            
        }
        else
        {
            placeChargeTimer = 0;
        }

        if (IsTriggerButton && placeChargeTimer == 0 && pushChargeTimer == 0 && triggerBallCount > 0)
        {
            if (triggerChargeTimer == 0)
            {
                chargeFx = Instantiate(Resources.Load("Charge/ChargeEffect"), transform.position - Vector3.back * 0.5f + Vector3.down * 0.15f, Quaternion.identity) as GameObject;
            }
            triggerChargeTimer += Time.deltaTime;
        }
        else
        {
            triggerChargeTimer = 0;
        }

        if (IsPushButton && placeChargeTimer == 0 && triggerChargeTimer == 0)
        {
            if (pushChargeTimer == 0)
            {
                chargeFx = Instantiate(Resources.Load("Charge/ChargeEffect"), transform.position - Vector3.back * 0.5f + Vector3.down * 0.15f, Quaternion.identity) as GameObject;
            }
            pushChargeTimer += Time.deltaTime;
        }
        else
        {
            pushChargeTimer = 0;
        }


    }

    public void Damage()
    {
        hp--;
        CheckGameOver();
        updateUI();
    }

    public void CheckGameOver()
    {
        if (hp <= 0)
        {
            var canvas = GameObject.FindGameObjectWithTag("Canvas");
            var gameOverText = canvas.transform.FindChild("GameOver");
            var visibleText = gameOverText.GetComponentInChildren<Text>();
            GetComponent<AudioSource>().PlayOneShot(gameOverClip);
            if (playerNumber == 1)
            {
                visibleText.text = "Player 2 wins !";
            }
            else
            {
                visibleText.text = "Player 1 wins !";
            }
        }
        
        //traitement
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

    public void addSuper(int increment)
    {
        super = Mathf.Min(maxSuper, super + increment);
        updateUI();
    }

    private void updateUI()
    {
        var canvas = GameObject.FindGameObjectWithTag("Canvas");
        var ui = canvas.transform.FindChild("Player" + playerNumber);
        var uihp = ui.transform.FindChild("Hp");
        var text = uihp.GetComponentInChildren<Text>();
        text.text = "" + hp;
        var uisuper = ui.transform.FindChild("Super");
        var textsuper = uisuper.GetComponentInChildren<Text>();
        textsuper.text = "" + super + " / " + maxSuper;

        var uiballs = ui.transform.FindChild("Balls");
        for (int i = 0; i < 5; i++)
        {
            var ball = uiballs.FindChild("Ball" + (i + 1));
            ball.gameObject.SetActive(i < placeBallCount);
        }
    }
}