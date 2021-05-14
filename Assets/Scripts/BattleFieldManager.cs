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

    bool eventStart = false;
    bool eventEnd = false;
    bool roundEnd = true;
    bool autoLock = true;

    int round = 0;
    int maxRound = 0;

    //public GameObject[] enemies;
    public GameObject enemyBase1;
    public string[] comboes;

    GameObject Player;
    GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        wall1.SetActive(false);
        wall2.SetActive(false);
        gates = new GameObject[4] {gate0, gate1, gate2, gate3};
        maxRound = comboes.Length;
        Debug.Log(maxRound);
        Player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if(mainCamera.transform.position.x >= transform.position.x && !eventStart && !eventEnd)
        {
            eventStart = true;
            mainCamera.GetComponent<CameraManager>().locked = true;
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
                    return;
                }
                int n = Random.Range(0, 4);
                Vector3 pos = new Vector3(gates[n].transform.position.x, gates[n].transform.position.y, 0.0f);
                GameObject enemy = Instantiate(enemyBase1, pos, Quaternion.identity);
                enemy.GetComponent<EnemyController>().comboAnime = comboes[round - 1];
                roundEnd = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            roundEnd = true;
        }
        if(collision.gameObject.tag == "Player")
        {
            mainCamera.GetComponent<CameraManager>().resetPos = true;
        }
    }
}
