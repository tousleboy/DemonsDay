using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkEventManager : MonoBehaviour
{
    GameObject player;
    PlayerController pc;

    public float range = 1.0f;
    public float delay = 0.0f;
    public string texts;    //Japanese texts should be devided with ' ', English ver has not been releaced
    
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
        if(transform.position.x - range <= playerX && transform.position.x + range >= playerX)
        {
            Invoke("SendTexts", delay);
            done = true;
        }
    }

    void SendTexts()
    {
        pc.texts = texts;
    }
}
