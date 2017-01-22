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
                for(int i = 0; i < 10; i++)
                {
                    InvokeRepeating("bombs", 0.2f, 0.5f);
                }
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

    private void bombs()
    {
        var ball = Instantiate(Resources.Load("PlaceBall/PlaceBall"), transform.position, Quaternion.identity) as GameObject;
        var ballscript = ball.GetComponent<PlaceBall>();
        ballscript.startSpeed = 1f;
        ballscript.playerNumber = 0;
    }
}
