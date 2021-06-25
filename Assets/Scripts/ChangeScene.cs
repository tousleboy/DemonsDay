using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string sceneName;
    public float delay = 0.0f;
    public bool resetProgress = true;
    public bool continueMusic = true;
    GameObject mp;
    // Start is called before the first frame update
    void Start()
    {
        mp = GameObject.FindGameObjectWithTag("Music");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load()
    {
        if(resetProgress)
        {
            CheckPointManager.active = false;
        }
        Invoke("GoNextScene", delay);
    }

    void GoNextScene()
    {
        if(continueMusic && mp != null)
        {
            DontDestroyOnLoad(mp);
        }
        SceneManager.LoadScene(sceneName);
    }
}
