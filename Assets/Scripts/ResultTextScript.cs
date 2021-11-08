using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultTextScript : MonoBehaviour
{
    public string mode;
    Text t;
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Text>();
        if(mode == "NumberofDefeats")
        {
            t.text = GameManager.defeats.ToString();
            GameManager.totalDefeats += GameManager.defeats;
            GameManager.defeats = 0;
        }
        else if(mode == "Money")
        {
            t.text = GameManager.score.ToString();
            GameManager.totalScore += GameManager.score;
            GameManager.score = 0;
        }
        else if(mode == "Retry")
        {
            t.text = GameManager.retry.ToString();
            GameManager.totalRetry += GameManager.retry;
            GameManager.retry = 0;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
