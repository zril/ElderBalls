using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SuperBase : MonoBehaviour {

    public int playerNumber;
    public float chargeFactor;
    public float startSpeed;

    public bool bomb;
    public bool potion;
    public bool knife;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected abstract void superBomb();

    protected abstract void superPotion();

    protected abstract void superKnife();
}
