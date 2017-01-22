using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Arena : MonoBehaviour
{

    private bool init = false;
    private float konamiTimer = 0.0f;
    private float konamiTrigger = 13.95f;
    private float konamiPeriod = 1.39f;

    // Use this for initialization
    void Start()
    {
        Global.winner = 0;
        if (Global.konamiCodeActive)
        {
            Instantiate(Resources.Load("KonamiMap/" + Global.arenaName), transform.position, Quaternion.identity);

        }
        else
        {
            Instantiate(Resources.Load("Maps/" + Global.arenaName), transform.position, Quaternion.identity);
        }

        var spawnPlayer1 = GameObject.FindGameObjectWithTag("SpawnPlayer1");
        var spawnPlayer2 = GameObject.FindGameObjectWithTag("SpawnPlayer2");
        var spawnGoal1 = GameObject.FindGameObjectWithTag("SpawnGoal1");
        var spawnGoal2 = GameObject.FindGameObjectWithTag("SpawnGoal2");
        var spawnMoveLimit1 = GameObject.FindGameObjectWithTag("MoveLimit1");
        var spawnMoveLimit2 = GameObject.FindGameObjectWithTag("MoveLimit2");

        var spawnBonusList = GameObject.FindGameObjectsWithTag("SpawnBonusSpawn");
        var spawnEventList = GameObject.FindGameObjectsWithTag("SpawnEventSpawn");

        var player1 = Instantiate(Resources.Load("Player"), spawnPlayer1.transform.position, Quaternion.identity) as GameObject;
        player1.GetComponent<Player>().playerNumber = 1;
        player1.transform.position = new Vector3(player1.transform.position.x, player1.transform.position.y, -1);
        var player2 = Instantiate(Resources.Load("Player"), spawnPlayer2.transform.position, Quaternion.identity) as GameObject;
        player2.GetComponent<Player>().playerNumber = 2;
        player2.transform.position = new Vector3(player2.transform.position.x, player2.transform.position.y, -1);

        if (Global.konamiCodeActive)
        {
            var player1Script = player1.GetComponent<Player>();
            player1Script.baseHp = 999;
            player1Script.maxPlaceBalls = 50;
            var player2Script = player2.GetComponent<Player>();
            player2Script.baseHp = 999;
            player2Script.maxPlaceBalls = 50;

        }

        var goal1 = Instantiate(Resources.Load("Goal1"), spawnGoal1.transform.position, Quaternion.identity) as GameObject;
        var goal2 = Instantiate(Resources.Load("Goal2"), spawnGoal2.transform.position, Quaternion.identity) as GameObject;
        var moveLimit1 = Instantiate(Resources.Load("MoveLimit"), spawnMoveLimit1.transform.position, Quaternion.identity) as GameObject;
        moveLimit1.GetComponent<MoveLimit>().playerNumber = 1;
        var moveLimit2 = Instantiate(Resources.Load("MoveLimit"), spawnMoveLimit2.transform.position, Quaternion.identity) as GameObject;
        moveLimit2.GetComponent<MoveLimit>().playerNumber = 2;

        foreach (GameObject spawnbonus in spawnBonusList)
        {
            var spawn = Instantiate(Resources.Load("Bonus/BonusSpawn"), spawnbonus.transform.position, Quaternion.identity) as GameObject;
            var param = spawnbonus.GetComponent<SpawnParam>();
            if (param != null)
            {
                spawn.GetComponent<BonusSpawn>().spawnPeriod = param.period;
                spawn.GetComponent<BonusSpawn>().firstPeriod = param.firstPeriod;
                spawn.GetComponent<BonusSpawn>().spawnType = param.spawnType;
            }

            Destroy(spawnbonus, 0.2f);
        }

        foreach (GameObject spawnevent in spawnEventList)
        {
            var spawn = Instantiate(Resources.Load("Event/EventSpawn"), spawnevent.transform.position, Quaternion.identity) as GameObject;

            var param = spawnevent.GetComponent<SpawnParam>();
            if (param != null)
            {
                spawn.GetComponent<EventSpawn>().spawnPeriod = param.period;
                spawn.GetComponent<EventSpawn>().firstPeriod = param.firstPeriod;
                spawn.GetComponent<EventSpawn>().spawnType = param.spawnType;
            }

            Destroy(spawnevent, 0.2f);
        }

        Destroy(spawnPlayer1);
        Destroy(spawnPlayer2);
        Destroy(spawnGoal1);
        Destroy(spawnGoal2);
        Destroy(spawnMoveLimit1);
        Destroy(spawnMoveLimit2);

        if (Global.konamiCodeActive)
        {
            GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Music/Konami");
        }
        else
        {
            GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Music/track" + Mathf.CeilToInt(Random.value * 5));
        }
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (Global.konamiCodeActive)
        {
            konamiTimer += Time.deltaTime;
            if (konamiTimer > konamiTrigger)
            {
                konamiTrigger += konamiPeriod *2;
                Debug.Log("Brain Power Balls " + konamiTimer);
                //DO STUFF
            }

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GetComponent<AudioSource>().Stop();
            SceneManager.LoadScene("main");
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<AudioSource>().Stop();
            SceneManager.LoadScene("Menu");
        }
    }
}
