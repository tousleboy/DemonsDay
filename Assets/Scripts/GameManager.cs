using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;
    public Sprite gameOverSpr;
    public Sprite gameClearSpr;
    public GameObject Life;
    public Sprite lifeZero;
    public Sprite lifeOne;
    public Sprite lifeTwo;
    public Sprite lifeThree;

    GameObject Boss;

    public bool bossIsGoal = true;


    // Start is called before the first frame update
    void Start()
    {
        mainImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    { 
        if(PlayerController.life >= 3)
        {
            Life.GetComponent<Image>().sprite = lifeThree;
        }
        else if(PlayerController.life == 2)
        {
            Life.GetComponent<Image>().sprite = lifeTwo;
        }
        else if(PlayerController.life == 1)
        {
            Life.GetComponent<Image>().sprite = lifeOne;
        }
        else
        {
            Life.GetComponent<Image>().sprite = lifeZero;
        }

        if(PlayerController.gameState == "gameover")
        {
            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            mainImage.SetActive(true);
        }
        if(bossIsGoal)
        {
            Boss = GameObject.FindGameObjectWithTag("Boss");
            if(Boss == null)
            {
                PlayerController.gameState = "clear";
                mainImage.GetComponent<Image>().sprite = gameClearSpr;
                mainImage.SetActive(true);
            }
        }
    }
}
