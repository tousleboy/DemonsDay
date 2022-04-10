using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldManager : MonoBehaviour
{
    public GameObject gate0;
    public GameObject gate1;
    public GameObject gate2;
    public GameObject gate3;
    GameObject[] gates;
    public GameObject wall1;
    public GameObject wall2;
    public bool goal = false;

    bool eventStart = false;
    bool eventEnd = false;
    bool playerIn = false;
    bool roundEnd = true;
    public bool autoLock = true;
    public bool random = true;

    int round = 0;
    int maxRound = 0;

    //public GameObject[] enemies;
    public GameObject enemyBase1;
    public string[] comboes;
    public GameObject[] enemies;

    GameObject Player;
    GameObject mainCamera;

    // Start is called before the first frame update
    void Awake()
    {
        wall1.SetActive(false);
        wall2.SetActive(false);
        gates = new GameObject[4] {gate0, gate1, gate2, gate3};
        maxRound = enemies.Length;
        Debug.Log(maxRound);
        Player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        int i;
        for(i = 0; i < maxRound; i++) enemies[i].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(mainCamera.transform.position.x >= transform.position.x && !eventStart && !eventEnd && playerIn)
        {
            eventStart = true;
            mainCamera.GetComponent<CameraManager>().locked = true;
            PlayerController.concentration += 1;
        }

        if(eventStart)
        {
            if(autoLock)
            {
                wall1.SetActive(true);
                wall2.SetActive(true);
            }

            if(roundEnd)
            {
                round += 1;
                if(round > maxRound)
                {
                    eventStart = false;
                    eventEnd = true;
                    wall1.SetActive(false);
                    wall2.SetActive(false);
                    if(goal)
                    {
                        PlayerController.gameState = "gameclear";
                    }
                    return;
                }

                Vector3 pos;
                if(random)
                {
                    int n = Random.Range(0, 4);
                    pos = new Vector3(gates[n].transform.position.x, gates[n].transform.position.y, 0.0f);
                }
                else
                {
                    pos = gates[0].transform.position;
                    float d = 0f;
                    int i ;
                    for(i = 0; i < gates.Length; i++)
                    {
                        if(Vector3.Distance(Player.transform.position, gates[i].transform.position) > d)
                        {
                            pos = new Vector3(gates[i].transform.position.x, gates[i].transform.position.y, 0.0f);
                            d = Vector3.Distance(Player.transform.position, gates[i].transform.position);
                        }
                    }
                }
                /*GameObject enemy = Instantiate(enemyBase1, pos, Quaternion.identity);
                enemy.GetComponent<EnemyController>().comboAnime = comboes[round - 1];*/
                enemies[round - 1].transform.position = pos;
                enemies[round - 1].SetActive(true);
                roundEnd = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerIn = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            roundEnd = true;
        }
        if(collision.gameObject.tag == "Player" && eventEnd)
        {
            mainCamera.GetComponent<CameraManager>().resetPos = true;
            PlayerController.concentration -= 1;
        }
    }
}
