using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public SpriteRenderer j;
    public SpriteRenderer k;
    public GameObject slowLogo;
    public EnemyController ec;
    public PlayerController pc;
    bool active = false;
    bool finish = false;
    int mode = 0;

    public string[] whenPunch;
    public string[] whenKick;
    public string gogo;
    public string end;

    public static bool onceCalled = false;

    public GameObject talkE;

    int plen;
    int klen;
    int n = 0;
    int m = 0;
    // Start is called before the first frame update
    void Start()
    {
        plen = whenPunch.Length;
        klen = whenKick.Length;
        slowLogo.SetActive(false);
        if(onceCalled) talkE.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(finish) return;

        if(ec.damaged)
        {
            finish = true;
            PlayerController.messages = end;
        }

        if(ec.gap && PlayerController.messages != gogo)
        {
            PlayerController.messages = gogo;
        }

        if(pc.damaged)
        {
            Time.timeScale = 1.0f;
            slowLogo.SetActive(false);
        }

        if(active)
        {
            if(mode == 0)
            {
                if(j.color == Color.white)
                {
                    Time.timeScale = 0.1f;
                    mode = 1;
                    if(n < plen)
                    {
                        PlayerController.messages = whenPunch[n];
                        n = (n + 1) % plen;
                    }
                    slowLogo.SetActive(true);
                }
                else if(k.color == Color.white)
                {
                    Time.timeScale = 0.1f;
                    mode = 2;
                    if(m < klen)
                    {
                        PlayerController.messages = whenKick[m];
                        m = (m + 1) % klen;
                    }
                    slowLogo.SetActive(true);
                }
            }
        }
        else
        {
            if(Time.timeScale != 0)
            {
                Time.timeScale = 1;
                slowLogo.SetActive(false);
            }
            mode = 0;
        }

        if(mode == 1)
        {
            if(Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
            {
                Time.timeScale = 1f;
                slowLogo.SetActive(false);
                mode = 3;
            }
        }
        else if(mode == 2)
        {
            if(Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
            {
                Time.timeScale = 1f;
                slowLogo.SetActive(false);
                mode = 4;
            }
        }
        else if(mode == 3)
        {
            if(j.color != Color.white) mode = 0;
        }
        else if(mode == 4)
        {
            if(k.color != Color.white) mode = 0;
        }
        else{}
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            active = true;
            onceCalled = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            active = false;
            slowLogo.SetActive(false);
        }
    }

}
