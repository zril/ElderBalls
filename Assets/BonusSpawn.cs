﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawn : MonoBehaviour {

    private float spawnTimer;
    public float spawnPeriod = 10f;
    public float firstPeriod = 10f;
    public string spawnType = "";

    // Use this for initialization
    void Start () {
        spawnTimer = firstPeriod * (1 + ((Random.value - 0.5f) * 0.2f));
    }
	
	// Update is called once per frame
	void Update () {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer < 0)
        {
            spawnTimer += spawnPeriod * (1 + ((Random.value - 0.5f) * 0.2f));
            Spawn();
        }
    }

    private void Spawn()
    {
        bool type = false;
        switch (spawnType)
        {
            case "bomb":
                Instantiate(Resources.Load("Bonus/BonusBomb"), transform.position, Quaternion.identity);
                type = true;
                break;
            case "speed":
                Instantiate(Resources.Load("Bonus/BonusSpeed"), transform.position, Quaternion.identity);
                type = true;
                break;
            case "power":
                Instantiate(Resources.Load("Bonus/BonusPower"), transform.position, Quaternion.identity);
                type = true;
                break;
            default:
                break;
        }

        if (!type)
        {
            var random = Random.value;
            if (random < 0.33f)
            {
                Instantiate(Resources.Load("Bonus/BonusBomb"), transform.position, Quaternion.identity);
            }
            else if (random < 0.66f)
            {
                Instantiate(Resources.Load("Bonus/BonusSpeed"), transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(Resources.Load("Bonus/BonusPower"), transform.position, Quaternion.identity);
            }
        }
    }
}
