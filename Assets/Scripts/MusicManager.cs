using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip mainTheme;
    public GameObject musicPlayerPrefab;
    public bool loop = true;
    GameObject mp;
    // Start is called before the first frame update
    void Start()
    {
        mp = GameObject.FindGameObjectWithTag("Music");
        if(mp == null)
        {
            mp = Instantiate(musicPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
        DontDestroyOnLoad(mp);

        AudioSource auds = mp.GetComponent<AudioSource>();
        if(auds.clip != mainTheme)
        {
            auds.clip = mainTheme;
            auds.volume = 1.0f;
            auds.Play();
        }

        auds.loop = loop;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
