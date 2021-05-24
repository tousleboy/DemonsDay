using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;
    public GameObject text;
    public GameObject headSet;
    public Sprite gameOverSpr;
    public Sprite gameClearSpr;
    public GameObject Life;
    public Sprite lifeZero;
    public Sprite lifeOne;
    public Sprite lifeTwo;
    public Sprite lifeThree;

    Image lifeImage;
    Text message;
    GameObject Boss;

    public bool bossIsGoal = true;
    public bool hsAlwaysActive = true;

    string messages;
    string nowMessages;
    string oldMessages;

    // Start is called before the first frame update
    void Start()
    {
        mainImage.SetActive(false);
        lifeImage = Life.GetComponent<Image>();
        message = text.GetComponent<Text>();

        if(hsAlwaysActive == false)
        {
            headSet.SetActive(false);
        }

        nowMessages = PlayerController.messages;
        oldMessages = PlayerController.messages;
    }

    // Update is called once per frame
    void Update()
    { 
        if(PlayerController.life >= 3)
        {
            lifeImage.sprite = lifeThree;
        }
        else if(PlayerController.life == 2)
        {
            lifeImage.sprite = lifeTwo;
        }
        else if(PlayerController.life == 1)
        {
            lifeImage.sprite = lifeOne;
        }
        else
        {
            lifeImage.sprite = lifeZero;
        }

        nowMessages = PlayerController.messages;
        if(nowMessages != oldMessages)
        {
            StartCoroutine("StoryTeller");
            oldMessages = nowMessages;
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
                PlayerController.gameState = "gameclear";
            }
        }
        if(PlayerController.gameState == "gameclear")
        {
            mainImage.GetComponent<Image>().sprite = gameClearSpr;
            mainImage.SetActive(true);
        }
    }

    IEnumerator StoryTeller()
    {
        string recieved = nowMessages;
        string[] messages = recieved.Split(' ');
        int length = messages.Length;
        int i;
        for(i = 0; i < length; i++)
        {
            message.text = messages[i];
            yield return new WaitForSeconds(messages[i].Length / 4);
            if(recieved != nowMessages)
            {
                yield break;
            }
        }
        message.text = "";
    }
}
