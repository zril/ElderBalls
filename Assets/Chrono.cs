using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chrono : MonoBehaviour {

    private float timer;

	// Use this for initialization
	void Start () {
        timer = 0;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        var canvas = GameObject.FindGameObjectWithTag("Canvas");
        var chrono = canvas.transform.FindChild("Chrono").GetComponent<Text>();

        int sec = Mathf.FloorToInt(timer);
        float dec = Mathf.FloorToInt((timer - (float)sec) * 100);

        string secstr = "";
        if (sec < 10)
        {
            secstr += "" + 0;
        }
        secstr += "" + sec;


        string decstr = "";
        if (dec < 10)
        {
            decstr += "" + 0;
        }
        decstr += "" + dec;
        chrono.text = "" + secstr + ":" + decstr;
    }
}
