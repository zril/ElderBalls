using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour {

    private bool init = false;

	// Use this for initialization
	void Start () {
        Instantiate(Resources.Load("Maps/"+Global.arenaName), transform.position, Quaternion.identity);

        var spawnPlayer1 = GameObject.FindGameObjectWithTag("SpawnPlayer1");
        var spawnPlayer2 = GameObject.FindGameObjectWithTag("SpawnPlayer2");
        var spawnGoal1 = GameObject.FindGameObjectWithTag("SpawnGoal1");
        var spawnGoal2 = GameObject.FindGameObjectWithTag("SpawnGoal2");

        var player1 = Instantiate(Resources.Load("Player"), spawnPlayer1.transform.position, Quaternion.identity) as GameObject;
        player1.GetComponent<Player>().playerNumber = 1;
        var player2 = Instantiate(Resources.Load("Player"), spawnPlayer2.transform.position, Quaternion.identity) as GameObject;
        player2.GetComponent<Player>().playerNumber = 2;
        var goal1 = Instantiate(Resources.Load("Goal"), spawnGoal1.transform.position, Quaternion.identity) as GameObject;
        goal1.GetComponent<Goal>().playerNumber = 1;
        var goal2 = Instantiate(Resources.Load("Goal"), spawnGoal2.transform.position, Quaternion.identity) as GameObject;
        goal2.GetComponent<Goal>().playerNumber = 2;

        Destroy(spawnPlayer1);
        Destroy(spawnPlayer2);
        Destroy(spawnGoal1);
        Destroy(spawnGoal2);
    }
	
	// Update is called once per frame
	void Update () {

    }
}
