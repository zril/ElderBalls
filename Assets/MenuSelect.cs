using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSelect : MonoBehaviour
{

    GameObject[] prefabs;
    private int currentIndex;
    private GameObject currentMap = null;
    private float previousHorizontal;
    private float previousVertical;

    char[] KonamiCodeSeq = { 'u', 'u', 'd', 'd', 'l', 'r', 'l', 'r', 'b', 'a' };
    private int konamiIndex = 0;

    // Use this for initialization
    void Start()
    {
        prefabs = Resources.LoadAll<GameObject>("Maps");
        currentIndex = 0;
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Music/KonamiIntro");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMap == null)
        {
            if (Input.anyKey)
            {
                var howTo  = GameObject.FindGameObjectWithTag("HowTo");
                Destroy(howTo);
                showCurrentMap();
            }
        }
        float horizontal = Input.GetAxis("P1_Horizontal");
        float vertical = Input.GetAxis("P1_Vertical");
        bool left = Input.GetKeyDown(KeyCode.LeftArrow) || (previousHorizontal >= 0 && horizontal < 0);
        bool right = Input.GetKeyDown(KeyCode.RightArrow) || (previousHorizontal <= 0 && horizontal > 0);

        bool down = Input.GetKeyDown(KeyCode.DownArrow) || (previousVertical >= 0 && vertical < 0);
        bool up = Input.GetKeyDown(KeyCode.UpArrow) || (previousVertical <= 0 && vertical > 0);
        bool valid = Input.GetButtonDown("P1_Place") || Input.GetKeyDown(KeyCode.A);
        bool cancel = Input.GetButtonDown("P1_Trigger") || Input.GetKeyDown(KeyCode.B); ;


        if (left || down)
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = prefabs.Length - 1;
            GameObject.Destroy(currentMap);
            showCurrentMap();
        }
        else if (right || up)
        {
            currentIndex++;
            if (currentIndex >= prefabs.Length) currentIndex = 0;
            GameObject.Destroy(currentMap);
            showCurrentMap();
        }


        previousHorizontal = horizontal;
        previousVertical = vertical;
        //KonamiCode
        if (up || down || left || right || valid || cancel)
        {
            if (up && KonamiCodeSeq[konamiIndex] == 'u' || down && KonamiCodeSeq[konamiIndex] == 'd' || left && KonamiCodeSeq[konamiIndex] == 'l' || right && KonamiCodeSeq[konamiIndex] == 'r' || valid && KonamiCodeSeq[konamiIndex] == 'a' || cancel && KonamiCodeSeq[konamiIndex] == 'b')
            {
                konamiIndex++;
                //Debug.Log(konamiIndex);
                if (konamiIndex >= KonamiCodeSeq.Length)
                {
                    konamiIndex = 0;
                    Global.konamiCodeActive = !Global.konamiCodeActive;
                }
            }
            else
            {
                konamiIndex = 0;
            }
            Debug.Log(konamiIndex);
        }

        if(Global.konamiCodeActive && !GetComponent<AudioSource>().isPlaying)
        {
            prefabs = Resources.LoadAll<GameObject>("KonamiMap");
            currentIndex = 0;
            GameObject.Destroy(currentMap);
            showCurrentMap();
            GetComponent<AudioSource>().Play();
        }
        else if (!Global.konamiCodeActive && GetComponent<AudioSource>().isPlaying)
        {
            prefabs = Resources.LoadAll<GameObject>("Maps");
            currentIndex = 0;
            GameObject.Destroy(currentMap);
            showCurrentMap();
            GetComponent<AudioSource>().Stop();
        }
        else
        if (valid)
        {
            Global.arenaName = prefabs[currentIndex].name;
            SceneManager.LoadScene("main");
        }
    }

    void showCurrentMap()
    {
        currentMap = Instantiate(prefabs[currentIndex]);

        var canvas = GameObject.FindGameObjectWithTag("Canvas");
        var ui = canvas.transform.FindChild("MapName");
        var text = ui.GetComponentInChildren<Text>();
        text.text = "< " + prefabs[currentIndex].name + " >";

    }

}
