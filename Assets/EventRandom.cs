using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRandom : MonoBehaviour {
    private float dirX;
    private float dirY;

    private EventSpawn parent;
    // Use this for initialization
    void Start()
    {
        dirX = Random.value - 0.5f;
        dirY = Random.value - 0.5f;


    }

    // Update is called once per frame
    void Update()
    {
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

        var itemNumber = Random.Range(0, 6);
        // 0 -- 2 Bonus : Bomb, Speed, Power
        // 3 -- 5 Event : Bomb, Potion,Explo

        switch(itemNumber)
        {
            case 0:
                if (playerScript != null)
                    playerScript.AddBonusPlaceBall(5);
                break;
            case 1:
                if (playerScript != null)
                    playerScript.moveSpeed += 0.5f;
                break;
            case 2:
                if (playerScript != null)
                    playerScript.startBonusBomb();
                break;
            case 3:
                Instantiate(Resources.Load("BombSpawner"), transform.position, Quaternion.identity);
                break;
            case 4:
                Instantiate(Resources.Load("TriggerSpawner"), transform.position, Quaternion.identity);
                break;
            case 5:
                Instantiate(Resources.Load("ExplosionSpawner"), transform.position, Quaternion.identity);
                break;
            default:
                break;
        }


        parent.UnFreeze();

        Destroy(gameObject);
    }


    public void SetParent(EventSpawn parent)
    {
        this.parent = parent;
    }
}
