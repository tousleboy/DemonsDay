using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkEventManager : MonoBehaviour
{
    GameObject player;
    PlayerController pc;

    public float range = 1.0f;
    public float height = 100.0f;
    public float delay = 0.0f;
    public string texts;    //Japanese texts should be devided with ' ', English ver has not been releaced
    public bool isEvent = false;
    public float eventLength = 3.0f;
    public bool occurEveryTime = true;
    public int when = 1;
    
    bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            pc = player.GetComponent<PlayerController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(done)
        {
            return;
        }
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;
        if(transform.position.x - range <= playerX && transform.position.x + range >= playerX && playerY >= transform.position.y && playerY <= transform.position.y + height)
        {
            if(occurEveryTime || when == GameManager.howManyLoad)
            {
                Invoke("SendTexts", delay);
                done = true;
                if(isEvent && PlayerController.messages != texts)
                {
                    pc.Wait();
                    Invoke("EventEnd", eventLength);
                }
            }
        }
    }

    void SendTexts()
    {
        PlayerController.messages = texts;
    }

    void EventEnd()
    {
        pc.StopWait();
    }
}
