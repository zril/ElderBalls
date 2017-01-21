using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    public int playerNumber = 1;
    public float pitchModificer = 0.1f;
    public int superIncr = 3;

	// Use this for initialization
	void Start () {
		if(playerNumber ==1)
        {
            GetComponent<AudioSource>().pitch -= pitchModificer;
        }
        else
        {
            GetComponent<AudioSource>().pitch += pitchModificer;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Damage()
    {
        GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/GoalHit"));
        var players = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject player in players)
        {
            var playerScript = player.GetComponent<Player>();
            if (playerScript.playerNumber == playerNumber)
            {
                playerScript.Damage();
                playerScript.addSuper(superIncr)
            }
        }
    }
}
