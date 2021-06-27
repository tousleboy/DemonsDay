using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    GameObject paint;
    GameObject wall;
    public static int progress = 0;
    public int individualNum = 1;
    AudioSource soundPlayer;
    public AudioClip sound;
    bool isPlayerIn;
    bool active;
    // Start is called before the first frame update
    void Start()
    {
        paint = transform.Find("paint").gameObject;
        wall = transform.Find("wall").gameObject;
        soundPlayer = GetComponent<AudioSource>();
        if(progress == individualNum) active = true;
        paint.SetActive(active);
        wall.SetActive(active);
        if(individualNum == 0)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxisRaw("Vertical") > 0 && !active && isPlayerIn)
            {
                soundPlayer.PlayOneShot(sound);
                paint.SetActive(true);
                active = true;
                progress = individualNum;
            }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isPlayerIn = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isPlayerIn = false;
        }
    }
}
