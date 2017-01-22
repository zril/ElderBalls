using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour {

    public string eventType;

    private EventSpawn parent;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void Activate()
    {
        switch (eventType)
        {
            case "bomb":
                Instantiate(Resources.Load("BombSpawner"), transform.position, Quaternion.identity);
                break;
            case "trigger":
                //todo
                break;
            case "detonate":
                //todo
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
