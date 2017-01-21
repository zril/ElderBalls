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
    
    private float placeChargeTimer = 0;
    private float triggerChargeTimer = 0;

    private int hp;
    private int placeBallCount;

    // Use this for initialization
    void Start()
    {
        hp = 51;
        placeBallCount = 5;

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
        

        Vector3 movement = new Vector3(horizontal, vertical, 0);
        if (movement.magnitude > 1) movement.Normalize();

        if (!IsPlaceButton && !IsTriggerButton)
        {
            transform.localPosition += movement * moveSpeed * Time.deltaTime;
        }

        Vector3 angle = new Vector3(horizontal, vertical, 0).normalized;
        
        var rad = Mathf.Atan2(angle.y, angle.x);

        if (placeUp && placeChargeTimer > 0)
        {
            var ball = Instantiate(Resources.Load("PlaceBall"), transform.position, Quaternion.Euler(0, 0, -90 + rad * 180 / Mathf.PI)) as GameObject;
            var ballscript = ball.GetComponent<PlaceBall>();
            ballscript.startSpeed = Mathf.Min(ballSpeedBase + ballSpeedFactor * placeChargeTimer,ballSpeedMax);
            ballscript.playerNumber = playerNumber;
            RemoveBall();
        }
        if (triggerUp)
        {
            var target = angle.normalized * (Mathf.Min(ballDistBase + ballDistFactor * triggerChargeTimer, ballDistMax));
            var ball = Instantiate(Resources.Load("TriggerBall"), transform.position, Quaternion.identity) as GameObject;
            ball.GetComponent<TriggerBall>().SetTarget(transform.position + target);
        }


        if (IsPlaceButton && triggerChargeTimer == 0 && placeBallCount > 0)
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

    public void Damage()
    {
        hp--;
        updateUI();
    }

    public void AddBall()
    {
        placeBallCount++;
        updateUI();
    }

    public void RemoveBall()
    {
        placeBallCount--;
        updateUI();
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