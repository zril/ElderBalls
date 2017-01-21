using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    public int playerNumber = 1;
    public float moveSpeed = 2;
    public float ballSpeedBase = 2;
    public float ballSpeedFactor = 2;
    public float ballDistBase = 2;
    public float ballDistFactor = 2;

    private KeyCode upKey;
    private KeyCode downKey;
    private KeyCode rightKey;
    private KeyCode leftKey;
    private KeyCode placeKey;
    private KeyCode triggerKey;
    private KeyCode superKey;

    private float placeChargeTimer = 0;
    private float triggerChargeTimer = 0;

    // Use this for initialization
    void Start () {
        if (playerNumber == 1)
        {
            upKey = KeyCode.UpArrow;
            downKey = KeyCode.DownArrow;
            rightKey = KeyCode.RightArrow;
            leftKey = KeyCode.LeftArrow;
            placeKey = KeyCode.RightControl;
            triggerKey = KeyCode.RightShift;
            superKey = KeyCode.Return;
        }
        if (playerNumber == 2)
        {
            upKey = KeyCode.E;
            downKey = KeyCode.D;
            rightKey = KeyCode.F;
            leftKey = KeyCode.S;
            placeKey = KeyCode.LeftControl;
            triggerKey = KeyCode.LeftShift;
            superKey = KeyCode.Space;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (placeChargeTimer == 0 && triggerChargeTimer == 0)
        {
            if (Input.GetKey(upKey))
            {
                transform.localPosition += new Vector3(0, 1, 0) * moveSpeed * Time.deltaTime;
            }

            if (Input.GetKey(downKey))
            {
                transform.localPosition += new Vector3(0, -1, 0) * moveSpeed * Time.deltaTime;
            }

            if (Input.GetKey(rightKey))
            {
                transform.localPosition += new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime;
            }

            if (Input.GetKey(leftKey))
            {
                transform.localPosition += new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime;
            }
        }
        

        Vector3 angle = new Vector3(0, 0, 0);
        if (Input.GetKey(upKey))
        {
            angle += new Vector3(0, 1, 0);
        }

        if (Input.GetKey(downKey))
        {
            angle += new Vector3(0, -1, 0);
        }

        if (Input.GetKey(rightKey))
        {
            angle += new Vector3(1, 0, 0);
        }

        if (Input.GetKey(leftKey))
        {
            angle += new Vector3(-1, 0, 0);
        }
        
        var rad = Mathf.Atan2(angle.y, angle.x);

        if (Input.GetKeyUp(placeKey))
        {
            Debug.Log(placeChargeTimer);
            var ball = Instantiate(Resources.Load("PlaceBall"), transform.position, Quaternion.Euler(0, 0, -90 + rad * 180 / Mathf.PI)) as GameObject;
            var ballscript = ball.GetComponent<PlaceBall>();
            ballscript.speed = ballSpeedBase + ballSpeedFactor * placeChargeTimer;
        }

        if (Input.GetKeyUp(triggerKey))
        {
            Debug.Log(triggerChargeTimer);
            var target = angle.normalized * (ballDistBase + ballDistFactor * triggerChargeTimer);
            var ball = Instantiate(Resources.Load("TriggerBall"), transform.position + target, Quaternion.identity) as GameObject;
        }


        if (Input.GetKey(placeKey) && triggerChargeTimer == 0)
        {
            placeChargeTimer += Time.deltaTime;
        }
        else
        {
            placeChargeTimer = 0;
        }

        if (Input.GetKey(triggerKey) && placeChargeTimer == 0)
        {
            triggerChargeTimer += Time.deltaTime;
        }
        else
        {
            triggerChargeTimer = 0;
        }
    }
}
