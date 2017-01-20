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
            parts.Add(Instantiate(Resources.Load("ShockwavePart"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, i)) as GameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
        /*for(int i = 0; i < parts.Count; i++)
        {
            var i2 = (i + 1) % parts.Count;
            DrawLine(parts[i].transform.position, parts[i2].transform.position, Color.white, 0.1f);
        }*/
	}


    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
}
