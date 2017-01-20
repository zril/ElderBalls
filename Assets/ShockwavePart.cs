using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwavePart : MonoBehaviour
{
    public float speed = 2;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += transform.up * Time.deltaTime * speed;
        transform.localScale += new Vector3(0.18f, 0, 0) * Time.deltaTime * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Kill();
            Debug.Log("kill");
        }
    }

    public void Kill()
    {
        GameObject.Destroy(gameObject);
    }
}
