using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour {


    private int bombs = 0;
    private float bombsTimer = 0;
    private float bombsPeriod = 0.1f;

    // Use this for initialization
    void Start () {
        bombs = 10;
    }
	
	// Update is called once per frame
	void Update () {
        if (bombs > 0)
        {
            bombsTimer -= Time.deltaTime;
            if (bombsTimer < 0)
            {
                bomb();
                bombsTimer += bombsPeriod;
                bombs--;
            }
        }
    }

    private void bomb()
    {
        var ball = Instantiate(Resources.Load("PlaceBall/PlaceBall"), transform.position, Quaternion.Euler(new Vector3(0,0, Random.value * 360))) as GameObject;
        var ballscript = ball.GetComponent<PlaceBall>();
        ballscript.startSpeed = 0.5f;
        ballscript.playerNumber = 0;
    }
}
