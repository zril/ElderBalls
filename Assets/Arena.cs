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
        var spawnMoveLimit1 = GameObject.FindGameObjectWithTag("MoveLimit1");
        var spawnMoveLimit2 = GameObject.FindGameObjectWithTag("MoveLimit2");

        var spawnBonusList = GameObject.FindGameObjectsWithTag("SpawnBonusSpawn");
        var spawnEventList = GameObject.FindGameObjectsWithTag("SpawnEventSpawn");

        var player1 = Instantiate(Resources.Load("Player"), spawnPlayer1.transform.position, Quaternion.identity) as GameObject;
        player1.GetComponent<Player>().playerNumber = 1;
        player1.transform.position = new Vector3(player1.transform.position.x, player1.transform.position.y, -1);
        var player2 = Instantiate(Resources.Load("Player"), spawnPlayer2.transform.position, Quaternion.identity) as GameObject;
        player2.GetComponent<Player>().playerNumber = 2;
        player2.transform.position = new Vector3(player2.transform.position.x, player2.transform.position.y, -1);
        var goal1 = Instantiate(Resources.Load("Goal"), spawnGoal1.transform.position, Quaternion.identity) as GameObject;
        goal1.GetComponent<Goal>().playerNumber = 1;
        var goal2 = Instantiate(Resources.Load("Goal"), spawnGoal2.transform.position, Quaternion.identity) as GameObject;
        goal2.GetComponent<Goal>().playerNumber = 2;
        var moveLimit1 = Instantiate(Resources.Load("MoveLimit"), spawnMoveLimit1.transform.position, Quaternion.identity) as GameObject;
        moveLimit1.GetComponent<MoveLimit>().playerNumber = 1;
        var moveLimit2 = Instantiate(Resources.Load("MoveLimit"), spawnMoveLimit2.transform.position, Quaternion.identity) as GameObject;
        moveLimit2.GetComponent<MoveLimit>().playerNumber = 2;

        foreach(GameObject spawnbonus in spawnBonusList)
        {
            Instantiate(Resources.Load("Bonus/BonusSpawn"), spawnbonus.transform.position, Quaternion.identity);
            Destroy(spawnbonus, 0.2f);
        }

        foreach (GameObject spawnevent in spawnEventList)
        {
            Instantiate(Resources.Load("Event/EventSpawn"), spawnevent.transform.position, Quaternion.identity);
            Destroy(spawnevent, 0.2f);
        }

        Destroy(spawnPlayer1);
        Destroy(spawnPlayer2);
        Destroy(spawnGoal1);
        Destroy(spawnGoal2);
        Destroy(spawnMoveLimit1);
        Destroy(spawnMoveLimit2);

        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Music/track" + Mathf.CeilToInt(Random.value * 5));
        GetComponent<AudioSource>().Play();
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.R))
        {
            GetComponent<AudioSource>().Stop();
            Application.LoadLevel("main");
            //GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Music/track" + Mathf.CeilToInt(Random.value * 5));
            //GetComponent<AudioSource>().Play();
        }
    }
}
