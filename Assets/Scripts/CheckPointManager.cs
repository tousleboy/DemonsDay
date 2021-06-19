using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    GameObject paint;
    public static bool active = false;
    AudioSource soundPlayer;
    public AudioClip sound;
    bool isPlayerIn;
    // Start is called before the first frame update
    void Start()
    {
        paint = transform.Find("paint").gameObject;
        soundPlayer = GetComponent<AudioSource>();
        paint.SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxisRaw("Vertical") > 0 && !active && isPlayerIn)
            {
                soundPlayer.PlayOneShot(sound);
                paint.SetActive(true);
                active = true;
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
