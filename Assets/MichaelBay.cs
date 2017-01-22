using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MichaelBay : MonoBehaviour {


    private int explosions = 0;
    private float explosionsTimer = 0;
    private float explosionsPeriod = 0.25f;

    // Use this for initialization
    void Start () {
        explosions = 10;
    }
	
	// Update is called once per frame
	void Update () {
        if (explosions > 0)
        {
            explosionsTimer -= Time.deltaTime;
            if (explosionsTimer < 0)
            {
                trigger();
                explosionsTimer += explosionsPeriod;
                explosions-=2;
            }
        }
    }

    private void trigger()
    {
        var pos = new Vector3(-6 + Random.value * 12, -3 + Random.value * 6, 0);
        var pos2 = new Vector3(-pos.x, -pos.y, 0);
        var ball = Instantiate(Resources.Load("ShockWave"), pos, Quaternion.identity) as GameObject;
        var ball2 = Instantiate(Resources.Load("ShockWave"), pos2, Quaternion.identity) as GameObject;
    }
}
