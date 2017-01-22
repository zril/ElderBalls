using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour {

    public string bonusType;

    private float dirX;
    private float dirY;

    // Use this for initialization
    void Start() {
        dirX = Random.value - 0.5f;
        dirY = Random.value - 0.5f;
    }

    // Update is called once per frame
    void Update() {
        transform.position += new Vector3(dirX, dirY, 0).normalized * Time.deltaTime * 0.2f;
    }

    public void Apply(int playerNumber)
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        Player playerScript = null;
        foreach (GameObject player in players)
        {
            if (player.GetComponent<Player>().playerNumber == playerNumber)
            {
                playerScript = player.GetComponent<Player>();
            }
        }

        if (playerScript != null)
        {
            switch (bonusType)
            {
                case "bomb":
                    playerScript.AddPlaceBall();
                    break;
                case "speed":
                    playerScript.moveSpeed += 0.5f;
                    break;
                case "power":
                    playerScript.bombPowerFactor += 0.25f;
                    break;
                default:
                    break;
            }
        }

        Destroy(gameObject);
    }
}
