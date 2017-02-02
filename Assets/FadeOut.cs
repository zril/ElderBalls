using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour {

    public float startFadeTime = 1.0f;
    public float fadeTime = 2.0f;

    private float fadeTimer;
	// Use this for initialization
	void Start () {
        fadeTimer = fadeTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(startFadeTime > 0)
        {
            startFadeTime -= Time.deltaTime;
        }
        else
        {
            fadeTimer -= Time.deltaTime;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, fadeTimer / fadeTime);
            if(fadeTimer == 0)
            {
                Destroy(gameObject);
            }
        }
	}
}
