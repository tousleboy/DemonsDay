using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;
    public GameObject text;
    public GameObject headSet;
    public GameObject MoneyText;
    public GameObject Pannel;
    public Sprite gameOverSpr;
    public Sprite gameClearSpr;
    public GameObject Life;
    public Sprite lifeZero;
    public Sprite lifeOne;
    public Sprite lifeTwo;
    public Sprite lifeThree;

    Image lifeImage;
    Text message;
    Text money;
    GameObject Boss;
    AudioSource soundPlayer;
    Animator hsAnimator;
    public AudioClip piron;


    public bool bossIsGoal = true;
    public bool hsAlwaysActive = true;

    static int score = 0;

    string messages;
    string nowMessages;
    string oldMessages;
    // Start is called before the first frame update
    void Start()
    {
        mainImage.SetActive(false);
        Pannel.SetActive(false);
        lifeImage = Life.GetComponent<Image>();
        message = text.GetComponent<Text>();
        money = MoneyText.GetComponent<Text>();
        soundPlayer = headSet.GetComponent<AudioSource>();
        hsAnimator = headSet.GetComponent<Animator>();

        if(hsAlwaysActive == false)
        {
            headSet.SetActive(false);
        }

        nowMessages = PlayerController.messages;
        oldMessages = PlayerController.messages;

        UpdateScore();

        if(CheckPointManager.active == true)
        {
            GameObject cp = GameObject.FindGameObjectWithTag("Respawn");
            Vector3 pos = cp.transform.position;
            PlayerController.SpawnPos = pos;
        }
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

        if(PlayerController.score != 0)
        {
            score += PlayerController.score;
            PlayerController.score = 0;
            UpdateScore();
        }

        nowMessages = PlayerController.messages;
        if(nowMessages != oldMessages)
        {
            StartCoroutine("StoryTeller");
            oldMessages = nowMessages;
        }

        if(PlayerController.gameState == "gameover")
        {
            Pannel.SetActive(true);
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
            Pannel.SetActive(true);
            mainImage.GetComponent<Image>().sprite = gameClearSpr;
            mainImage.SetActive(true);
            CheckPointManager.active = false;
        }
    }

    void UpdateScore()
    {
        money.text = "¥" + score.ToString();
    }

    IEnumerator StoryTeller()
    {
        string recieved = nowMessages;
        string[] messages = recieved.Split(' ');
        int length = messages.Length;
        int i;
        for(i = 0; i < length; i++)
        {
            hsAnimator.SetTrigger("Call");
            message.text = messages[i];
            soundPlayer.PlayOneShot(piron);
            yield return new WaitForSeconds(messages[i].Length / 5.0f);
            if(recieved != nowMessages)
            {
                yield break;
            }
        }
        message.text = "";
    }
}
