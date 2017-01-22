using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour {

    public float radius;
    public float angle;
    public float basePower;
    public float chargeModifier;

    private float chargeFactor = 0;

	// Use this for initialization
	void Start () {
        var balls = GameObject.FindGameObjectsWithTag("PlaceBall");
        foreach (GameObject ball in balls)
        {
            var p1 = new Vector2(ball.transform.position.x, ball.transform.position.y);
            var p2 = new Vector2(transform.position.x, transform.position.y);

            var v = p2 - p1;
            var delta = Vector3.Angle(v, - new Vector2(transform.up.x, transform.up.y));
            if (Vector3.Distance(p1, p2) < radius && delta < angle / 2)
            {
                Vector3 push = transform.up * (basePower + chargeFactor);
                ball.GetComponent < Rigidbody2D > ().velocity = new Vector2(push.x, push.y);
            }
        }

        var triggers = GameObject.FindGameObjectsWithTag("TriggerBall");
        foreach (GameObject trigger in triggers)
        {
            var p1 = new Vector2(trigger.transform.position.x, trigger.transform.position.y);
            var p2 = new Vector2(transform.position.x, transform.position.y);

            var v = p2 - p1;
            var delta = Vector3.Angle(v, -new Vector2(transform.up.x, transform.up.y));
            if (Vector3.Distance(p1, p2) < radius && delta < angle / 2)
            {
                Vector3 push = transform.up * (basePower + chargeFactor);
                trigger.GetComponent<TriggerBall>().Reset();
                trigger.GetComponent<TriggerBall>().SetTarget(transform.position + push);
            }
        }

        Destroy(gameObject, 0.5f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetChargeFactor(float timer)
    {
        chargeFactor = timer * chargeModifier;
    }
}
