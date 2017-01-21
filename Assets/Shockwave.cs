using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour {

    List<GameObject> parts;

	// Use this for initialization
	void Start () {
        parts = new List<GameObject>();
        for (int i = 0; i < 360; i += 10)
        {
            parts.Add(Instantiate(Resources.Load("ShockwavePart"), transform.position, Quaternion.Euler(0, 0, i)) as GameObject);
        }
        Destroy(gameObject, 1f);
    }
	
	// Update is called once per frame
	void Update () {

	}
}
