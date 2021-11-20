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
    public bool stopMusic = false;
    GameObject mp;
    // Start is called before the first frame update
    void Start()
    {
        mp = GameObject.FindGameObjectWithTag("Music");
        Debug.Log(mp + "mp");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load()
    {
        if(resetProgress)
        {
            CheckPointManager.progress = 0;
            GameManager.fadeInDone = false;
            GameManager.howManyLoad = 0;
        }
        //StartCoroutine(GoNextScene(delay));
        Invoke("GoNextScene", delay);
    }

    void GoNextScene()
    {
        //yield return new WaitForSecondsRealtime(d);
        if(mp != null)
        {
            /*if(continueMusic) DontDestroyOnLoad(mp);
            else SceneManager.MoveGameObjectToScene(mp, SceneManager.GetActiveScene());*/

            if(stopMusic)
            {
                SceneManager.MoveGameObjectToScene(mp, SceneManager.GetActiveScene());
            }
        }
        SceneManager.LoadScene(sceneName);
    }
}
