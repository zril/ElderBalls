using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpawn : MonoBehaviour {

    private float spawnTimer;
    public float spawnPeriod = 10f;
    public float firstPeriod = 10f;
    public string spawnType = "";

    private bool freeze = false;

    // Use this for initialization
    void Start()
    {
        spawnTimer = firstPeriod * (1 + ((Random.value - 0.5f) * 0.2f));
    }

    // Update is called once per frame
    void Update()
    {
        if (!freeze)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer < 0)
            {
                spawnTimer += spawnPeriod * (1 + ((Random.value - 0.5f) * 0.2f));
                Spawn();
            }
        }
        
    }

    private void Spawn()
    {
        GameObject ev = null;

        switch (spawnType)
        {
            case "trigger":
                ev = Instantiate(Resources.Load("Event/EventTrigger"), transform.position, Quaternion.identity) as GameObject;
                break;
            case "bomb":
                ev = Instantiate(Resources.Load("Event/EventBomb"), transform.position, Quaternion.identity) as GameObject;
                break;
            case "explosion":
                ev = Instantiate(Resources.Load("Event/EventExplosion"), transform.position, Quaternion.identity) as GameObject;
                break;
            case "random":
                ev = Instantiate(Resources.Load("Event/EventRandom"), transform.position, Quaternion.identity) as GameObject;
                break;
            default:
                break;
        }

        if (ev == null)
        {
            var random = Random.value;
            if (random < 0.33f)
            {
                ev = Instantiate(Resources.Load("Event/EventTrigger"), transform.position, Quaternion.identity) as GameObject;
            }
            else if (random < 0.66f)
            {
                ev = Instantiate(Resources.Load("Event/EventBomb"), transform.position, Quaternion.identity) as GameObject;
            }
            else
            {
                ev = Instantiate(Resources.Load("Event/EventExplosion"), transform.position, Quaternion.identity) as GameObject;
            }
        }

        var eventScript = ev.GetComponent<Event>();
        if (eventScript != null)
            eventScript.SetParent(this);

        var randEventScript = ev.GetComponent<EventRandom>();
        if (randEventScript != null)
            randEventScript.SetParent(this);


        freeze = true;
    }

    public void UnFreeze()
    {
        freeze = false;
    }
}
