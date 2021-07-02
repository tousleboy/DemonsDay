using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangerScript : MonoBehaviour
{
    GameObject player;
    GameObject musicPlayer;
    public AudioClip music;
    bool activated = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        musicPlayer = GameObject.FindGameObjectWithTag("Music");
    }

    // Update is called once per frame
    void Update()
    {
        if(musicPlayer == null)
        {
            return;
        }
        if(player.transform.position.x > transform.position.x && !activated)
        {
            AudioSource auds = musicPlayer.GetComponent<AudioSource>();
            if(auds.clip != music)
            {
                StartCoroutine(AudioChange(auds));
            }
            activated = true;
        }
    }

    IEnumerator AudioChange(AudioSource auds)
    {
        float i;
        float downspeed = 0.01f;
        float upspeed = 0.01f;
        for(i = 1.0f; i >= 0.0f; i -= downspeed)
        {
            auds.volume = i;
            yield return null;
        }
        auds.clip = music;
        auds.Play();
        for(i = 0.0f; i <= 1.0f; i += upspeed)
        {
            auds.volume = i;
            yield return null;
        }
    }
}
