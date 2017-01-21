using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwavePart : MonoBehaviour
{
    public float speed = 2;
    public float lifeTime = 0.5f;
    public float pushSpeed = 0.1f;

    private float timer;

    // Use this for initialization
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            Kill();
        }

        transform.localPosition += transform.up * Time.deltaTime * speed;
        transform.localScale += new Vector3(0.15f, 0, 0) * Time.deltaTime * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Kill();
        } else if (other.gameObject.tag == "PlaceBall")
        {
            other.GetComponent<PlaceBall>().Trigger();
            var pushtmp = pushSpeed * transform.up;
            var push = new Vector2(pushtmp.x, pushtmp.y);
            other.transform.GetComponent<Rigidbody2D>().velocity += push;
        }
        else if (other.gameObject.tag == "Goal")
        {
            Kill();
            other.GetComponent<Goal>().Damage();
        } else if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Player>().addSuper(other.GetComponent<Player>().damageBlockSuperIncr);
            Kill();
        }
    }

    public void Kill()
    {
        GameObject.Destroy(gameObject);
    }
}
