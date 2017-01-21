using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    public int playerNumber = 1;
    public float moveSpeed = 2;

    string xAxis;
    string yAxis;
    string placeButton;
    string triggerButton;
    string superButton;


    private float chargeTimer = 0;

    // Use this for initialization
    void Start () {
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
        float vertical = Input.GetAxis(yAxis) ;
        bool placeDown = Input.GetButtonDown(placeButton);
        bool placeUp = Input.GetButtonUp(placeButton);
        bool trigger = Input.GetButtonDown(triggerButton);
        bool super = Input.GetButtonDown(superButton);

        chargeTimer += Time.deltaTime;

        transform.localPosition += new Vector3(horizontal, vertical, 0) * moveSpeed * Time.deltaTime;
        
        if (placeDown)
        {
            chargeTimer = 0;
        }

        Vector3 angle = new Vector3(horizontal, vertical, 0);

        Debug.Log(angle);
        var rad = Mathf.Atan2(angle.y, angle.x);
        Debug.Log(rad * 180 / Mathf.PI);

        if (placeUp)
        {
            Instantiate(Resources.Load("PlaceBall"), transform.position, Quaternion.Euler(0, 0, -90 + rad * 180 / Mathf.PI));
        }
    }
}
