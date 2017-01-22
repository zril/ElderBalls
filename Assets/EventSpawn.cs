using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpawn : MonoBehaviour {

    private float spawnTimer;
    private float spawnPeriod = 5f;

    private bool freeze = false;

    // Use this for initialization
    void Start()
    {
        spawnTimer = spawnPeriod * (1 + ((Random.value - 0.5f) * 2));
    }

    // Update is called once per frame
    void Update()
    {
        if (!freeze)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer < 0)
            {
                spawnTimer += spawnPeriod;
                Spawn();
            }
        }
        
    }

    private void Spawn()
    {
        GameObject ev = null;
        var random = Random.value;
        if (random < 0.33f)
        {
            ev = Instantiate(Resources.Load("Event/EventBomb"), transform.position, Quaternion.identity) as GameObject;
            ev.GetComponent<Event>().eventType = "bomb";
        }
        else if (random < 0.66f)
        {
            ev = Instantiate(Resources.Load("Event/EventBomb"), transform.position, Quaternion.identity) as GameObject;
        }
        else
        {
            ev = Instantiate(Resources.Load("Event/EventBomb"), transform.position, Quaternion.identity) as GameObject;
        }

        ev.GetComponent<Event>().SetParent(this);
        freeze = true;
    }

    public void UnFreeze()
    {
        freeze = false;
    }
}
