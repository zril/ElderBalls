using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour {

    List<GameObject> parts;

    private float powerFactor = 1;

	// Use this for initialization
	void Start () {
        parts = new List<GameObject>();
        for (int i = 0; i < 360; i += 10)
        {
            var part = Instantiate(Resources.Load("ShockwavePart"), transform.position, Quaternion.Euler(0, 0, i)) as GameObject;
            part.GetComponent<ShockwavePart>().SetPowerFactor(powerFactor);
            parts.Add(part);
        }
        Destroy(gameObject, 1f);
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void SetPowerFactor(float factor)
    {
        powerFactor = factor;
    }
}
