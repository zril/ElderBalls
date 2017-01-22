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

    // Use this for initialization
    void Start()
    {
        prefabs = Resources.LoadAll<GameObject>("Maps");
        currentIndex = 0;
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

        bool down = Input.GetKeyDown(KeyCode.LeftArrow) || (previousVertical >= 0 && vertical < 0);
        bool up = Input.GetKeyDown(KeyCode.RightArrow) || (previousVertical <= 0 && vertical > 0);
        bool valid = Input.GetButtonDown("P1_Place");
        bool cancel = Input.GetButtonDown("P1_Trigger");


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

        if (valid)
        {
            Global.arenaName = prefabs[currentIndex].name;
            SceneManager.LoadScene("main");
        }

        previousHorizontal = horizontal;
        previousVertical = vertical;
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
