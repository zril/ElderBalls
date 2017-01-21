using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    public int playerNumber = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Damage()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject player in players)
        {
            var playerScript = player.GetComponent<Player>();
            if (playerScript.playerNumber == playerNumber)
            {
                playerScript.Damage();
            }
        }
    }
}
