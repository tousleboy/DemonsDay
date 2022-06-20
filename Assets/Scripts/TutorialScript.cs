using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public SpriteRenderer j;
    public SpriteRenderer k;
    public EnemyController ec;
    bool active = false;
    bool finish = false;
    int mode = 0;

    public string[] whenPunch;
    public string[] whenKick;
    public string gogo;
    public string end;

    int plen;
    int klen;
    int n = 0;
    int m = 0;
    // Start is called before the first frame update
    void Start()
    {
        plen = whenPunch.Length;
        klen = whenKick.Length;
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
                        n += 1;
                    }
                }
                else if(k.color == Color.white)
                {
                    Time.timeScale = 0.1f;
                    mode = 2;
                    if(m < klen)
                    {
                        PlayerController.messages = whenKick[m];
                        m += 1;
                    }
                }
            }
        }
        else
        {
            if(Time.timeScale != 0) Time.timeScale = 1;
            mode = 0;
        }

        if(mode == 1)
        {
            if(Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
            {
                Time.timeScale = 1f;
                mode = 3;
            }
        }
        else if(mode == 2)
        {
            if(Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
            {
                Time.timeScale = 1f;
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
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            active = false;
        }
    }

}
