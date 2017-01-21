using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwavePart : MonoBehaviour
{
    public float speed = 2;
    public float lifeTime = 0.5f;

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
        transform.localScale += new Vector3(0.18f, 0, 0) * Time.deltaTime * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Kill();
        } else if (other.gameObject.tag == "PlaceBall")
        {
            other.GetComponent<PlaceBall>().Trigger();
        }
    }

    public void Kill()
    {
        GameObject.Destroy(gameObject);
    }
}
