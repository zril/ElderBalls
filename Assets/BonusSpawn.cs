using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawn : MonoBehaviour {

    private float spawnTimer;
    private float spawnPeriod = 10f;

	// Use this for initialization
	void Start () {
        spawnTimer = spawnPeriod * (1 + ((Random.value - 0.5f) * 2));
    }
	
	// Update is called once per frame
	void Update () {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer < 0)
        {
            spawnTimer += spawnPeriod;
            Spawn();
        }
    }

    private void Spawn()
    {
        var random = Random.value;
        if (random < 0.33f)
        {
            Instantiate(Resources.Load("Bonus/BonusBomb"), transform.position, Quaternion.identity);
        } else if (random < 0.66f)
        {
            Instantiate(Resources.Load("Bonus/BonusSpeed"), transform.position, Quaternion.identity);
        } else
        {
            Instantiate(Resources.Load("Bonus/BonusPower"), transform.position, Quaternion.identity);
        }
    }
}
